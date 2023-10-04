using Microsoft.EntityFrameworkCore;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using qcs_product.API.ViewModels;

namespace qcs_product.API.DataProviders.Collection
{
    public class TransactionTestTypeMethodValidationParameterDataProvider : ITransactionTestTypeMethodValidationParameterDataProvider
    {
   
        private readonly QcsProductContext _dbContext;

        public TransactionTestTypeMethodValidationParameterDataProvider(QcsProductContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<TransactionTestTypeMethodValidationParameter>> GetAll()
        {
            return await _dbContext.Set<TransactionTestTypeMethodValidationParameter>().ToListAsync();
        }

        public async Task<TransactionTestTypeMethodValidationParameter> GetById(int id)
        {
            return await _dbContext.Set<TransactionTestTypeMethodValidationParameter>().FindAsync(id);
        }

        public async Task Add(TransactionTestTypeMethodValidationParameter entity)
        {
            await _dbContext.Set<TransactionTestTypeMethodValidationParameter>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(TransactionTestTypeMethodValidationParameter entity)
        {
            _dbContext.Set<TransactionTestTypeMethodValidationParameter>().Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(TransactionTestTypeMethodValidationParameter entity)
        {
            _dbContext.Set<TransactionTestTypeMethodValidationParameter>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<TransactionMethodValidationParameterViewModel>> GetByIdTestingId(int id)
        {

            var result = await (from val in _dbContext.TransactionTestTypeMethodValidationParameters
                              join param in _dbContext.TransactionTestingProcedureParameters on val.TransactionTestingProcedureParameterId equals param.Id
                                where val.TestingId == id
                                select new TransactionMethodValidationParameterViewModel
                                {
                                   PropertiesValue = JsonConvert.SerializeObject(param.PropertiesValue).ToString(),
                                    Id = val.Id,
                                    NeedAttachment = val.NeedAttachment,
                                    Properties = val.Properties,
                                    CreatedAt = val.CreatedAt,
                                    IsNullable = val.IsNullable,
                                    InputTypeId = val.InputTypeId,
                                    Code = val.Code,
                                    Sequence = val.Sequence,
                                    TestTypeMethodCode = val.TestTypeMethodCode,
                                    RowStatus = val.RowStatus,
                                    CreatedBy = val.CreatedBy,
                                    UpdatedAt = val.UpdatedAt,
                                    Name = val.Name,
                                    IsInstruction = val.IsInstruction,
                                    AttachmentFile = val.AttachmentFile,
                                    Instruction = val.Instruction,
                                    IsExisting = val.IsExisting,
                                    ValidationResult = val.ValidationResult,
                                    TransactionTestingProcedureParameterId = val.TransactionTestingProcedureParameterId,
                                    TestingId = val.TestingId,
                              
                                
                                }).ToListAsync();


            return result;
        }
    }
}
