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
    public class ApplicationDataProvider
    {
        private readonly q100_authorizationContext _context;

        [ExcludeFromCodeCoverage]
        public ApplicationDataProvider(
            q100_authorizationContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// insert single application data
        /// </summary>
        /// <param name="notification"></param>
        /// <returns>inserted data</returns>
        public async Task<Application> Insert(Application data)
        {
            await _context.Application.AddAsync(data);
            await _context.SaveChangesAsync();
            return data;
        }

        /// <summary>
        /// Update Application Data
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Application</returns>
        public async Task<Application> Update(Application data)
        {
            Application result = new Application();
            NowTimestamp nowTimestamp = _context.NowTimestamp.FromSqlRaw(Q100AUAMAuthorizationConstant.GET_DB_CURRENT_TIMESTAMP_QUERY).FirstOrDefault();
            DateTime endDate = nowTimestamp.CurrentTimestamp.AddDays(-1);
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //deactivate current data
                    Application currentData = await GetActiveApplicationByCode(data.ApplicationCode);
                    if (currentData == null)
                    {
                        currentData = await GetPlannedApplicationByCode(data.ApplicationCode);
                        currentData.UpdatedAt = nowTimestamp.CurrentTimestamp;
                        currentData.BeginDate = endDate;
                        currentData.EndDate = endDate;
                    }
                    else
                    {
                        currentData.UpdatedAt = nowTimestamp.CurrentTimestamp;
                        currentData.EndDate = endDate;
                    }
                    _context.Application.Update(currentData);
                    await _context.SaveChangesAsync();

                    //insert updated data
                    Application newData = new Application()
                    {
                        ApplicationCode = data.ApplicationCode,
                        ApplicationName = data.ApplicationName,
                        BeginDate = data.BeginDate,
                        EndDate = data.EndDate,
                        CreatedBy = data.UpdatedBy,
                        UpdatedBy = data.UpdatedBy
                    };
                    await _context.Application.AddAsync(newData);
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
        /// get active application by code
        /// </summary>
        /// <param name="applicationCode"></param>
        /// <param name="nowTimestamp"></param>
        /// <returns>application</returns>
        public async Task<Application> GetActiveApplicationByCode(string applicationCode)
        {
            NowTimestamp nowTimestamp = _context.NowTimestamp.FromSqlRaw(Q100AUAMAuthorizationConstant.GET_DB_CURRENT_TIMESTAMP_QUERY).FirstOrDefault();

            return await
            (
                from data in _context.Application
                where
                    data.BeginDate <= nowTimestamp.CurrentTimestamp &&
                    data.EndDate >= nowTimestamp.CurrentTimestamp &&
                    data.ApplicationCode == applicationCode
                select new Application
                {
                    Id = data.Id,
                    ApplicationCode = data.ApplicationCode,
                    ApplicationName = data.ApplicationName,
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
        /// get planned application by code
        /// </summary>
        /// <param name="applicationCode"></param>
        /// <param name="nowTimestamp"></param>
        /// <returns>application</returns>
        public async Task<Application> GetPlannedApplicationByCode(string applicationCode)
        {
            NowTimestamp nowTimestamp = _context.NowTimestamp.FromSqlRaw(Q100AUAMAuthorizationConstant.GET_DB_CURRENT_TIMESTAMP_QUERY).FirstOrDefault();

            return await
            (
                from data in _context.Application
                where
                    data.BeginDate > nowTimestamp.CurrentTimestamp &&
                    data.ApplicationCode == applicationCode
                select new Application
                {
                    Id = data.Id,
                    ApplicationCode = data.ApplicationCode,
                    ApplicationName = data.ApplicationName,
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