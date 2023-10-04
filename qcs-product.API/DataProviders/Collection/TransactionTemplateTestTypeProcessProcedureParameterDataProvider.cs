using Newtonsoft.Json;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;

namespace qcs_product.API.DataProviders.Collection
{
    public class TransactionTemplateTestTypeProcessProcedureParameterDataProvider : ITransactionTemplateTestTypeProcessProcedureParameterDataProvider
    {
        private readonly QcsProductContext _dbContext;

        public TransactionTemplateTestTypeProcessProcedureParameterDataProvider(QcsProductContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<TransactionTemplateTestTypeProcessProcedureParameter> GetAllTransactionTmpltTestTypeProcessProcedureParameters()
        {
         /*   return _dbContext.TransactionTemplateTestTypeProcessProcedureParameter.ToList();
*/

            return (from x in _dbContext.TransactionTemplateTestTypeProcessProcedureParameter
                    select new TransactionTemplateTestTypeProcessProcedureParameter
                    {
          
            DeviationAttachment = x.DeviationAttachment !=null? x.DeviationAttachment:0,
            DeviationLevel  = x.DeviationLevel!=null? x.DeviationLevel:0 ,
            HasAttachment = x.HasAttachment!=null? x.HasAttachment:false,
            InputTypeId = x.InputTypeId!=null? x.InputTypeId:0,
            IsNullable = x.IsNullable!=null? x.IsNullable:false ,
            Properties = x.Properties ?? JsonConvert.SerializeObject(x.Properties) ,
            PropertiesValue = x.PropertiesValue ?? JsonConvert.SerializeObject(x.Properties),
            Sequence = x.Sequence != null ? x.Sequence : 0,
                        DeviationNote = x.DeviationNote,
                        TestTypeProcessPrecedureCode = x.TestTypeProcessPrecedureCode ,
                        RowStatus =x.RowStatus ,
            ComponentName =x.ComponentName ,
            CreatedBy = x.CreatedBy,
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                        UpdatedBy = x.UpdatedBy,
                        TestTypeProcessPrecedureId = x.TestTypeProcessPrecedureId,
                    }).ToList();
        }

        public TransactionTemplateTestTypeProcessProcedureParameter GetTransactionTemplateTestTypeProcessProcedureParameterById(int id)
        {

            var getData = GetAllTransactionTmpltTestTypeProcessProcedureParameters();

            IEnumerable<TransactionTemplateTestTypeProcessProcedureParameter> param = from m in getData where m.Id == id select m;

            if (param != null)
            {
                return param.FirstOrDefault();
            }
            else { return null; }
        }

        public void AddTransactionTemplateTestTypeProcessProcedureParameter(TransactionTemplateTestTypeProcessProcedureParameter parameter)
        {
            _dbContext.TransactionTemplateTestTypeProcessProcedureParameter.Add(parameter);
            _dbContext.SaveChanges();
        }

        public void UpdateTransactionTemplateTestTypeProcessProcedureParameter(TransactionTemplateTestTypeProcessProcedureParameter parameter)
        {
            _dbContext.TransactionTemplateTestTypeProcessProcedureParameter.Update(parameter);
            _dbContext.SaveChanges();
        }

        public void DeleteTransactionTemplateTestTypeProcessProcedureParameter(int id)
        {
            var parameter = _dbContext.TransactionTemplateTestTypeProcessProcedureParameter.FirstOrDefault(p => p.Id == id);
            if (parameter != null)
            {
                _dbContext.TransactionTemplateTestTypeProcessProcedureParameter.Remove(parameter);
                _dbContext.SaveChanges();
            }
        }
    }
}
