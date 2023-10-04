using Microsoft.Extensions.Logging;
using qcs_product.API.DataProviders;
using qcs_product.API.ViewModels;
using qcs_product.API.Models;
using qcs_product.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using qcs_product.API.BindingModels;
using qcs_product.API.Helpers;

namespace qcs_product.API.BusinessProviders.Collection
{
    public class QcProcessBusinessProvider : IQcProcessBusinessProvider
    {
        private readonly IQcProcessDataProvider _dataProvider;
        private readonly ILogger<ToolBusinessProvider> _logger;
        public QcProcessBusinessProvider(IQcProcessDataProvider dataProvider, ILogger<ToolBusinessProvider> logger)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
            _logger = logger;
        }

        public async Task<ResponseViewModel<QcProcess>> Delete(int qcProcessId)
        {
            ResponseViewModel<QcProcess> result = new ResponseViewModel<QcProcess>();
            List<QcProcess> deletedQcProcessList = new List<QcProcess>();

            QcProcess qcProcessData = await _dataProvider.GetById(qcProcessId);
            QcProcess deletedQcProcessData = await _dataProvider.Delete(qcProcessData);
            deletedQcProcessList.Add(deletedQcProcessData);
            
            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            result.Data = deletedQcProcessList;
            return result;
        }

        public async Task<ResponseOneDataViewModel<QcProcessViewModel>> GetById(int id)
        {
            ResponseOneDataViewModel<QcProcessViewModel> result = new ResponseOneDataViewModel<QcProcessViewModel>();
            var getData = await _dataProvider.GetByIdDetail(id);

            if (getData == null)
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
            }
            else
            {
                result.StatusCode = 200;
                result.Message = ApplicationConstant.OK_MESSAGE;
                result.Data = getData;
            }

            return result;
        }

        public async Task<ResponseViewModel<QcProcess>> Insert(InsertQcProcessBindingModel data)
        {
            DateTime now = DateTime.UtcNow;
            DateTime nowTimestamp = DateHelper.Now();

            ResponseViewModel<QcProcess> result = new ResponseViewModel<QcProcess>();

            List<QcProcess> qcProcess = new List<QcProcess>();
            List<FormMaterial> formMaterialData = new List<FormMaterial>();
            List<FormTool> formToolData = new List<FormTool>();
            List<FormProcedure> formProcedureData = new List<FormProcedure>();

            foreach (var formMaterial in data.FormMaterial)
            {
                FormMaterial tempFormMaterial = new FormMaterial()
                {
                    Sequence = formMaterial.Sequence,
                    ItemId = formMaterial.ItemId,
                    DefaultPackageQty = formMaterial.DefaultPackageQty,
                    UomPackage = formMaterial.UomPackage,
                    DefaultQty = formMaterial.DefaultQty,
                    Uom = formMaterial.Uom,
                    GroupName = formMaterial.GroupName,
                    CreatedAt = nowTimestamp,
                    CreatedBy = data.CreatedBy,
                    UpdatedAt = nowTimestamp,
                    UpdatedBy = data.CreatedBy
                };
                formMaterialData.Add(tempFormMaterial);
            }
            foreach (var formTool in data.FormTool)
            {
                FormTool tempFormTool = new FormTool()
                {
                    Sequence = formTool.Sequence,
                    Type = formTool.Type,
                    ToolId = formTool.ToolId,
                    ItemId = formTool.ItemId,
                    Qty = formTool.Qty,
                    CreatedAt = nowTimestamp,
                    CreatedBy = data.CreatedBy,
                    UpdatedAt = nowTimestamp,
                    UpdatedBy = data.CreatedBy
                };
                formToolData.Add(tempFormTool);
            }
            foreach (var formProcedure in data.FormProcedure)
            {
                List<FormParameter> formParameterList = new List<FormParameter>();
                foreach (var formParameter in formProcedure.FormParameter)
                {
                    FormParameter tempFormParameter = new FormParameter()
                    {
                        Sequence = formParameter.Sequence,
                        Label = formParameter.Label,
                        Code = formParameter.Code,
                        InputType = formParameter.InputType,
                        Uom = formParameter.Uom,
                        ThresholdOperator = formParameter.ThresholdOperator,
                        ThresholdValue = formParameter.ThresholdValue,
                        ThresholdValueTo = formParameter.ThresholdValueTo,
                        ThresholdValueFrom = formParameter.ThresholdValueFrom,
                        NeedAttachment = formParameter.NeedAttachment,
                        Note = formParameter.Note,
                        IsForAllSample = formParameter.IsForAllSample,
                        IsResult = formParameter.IsResult,
                        DefaultValue = formParameter.DefaultValue,
                        CreatedAt = nowTimestamp,
                        CreatedBy = data.CreatedBy,
                        UpdatedAt = nowTimestamp,
                        UpdatedBy = data.CreatedBy
                    };
                    formParameterList.Add(tempFormParameter);
                }
                FormProcedure tempFormProcedure = new FormProcedure()
                {
                    Sequence = formProcedure.Sequence,
                    Description = formProcedure.Description,
                    FormParameter = formParameterList,
                    CreatedAt = nowTimestamp,
                    CreatedBy = data.CreatedBy,
                    UpdatedAt = nowTimestamp,
                    UpdatedBy = data.CreatedBy
                };
                formProcedureData.Add(tempFormProcedure);
            }
            QcProcess qcProcessData = new QcProcess()
            {
                Sequence = data.Sequence,
                Name = data.Name,
                ParentId = data.ParentId,
                RoomId = data.RoomId,
                IsInputForm = data.IsInputForm,
                FormMaterial = formMaterialData,
                FormTool = formToolData,
                FormProcedure = formProcedureData,
                CreatedAt = nowTimestamp,
                CreatedBy = data.CreatedBy,
                UpdatedAt = nowTimestamp,
                UpdatedBy = data.CreatedBy
            };
            QcProcess newQcProcess = await _dataProvider.Insert(qcProcessData);
            qcProcess.Add(newQcProcess);
            
            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            result.Data = qcProcess;

            return result;
        }

        public async Task<ResponseViewModel<QcProcessViewModel>> List()
        {            
            ResponseViewModel<QcProcessViewModel> result = new ResponseViewModel<QcProcessViewModel>();
            
            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            result.Data = await _dataProvider.List();
            return result;
        }

        public async Task<ResponseViewModel<QcProcessShortViewModel>> ListShort(string search, int limit, int page, int roomId, int? purposeId)
        {
            BasePagination pagination = new BasePagination(page, limit);
            ResponseViewModel<QcProcessShortViewModel> result = new ResponseViewModel<QcProcessShortViewModel>();

            List<QcProcessShortViewModel> getData = await _dataProvider.ListShort(search, limit, pagination.CalculateOffset(), roomId, purposeId);

            if (!getData.Any())
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
            }
            else
            {
                result.StatusCode = 200;
                result.Message = ApplicationConstant.OK_MESSAGE;
                result.Data = getData;
            }

            return result;
        }
    }
}
