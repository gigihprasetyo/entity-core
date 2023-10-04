using Microsoft.EntityFrameworkCore;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace qcs_product.API.DataProviders.Collection
{
    public class TransactionTestingSamplingDataProvider : ITransactionTestingSamplingDataProvider
    {
        private readonly QcsProductContext _dbContext;
   
        public TransactionTestingSamplingDataProvider(QcsProductContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TransactionTestingSampling> GetById(int id)
        {
            return await _dbContext.TransactionTestingSampling.FindAsync(id);
        }

        public async Task<List<TransactionTestingSampling>> GetAll()
        {
            return await _dbContext.TransactionTestingSampling.ToListAsync();
        } 
        public async Task<List<TransactionTestingSampling>> GetByTestingId(int testingId)
        {
            return await (from x in _dbContext.TransactionTestingSampling
             where x.TestingId == testingId
             select x).ToListAsync();
        }

        public async Task Create(TransactionTestingSampling entity)
        {
            _dbContext.TransactionTestingSampling.Add(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(TransactionTestingSampling entity)
        {
            _dbContext.TransactionTestingSampling.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var entity = await GetById(id);
            if (entity != null)
            {
                _dbContext.TransactionTestingSampling.Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
