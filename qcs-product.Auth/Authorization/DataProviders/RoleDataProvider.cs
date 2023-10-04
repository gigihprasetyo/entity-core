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
    public class RoleDataProvider
    {
        private readonly q100_authorizationContext _context;

        [ExcludeFromCodeCoverage]
        public RoleDataProvider(
            q100_authorizationContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// insert single endpoint data
        /// </summary>
        /// <param name="notification"></param>
        /// <returns>inserted data</returns>
        public async Task<Role> Insert(Role data)
        {
            await _context.Role.AddAsync(data);
            await _context.SaveChangesAsync();
            return data;
        }

        /// <summary>
        /// insert position to role data
        /// </summary>
        /// <param name="data"></param>
        /// <returns>position to role</returns>
        public async Task<PositionToRole> InsertPositionToRole(PositionToRole data)
        {
            await _context.PositionToRole.AddAsync(data);
            await _context.SaveChangesAsync();
            return data;
        }

        /// <summary>
        /// get position to role data
        /// </summary>
        /// <param name="data"></param>
        /// <returns>position to role</returns>
        public async Task<PositionToRole> GetPositionToRoleById(int id)
        {
            return await _context.PositionToRole.FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Update Role Data
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Role</returns>
        public async Task<Role> Update(Role data)
        {
            Role result = new Role();
            NowTimestamp nowTimestamp = _context.NowTimestamp.FromSqlRaw(Q100AUAMAuthorizationConstant.GET_DB_CURRENT_TIMESTAMP_QUERY).FirstOrDefault();
            DateTime endDate = nowTimestamp.CurrentTimestamp.AddDays(-1);
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //deactivate current data
                    Role currentData = await GetActiveRoleByCode(data.RoleCode);
                    if (currentData == null)
                    {
                        currentData = await GetPlannedRoleByCode(data.RoleCode);
                        currentData.UpdatedAt = nowTimestamp.CurrentTimestamp;
                        currentData.BeginDate = endDate;
                        currentData.EndDate = endDate;
                    }
                    else
                    {
                        currentData.UpdatedAt = nowTimestamp.CurrentTimestamp;
                        currentData.EndDate = endDate;
                    }
                    _context.Role.Update(currentData);
                    await _context.SaveChangesAsync();

                    //insert updated data
                    Role newData = new Role()
                    {
                        ApplicationCode = data.ApplicationCode,
                        RoleCode = data.RoleCode,
                        RoleName = data.RoleName,
                        BeginDate = data.BeginDate,
                        EndDate = data.EndDate,
                        CreatedBy = data.UpdatedBy,
                        UpdatedBy = data.UpdatedBy
                    };
                    await _context.Role.AddAsync(newData);
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
        /// Update Position To Role Data
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Role</returns>
        public async Task<PositionToRole> UpdatePositionToRole(PositionToRole data)
        {
            NowTimestamp nowTimestamp = _context.NowTimestamp.FromSqlRaw(Q100AUAMAuthorizationConstant.GET_DB_CURRENT_TIMESTAMP_QUERY).FirstOrDefault();
            try
            {
                PositionToRole currentData = await _context.PositionToRole.FindAsync(data.Id);
                currentData.ApplicationCode = data.ApplicationCode;
                currentData.RoleCode = data.RoleCode;
                currentData.PosId = data.PosId;
                currentData.Name = data.Name;
                currentData.RowStatus = data.RowStatus;
                currentData.UpdatedBy = data.UpdatedBy;
                currentData.UpdatedAt = nowTimestamp.CurrentTimestamp;
                _context.PositionToRole.Update(currentData);
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
        public async Task<Role> GetActiveRoleByCode(string endpointCode)
        {
            NowTimestamp nowTimestamp = _context.NowTimestamp.FromSqlRaw(Q100AUAMAuthorizationConstant.GET_DB_CURRENT_TIMESTAMP_QUERY).FirstOrDefault();
            return await
            (
                from data in _context.Role
                where
                    data.BeginDate <= nowTimestamp.CurrentTimestamp &&
                    data.EndDate >= nowTimestamp.CurrentTimestamp &&
                    data.RoleCode == endpointCode
                select new Role
                {
                    Id = data.Id,
                    ApplicationCode = data.ApplicationCode,
                    RoleCode = data.RoleCode,
                    RoleName = data.RoleName,
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
        public async Task<Role> GetPlannedRoleByCode(string endpointCode)
        {
            NowTimestamp nowTimestamp = _context.NowTimestamp.FromSqlRaw(Q100AUAMAuthorizationConstant.GET_DB_CURRENT_TIMESTAMP_QUERY).FirstOrDefault();
            return await
            (
                from data in _context.Role
                where
                    data.BeginDate > nowTimestamp.CurrentTimestamp &&
                    data.RoleCode == endpointCode
                select new Role
                {
                    Id = data.Id,
                    ApplicationCode = data.ApplicationCode,
                    RoleCode = data.RoleCode,
                    RoleName = data.RoleName,
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