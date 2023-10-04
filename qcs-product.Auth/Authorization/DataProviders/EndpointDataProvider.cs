using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using qcs_product.Auth.Authorization.Infrastructure;
using qcs_product.Auth.Authorization.Models;
using qcs_product.Auth.Authorization.Constants;

namespace qcs_product.Auth.Authorization.DataProviders
{
    public class EndpointDataProvider
    {
        private readonly q100_authorizationContext _context;

        [ExcludeFromCodeCoverage]
        public EndpointDataProvider(
            q100_authorizationContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// insert single endpoint data
        /// </summary>
        /// <param name="notification"></param>
        /// <returns>inserted data</returns>
        public async Task<Endpoint> Insert(Endpoint data)
        {
            await _context.Endpoint.AddAsync(data);
            await _context.SaveChangesAsync();
            return data;
        }

        /// <summary>
        /// insert single role to endpoint data
        /// </summary>
        /// <param name="data"></param>
        /// <returns>inserted data</returns>

        public async Task<RoleToEndpoint> InsertRoleToEndpoint(RoleToEndpoint data)
        {
            await _context.RoleToEndpoint.AddAsync(data);
            await _context.SaveChangesAsync();
            return data;
        }

        /// <summary>
        /// get role to endpoint data
        /// </summary>
        /// <param name="data"></param>
        /// <returns>role to endpoint</returns>
        public async Task<RoleToEndpoint> GetRoleToEndpointById(int id)
        {
            return await _context.RoleToEndpoint.FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Update Endpoint Data
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Endpoint</returns>
        public async Task<Endpoint> Update(Endpoint data)
        {
            Endpoint result = new Endpoint();
            NowTimestamp nowTimestamp = _context.NowTimestamp.FromSqlRaw(Q100AUAMAuthorizationConstant.GET_DB_CURRENT_TIMESTAMP_QUERY).FirstOrDefault();
            DateTime endDate = nowTimestamp.CurrentTimestamp.AddDays(-1);
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //deactivate current data
                    Endpoint currentData = await GetActiveEndpointByCode(data.EndpointCode);
                    if (currentData == null)
                    {
                        currentData = await GetPlannedEndpointByCode(data.EndpointCode);
                        currentData.UpdatedAt = nowTimestamp.CurrentTimestamp;
                        currentData.BeginDate = endDate;
                        currentData.EndDate = endDate;
                    }
                    else
                    {
                        currentData.UpdatedAt = nowTimestamp.CurrentTimestamp;
                        currentData.EndDate = endDate;
                    }
                    _context.Endpoint.Update(currentData);
                    await _context.SaveChangesAsync();

                    //insert updated data
                    Endpoint newData = new Endpoint()
                    {
                        ApplicationCode = data.ApplicationCode,
                        EndpointCode = data.EndpointCode,
                        EndpointName = data.EndpointName,
                        EndpointPath = data.EndpointPath,
                        BeginDate = data.BeginDate,
                        EndDate = data.EndDate,
                        CreatedBy = data.UpdatedBy,
                        UpdatedBy = data.UpdatedBy
                    };
                    await _context.Endpoint.AddAsync(newData);
                    await _context.SaveChangesAsync();

                    transaction.Commit();
                    result = newData;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
            return result;
        }

        /// <summary>
        /// Update Role To Endpoint Data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="nowTimestamp"></param>
        /// <returns>Endpoint</returns>
        public async Task<RoleToEndpoint> UpdateRoleToEndpoint(RoleToEndpoint data)
        {
            NowTimestamp nowTimestamp = _context.NowTimestamp.FromSqlRaw(Q100AUAMAuthorizationConstant.GET_DB_CURRENT_TIMESTAMP_QUERY).FirstOrDefault();
            try
            {
                RoleToEndpoint currentData = await _context.RoleToEndpoint.FindAsync(data.Id);
                currentData.ApplicationCode = data.ApplicationCode;
                currentData.RoleCode = data.RoleCode;
                currentData.EndpointCode = data.EndpointCode;
                currentData.RowStatus = data.RowStatus;
                currentData.UpdatedBy = data.UpdatedBy;
                currentData.UpdatedAt = nowTimestamp.CurrentTimestamp;
                _context.RoleToEndpoint.Update(currentData);
                await _context.SaveChangesAsync();
                return currentData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// get active endpoint by code
        /// </summary>
        /// <param name="endpointCode"></param>
        /// <returns>endpoint</returns>
        public async Task<Endpoint> GetActiveEndpointByCode(string endpointCode)
        {
            NowTimestamp nowTimestamp = _context.NowTimestamp.FromSqlRaw(Q100AUAMAuthorizationConstant.GET_DB_CURRENT_TIMESTAMP_QUERY).FirstOrDefault();
            return await
            (
                from data in _context.Endpoint
                where
                    data.BeginDate <= nowTimestamp.CurrentTimestamp &&
                    data.EndDate >= nowTimestamp.CurrentTimestamp &&
                    data.EndpointCode == endpointCode
                select new Endpoint
                {
                    Id = data.Id,
                    ApplicationCode = data.ApplicationCode,
                    EndpointCode = data.EndpointCode,
                    EndpointName = data.EndpointName,
                    EndpointPath = data.EndpointPath,
                    BeginDate = data.BeginDate,
                    EndDate = data.EndDate,
                    CreatedAt = data.CreatedAt,
                    CreatedBy = data.CreatedBy,
                    UpdatedAt = data.UpdatedAt,
                    UpdatedBy = data.UpdatedBy
                }
            ).FirstOrDefaultAsync();
        }

        /// <summary>
        /// get planned endpoint by code
        /// </summary>
        /// <param name="endpointCode"></param>
        /// <returns>endpoint</returns>
        public async Task<Endpoint> GetPlannedEndpointByCode(string endpointCode)
        {
            NowTimestamp nowTimestamp = _context.NowTimestamp.FromSqlRaw(Q100AUAMAuthorizationConstant.GET_DB_CURRENT_TIMESTAMP_QUERY).FirstOrDefault();
            return await
            (
                from data in _context.Endpoint
                where
                    data.BeginDate > nowTimestamp.CurrentTimestamp &&
                    data.EndpointCode == endpointCode
                select new Endpoint
                {
                    Id = data.Id,
                    ApplicationCode = data.ApplicationCode,
                    EndpointCode = data.EndpointCode,
                    EndpointName = data.EndpointName,
                    EndpointPath = data.EndpointPath,
                    BeginDate = data.BeginDate,
                    EndDate = data.EndDate,
                    CreatedAt = data.CreatedAt,
                    CreatedBy = data.CreatedBy,
                    UpdatedAt = data.UpdatedAt,
                    UpdatedBy = data.UpdatedBy
                }
            ).FirstOrDefaultAsync();
        }
    }
}