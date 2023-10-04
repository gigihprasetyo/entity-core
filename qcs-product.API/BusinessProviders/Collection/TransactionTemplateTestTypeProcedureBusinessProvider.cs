using Google.Type;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using qcs_product.API.BindingModels;
using qcs_product.API.DataProviders;
using qcs_product.API.DataProviders.Collection;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders.Collection
{
    public class TransactionTemplateTestTypeProcedureBusinessProvider : ITransactionTemplateTestTypeProcedureBusinessProvider
    {
        private readonly QcsProductContext _dbContext;

        private readonly ITransactionTemplateTestTypeProcessProcedureDataProvider _dataProvider;
        private readonly ITransactionTemplateTestTypeProcessProcedureParameterDataProvider _procedureParameterDataProvider;
        public TransactionTemplateTestTypeProcedureBusinessProvider(QcsProductContext dbContext, ITransactionTemplateTestTypeProcessProcedureDataProvider dataProvider, ITransactionTemplateTestTypeProcessProcedureParameterDataProvider procedureParameterDataProvider)
        {
            _dbContext = dbContext;
            _dataProvider = dataProvider;
            _procedureParameterDataProvider = procedureParameterDataProvider;
        }

        public async Task<ResponseViewModel<TransactionTemplateTestTypeProcessProcedure>> GetAllTransactionTemplateTestTypeProcedures()
        {

            ResponseViewModel<TransactionTemplateTestTypeProcessProcedure> result = new ResponseViewModel<TransactionTemplateTestTypeProcessProcedure>();   
            var getData = _dataProvider.GetAllTransactionTemplateTestTypeProcessProcedures();
            var getDataParameter =  _procedureParameterDataProvider.GetAllTransactionTmpltTestTypeProcessProcedureParameters();

            foreach (var procedure in getData)
            {
                IEnumerable<TransactionTemplateTestTypeProcessProcedureParameter> param = from m in getDataParameter where m.TestTypeProcessPrecedureId == procedure.Id select m;

                if (param != null)
                {
                    foreach (var item in param)
                    {
                        procedure.TransactionTemplateTestTypeProcessProcedureParameter.Add(item);
                    }
                }
            }

            if (getData == null || !getData.Any())
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
                return result;
            }

            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;

            result.Data = getData;
            return result;

        }

        public ActionResult<TransactionTemplateTestTypeProcessProcedure> GetTransactionTemplateTestTypeProcedureById(int id)
        {
            return _dataProvider.GetTransactionTmpltTestTypeProcessProcedureById(id);
        }

        public ResponseViewModel<TransactionTemplateTestTypeProcessProcedure> GetTransactionTemplateTestTypeProcedureByIdTrxTesting(int id)
        {

            ResponseViewModel<TransactionTemplateTestTypeProcessProcedure> result = new ResponseViewModel<TransactionTemplateTestTypeProcessProcedure>();
            var getData = _dataProvider.GetTransactionTemplateTestTypeProcedureByIdTrxTesting(id);
            if (getData == null|| !getData.Any())
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
                return result;
            }

            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            result.Data = getData;
            return result;
        }

        public async Task<IActionResult> AddTransactionTemplateTestTypeProcedure(TransactionTemplateTestTypeProcessProcedure procedure )
        {


            _dataProvider.AddTransactionTemplateTestTypeProcessProcedure(procedure);
            return new NoContentResult();
         

        }

        public async Task<ResponseViewModel<TransactionTemplateTestTypeProcessProcedure>> UpdateTransactionTemplateTestTypeProcedure(int id, TransactionTemplateTestTypeProcessProcedure data)
        {

            ResponseViewModel<TransactionTemplateTestTypeProcessProcedure> result = new ResponseViewModel<TransactionTemplateTestTypeProcessProcedure>();

            try
            {
                var existingData = _dataProvider.GetTransactionTmpltTestTypeProcessProcedureById(id);

                if (existingData == null)
                {
                    result.StatusCode = 404;
                    result.Message = "Data not found";
                    return result;
                }

                _dataProvider.UpdateTransactionTemplateTestTypeProcessProcedure(id, data);

                var list = new List<TransactionTemplateTestTypeProcessProcedure> { existingData };
                result.StatusCode = 200;
                result.Message = "Data updated successfully";
                result.Data = list;
                return result;
            }  catch (Exception ex)
            {

                result.StatusCode = 500;
                result.Message = "An error occurred during the update";
                return result;
            }
        }

        public async Task<IActionResult> DeleteTransactionTemplateTestTypeProcessProcedure(int id)
        {
            _dataProvider.DeleteTransactionTemplateTestTypeProcessProcedure(id);
            return new NoContentResult();
        }

      
    
    }
}
