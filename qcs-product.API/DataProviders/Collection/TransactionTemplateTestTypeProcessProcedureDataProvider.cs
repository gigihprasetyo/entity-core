using Google.Rpc;
using Google.Type;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace qcs_product.API.DataProviders.Collection
{
    public class TransactionTemplateTestTypeProcessProcedureDataProvider : ITransactionTemplateTestTypeProcessProcedureDataProvider
    {
        private readonly QcsProductContext _dbContext;
        private readonly ITransactionTemplateTestTypeProcessProcedureParameterDataProvider _procedureParameterDataProvider;
        private readonly ITransactionTemplateTestTypeProcessProcedureParameterDataProvider transactionTemplateTestTypeProcessProcedureParameterDataProvider;

        public TransactionTemplateTestTypeProcessProcedureDataProvider(QcsProductContext dbContext, ITransactionTemplateTestTypeProcessProcedureParameterDataProvider procedureParameterDataProvider)
        {
            _dbContext = dbContext;
            _procedureParameterDataProvider = procedureParameterDataProvider;
        }

        public List<TransactionTemplateTestTypeProcessProcedure> GetAllTransactionTemplateTestTypeProcessProcedures()
        {
            //return _dbContext.TransactionTemplateTestTypeProcessProcedure.ToList();
            return (from x in _dbContext.TransactionTemplateTestTypeProcessProcedure select new TransactionTemplateTestTypeProcessProcedure
            {
              
                CreatedAt = x.CreatedAt,
                UpdatedBy = x.UpdatedBy,
                TestTypeProcessId = x.TestTypeProcessId,
                TransactionTemplateTestingId = x.TransactionTemplateTestingId,
                TestTypeProcessCode = x.TestTypeProcessCode,
                UpdatedAt = x.UpdatedAt,
                CreatedBy = x.CreatedBy,
                Title = x.Title,
                Id = x.Id,
                Instruction = x.Instruction,
                AttachmentFile = x.AttachmentFile,
                RowStatus = x.RowStatus,
                AttachmentStorageName = x.AttachmentStorageName,
                TransactionTemplateTestTypeProcessProcedureParameter = new HashSet<TransactionTemplateTestTypeProcessProcedureParameter>()

            }).ToList();
        }

        public TransactionTemplateTestTypeProcessProcedure GetTransactionTmpltTestTypeProcessProcedureById(int id)
        {
            var getData = GetAllTransactionTemplateTestTypeProcessProcedures();

            IEnumerable<TransactionTemplateTestTypeProcessProcedure> param = from m in getData where m.Id == id select m;

            if (param != null)
            {
                return param.FirstOrDefault();
            }else { return null; }
              
        }

        public List<TransactionTemplateTestTypeProcessProcedure> GetTransactionTemplateTestTypeProcedureByIdTrxTesting(int id)
        {
            List<TransactionTemplateTestTypeProcessProcedure> result = new List<TransactionTemplateTestTypeProcessProcedure>();

            var getData = GetAllTransactionTemplateTestTypeProcessProcedures();
            var getDataParameter = _procedureParameterDataProvider.GetAllTransactionTmpltTestTypeProcessProcedureParameters();

            foreach (var procedure in getData)
            {
                if (procedure.TransactionTemplateTestingId == id)
                {
                    IEnumerable<TransactionTemplateTestTypeProcessProcedureParameter> param = from m in getDataParameter where m.TestTypeProcessPrecedureId == procedure.Id select m;

                    if (param != null)
                    {
                        foreach (var item in param)
                        {
                            procedure.TransactionTemplateTestTypeProcessProcedureParameter.Add(item);
                        }
                        result.Add(procedure);
                       
                    }
                }
            }
            return result;
        }

        public void AddTransactionTemplateTestTypeProcessProcedure(TransactionTemplateTestTypeProcessProcedure procedure)
        {
            _dbContext.TransactionTemplateTestTypeProcessProcedure.Add(procedure);
            _dbContext.SaveChanges();
        }

        public void UpdateTransactionTemplateTestTypeProcessProcedure(int id, TransactionTemplateTestTypeProcessProcedure data)
        {

         //   var existingData =   GetTransactionTmpltTestTypeProcessProcedureById(id);



            using (var transaction = _dbContext.Database.BeginTransaction())
            {

                var existingData = (from x in _dbContext.TransactionTemplateTestTypeProcessProcedure
                                    where x.Id == id
                                    select new TransactionTemplateTestTypeProcessProcedure
                 {

                     CreatedAt = x.CreatedAt,
                     UpdatedBy = x.UpdatedBy,
                     TestTypeProcessId = x.TestTypeProcessId,
                     TransactionTemplateTestingId = x.TransactionTemplateTestingId,
                     TestTypeProcessCode = x.TestTypeProcessCode,
                     UpdatedAt = x.UpdatedAt,
                     CreatedBy = x.CreatedBy,
                     Title = x.Title,
                     Id = x.Id,
                     Instruction = x.Instruction,
                     AttachmentFile = x.AttachmentFile,
                     RowStatus = x.RowStatus,
                     AttachmentStorageName = x.AttachmentStorageName,
                     //TransactionTemplateTestTypeProcessProcedureParameter = new HashSet<TransactionTemplateTestTypeProcessProcedureParameter>()

                 }).FirstOrDefault();

                try
                {

            existingData.CreatedAt = data.CreatedAt;
            existingData.UpdatedBy = data.UpdatedBy;
            existingData.TestTypeProcessId = data.TestTypeProcessId;
            existingData.TransactionTemplateTestingId = data.TransactionTemplateTestingId;
            existingData.TestTypeProcessCode = data.TestTypeProcessCode;
            existingData.UpdatedAt = data.UpdatedAt;
            existingData.CreatedBy = data.CreatedBy;
            existingData.Title = data.Title;
            existingData.Instruction = data.Instruction;
            existingData.AttachmentFile = data.AttachmentFile;
            existingData.RowStatus = data.RowStatus;
            existingData.AttachmentStorageName = data.AttachmentStorageName;
            existingData.UpdatedAt = System.DateTime.Now;

                    _dbContext.Update(existingData);
                    _dbContext.SaveChanges();
                   
                    foreach (var parameter in data.TransactionTemplateTestTypeProcessProcedureParameter)
            {
                var getdata = _procedureParameterDataProvider.GetTransactionTemplateTestTypeProcessProcedureParameterById(parameter.Id);

                if (getdata != null)
                {

                    getdata.Code = parameter.Code;
                    getdata.DeviationAttachment = parameter.DeviationAttachment;
                    getdata.DeviationLevel = parameter.DeviationLevel;
                    getdata.DeviationNote= parameter.DeviationNote;
                    getdata.HasAttachment = parameter.HasAttachment;
                    getdata.InputTypeId = parameter.InputTypeId;
                    getdata.IsNullable=parameter.IsNullable;
                    getdata.TestTypeProcessPrecedureId = parameter.TestTypeProcessPrecedureId;
                    getdata.TestTypeProcessPrecedureCode = parameter.TestTypeProcessPrecedureCode;
                    getdata.Properties= parameter.Properties;
                    getdata.PropertiesValue=parameter.PropertiesValue;
                    getdata.RowStatus=parameter.RowStatus;
                    getdata.ComponentName = parameter.ComponentName;
                    getdata.Sequence=parameter.Sequence;
                    getdata.UpdatedBy=parameter.UpdatedBy;
                    getdata.CreatedBy = parameter.CreatedBy;
                    getdata.UpdatedAt= System.DateTime.Now;
                            

                            _dbContext.Update(getdata);
                            _dbContext.SaveChanges();

                            transaction.Commit();

                        }
            }

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
        }

        public void DeleteTransactionTemplateTestTypeProcessProcedure(int id)
        {
            var procedure = _dbContext.TransactionTemplateTestTypeProcessProcedure.FirstOrDefault(p => p.Id == id);
            if (procedure != null)
            {
                _dbContext.TransactionTemplateTestTypeProcessProcedure.Remove(procedure);
                _dbContext.SaveChanges();
            }
        }

      
    }
}
