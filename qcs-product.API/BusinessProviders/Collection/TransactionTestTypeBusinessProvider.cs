using production_execution_system.API.ViewModels;
using qcs_product.API.BindingModels;
using qcs_product.API.DataProviders.Collection;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders.Collection
{
    public class TransactionTestTypeBusinessProvider : ITransactionTestTypeBusinessProvider
    {
        private readonly QcsProductContext _context;
        private readonly ITransactionTestTypeDataProvider _dataProvider;

        public TransactionTestTypeBusinessProvider(QcsProductContext context, ITransactionTestTypeDataProvider dataProvider)
        {
            _context = context;
            _dataProvider = dataProvider;
        }

        public async Task<ResponseOneDataViewModel<TransactionTestingViewModel>> GetTestingWProcedure(int testingId)
        {
            ResponseOneDataViewModel<TransactionTestingViewModel> result = new ResponseOneDataViewModel<TransactionTestingViewModel>();
            result.Data = await _dataProvider.GetTestingById(testingId);
            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            return result;
        }

        public async Task<ResponseOneDataViewModel<TransactionTestingProcedure>> UpdateParameterValue(InsertTransactionTestingProcedureBindingModel data)
        {
            ResponseOneDataViewModel<TransactionTestingProcedure> result = new ResponseOneDataViewModel<TransactionTestingProcedure>();
            List<TransactionTestingProcedureParameter> updatedParameterDataList = new List<TransactionTestingProcedureParameter>();

            var options = new JsonSerializerOptions { WriteIndented = true };
            var procedure = await _dataProvider.GetProcedureById(data.ProcedureId);
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (data.IsSubmit)
                    {
                        procedure.Status = ApplicationConstant.TEST_TYPE_COMPLETE;
                    }
                    foreach (var procedureParameter in data.ProcedureParameterValues)
                    {
                        TransactionTestingProcedureParameter procedureParameterData = await _dataProvider.GetParameterById(procedureParameter.TestingProcedureParameterId);
                        procedureParameterData.PropertiesValue = procedureParameter.PropertiesValue;
                        await _dataProvider.UpdateParameter(procedureParameterData);
                        updatedParameterDataList.Add(new TransactionTestingProcedureParameter
                        {
                            Id = procedureParameterData.Id,
                            InputTypeId = procedureParameterData.InputTypeId,
                            Properties = procedureParameterData.Properties != null ? JsonSerializer.Deserialize<dynamic>(Regex.Unescape(procedureParameterData.Properties.ToString().Trim('"')), options) : null,
                            PropertiesValue = procedureParameterData.PropertiesValue != null ? JsonSerializer.Deserialize<dynamic>(Regex.Unescape(procedureParameterData.PropertiesValue.ToString().Trim('"')), options) : null,
                            Code = procedureParameterData.Code,
                            CreatedBy = procedureParameterData.CreatedBy,
                            RowStatus = procedureParameterData.RowStatus,
                            TransactionTestingProcedureId = procedureParameterData.TransactionTestingProcedureId,
                            TestTypeProcedureCode = procedureParameterData.TestTypeProcedureCode,
                            DeviationLevel = procedureParameterData.DeviationLevel,
                            DeviationNote = procedureParameterData.DeviationNote,
                            HasAttachment = procedureParameterData.HasAttachment,
                            IsNullable = procedureParameterData.IsNullable,
                            Name = procedureParameterData.Name,
                            Sequence = procedureParameterData.Sequence,
                            IsDeviation = procedureParameterData.IsDeviation,
                            CreatedAt = procedureParameterData.CreatedAt,
                            UpdatedAt = procedureParameterData.UpdatedAt,
                            UpdatedBy = procedureParameterData.UpdatedBy,
                        });

                        await _dataProvider.InsertHistoryParameter(new TransactionHtrTestingProcedureParameter()
                        {
                            Code = procedureParameterData.Code,
                            DeviationLevel = procedureParameterData.DeviationLevel ?? 0,
                            DeviationNote = procedureParameterData.DeviationNote,
                            HasAttachment = procedureParameterData.HasAttachment,
                            InputTypeId = procedureParameterData.InputTypeId,
                            IsNullable = procedureParameterData.IsNullable,
                            Name = procedureParameterData.Name,
                            Properties = procedureParameterData.Properties,
                            PropertiesValue = procedureParameterData.PropertiesValue,
                            RowStatus = procedureParameterData.RowStatus,
                            Sequence = procedureParameterData.Sequence,
                            IsDeviation = procedureParameterData.IsDeviation ?? false,
                            CreatedAt = procedureParameterData.CreatedAt,
                            CreatedBy = procedureParameterData.CreatedBy,
                            UpdatedAt = procedureParameterData.UpdatedAt,
                            UpdatedBy = procedureParameterData.UpdatedBy,
                            ParameterId = procedureParameterData.Id,
                            ExecutorName = data.Name,
                            ExecutorNik = data.Nik,
                            ExecutorPosition = data.Position,
                        });
                    }
                    await _dataProvider.UpdateProcedure(procedure);
                    transaction.Commit();
                    procedure.ProcedureParameter = updatedParameterDataList;
                    result.Data = procedure;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    result.StatusCode = 500;
                    result.Message = ex.Message;
                }
            }
            return result;
        }

        public async Task<ResponseOneDataViewModel<InsertParameterAttachmentViewModel>> InsertParameterAttachment(InsertParameterAttachmentBindingModel data)
        {
            ResponseOneDataViewModel<InsertParameterAttachmentViewModel> result = new ResponseOneDataViewModel<InsertParameterAttachmentViewModel>();
            if (data.Attachments != null)
            {
                List<TransactionTestingProcedureParameterAttachment> currentParameterAttachmentList = await _dataProvider.GetAttachmentByParameterId(data.ProcedureParameterId);
                List<TransactionTestingProcedureParameterAttachment> latestAttachmentData = new List<TransactionTestingProcedureParameterAttachment>();
                List<TransactionHtrProcessProcedureParameterAttachment> attachmentHistoryDataList = new List<TransactionHtrProcessProcedureParameterAttachment>();

                if (!this._IsSameAttachmentList(currentParameterAttachmentList, data.Attachments))
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            foreach (var attachment in data.Attachments)
                            {
                                TransactionTestingProcedureParameterAttachment currentParameterAttachment = await _dataProvider.GetByMediaLink(attachment.MediaLink);
                                if (currentParameterAttachment == null)
                                {
                                    TransactionTestingProcedureParameterAttachment insertedData = await _dataProvider.InsertProcedureParameterAttachment(new TransactionTestingProcedureParameterAttachment()
                                    {
                                        TransactionTestingProcedureParameterId = data.ProcedureParameterId,
                                        Filename = attachment.Filename,
                                        MediaLink = attachment.MediaLink,
                                        ExecutorName = data.Name,
                                        ExecutorPosition = data.Position,
                                        ExecutorNik = data.CreatedBy,
                                        Ext = attachment.Ext,
                                        CreatedBy = data.CreatedBy,
                                        UpdatedBy = data.CreatedBy,
                                        CreatedAt = DateTime.Now.ToLocalTime(),
                                        UpdatedAt = DateTime.Now.ToLocalTime(),
                                        Type = "PARAMETER"
                                    });
                                    latestAttachmentData.Add(insertedData);
                                    attachmentHistoryDataList.Add(new TransactionHtrProcessProcedureParameterAttachment()
                                    {
                                        TestingProcedureParameterId = insertedData.TransactionTestingProcedureParameterId,
                                        Filename = insertedData.Filename,
                                        MediaLink = insertedData.MediaLink,
                                        Ext = insertedData.Ext,
                                        Note = data.Note,
                                        Action = ApplicationConstant.HISTORY_ADD_ACTION,
                                        ExecutorName = data.Name,
                                        ExecutorPosition = data.Position,
                                        ExecutorNik = data.CreatedBy,
                                        CreatedBy = data.CreatedBy,
                                        UpdatedBy = data.CreatedBy,
                                        CreatedAt = DateTime.Now.ToUniversalTime(),
                                        UpdatedAt = DateTime.Now.ToUniversalTime()
                                    });
                                }
                                else
                                {
                                    latestAttachmentData.Add(currentParameterAttachment);
                                }
                            }
                            List<TransactionTestingProcedureParameterAttachment> deletedDataList = await _dataProvider.DeleteNotInRange(data.ProcedureParameterId, latestAttachmentData);
                            if (deletedDataList != null)
                            {
                                foreach (var deletedData in deletedDataList)
                                {
                                    attachmentHistoryDataList.Add(new TransactionHtrProcessProcedureParameterAttachment()
                                    {
                                        TestingProcedureParameterId = deletedData.TransactionTestingProcedureParameterId,
                                        Filename = deletedData.Filename,
                                        MediaLink = deletedData.MediaLink,
                                        Ext = deletedData.Ext,
                                        Note = data.Note,
                                        Action = ApplicationConstant.HISTORY_DELETE_ACTION,
                                        ExecutorName = data.Name,
                                        ExecutorPosition = data.Position,
                                        ExecutorNik = data.CreatedBy,
                                        CreatedBy = data.CreatedBy,
                                        UpdatedBy = data.CreatedBy,
                                        CreatedAt = DateTime.Now.ToLocalTime(),
                                        UpdatedAt = DateTime.Now.ToLocalTime(),
                                    });
                                }
                            }
                            //insert history data
                            attachmentHistoryDataList.Reverse();
                            foreach (var attachmentHistoryData in attachmentHistoryDataList)
                            {
                                TransactionHtrProcessProcedureParameterAttachment insertedAttachmentHtrData = await _dataProvider.InsertHistoryParameterAttachment(attachmentHistoryData);
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            result.StatusCode = 500;
                            result.Message = ApplicationConstant.MESSAGE_EDIT_CONC_NOT_SUCCESS;
                        }
                        if (result.StatusCode == 200)
                        {
                            result.Data = new InsertParameterAttachmentViewModel()
                            {
                                Attachments = await _dataProvider.GetAttachmentByParameterId(data.ProcedureParameterId),
                                AttachmentHistories = await _dataProvider.GetAttachmentHistoryByParameterId(data.ProcedureParameterId)
                            };
                            result.Message = ApplicationConstant.OK_MESSAGE;
                        }
                    }
                }
                else
                {
                    result.StatusCode = 409;
                    result.Message = ApplicationConstant.DATA_CANNOT_SAME;
                    result.Data = new InsertParameterAttachmentViewModel()
                    {
                        Attachments = await _dataProvider.GetAttachmentByParameterId(data.ProcedureParameterId),
                        AttachmentHistories = await _dataProvider.GetAttachmentHistoryByParameterId(data.ProcedureParameterId)
                    };
                }
            }
            return result;
        }

        public async Task<ResponseOneDataViewModel<ListParameterNoteViewModel>> InsertParameterNote(InsertParameterNoteBindingModel data)
        {
            ResponseOneDataViewModel<ListParameterNoteViewModel> result = new ResponseOneDataViewModel<ListParameterNoteViewModel>();
            result.Data = await _dataProvider.InsertProcedureParameterNote(new TransactionTestingProcedureParameterNote()
            {
                TransactionTestingProcedureParameterId = data.ProcedureParameterId,
                Note = data.Note,
                Name = data.Name,
                Position = data.Position,
                CreatedBy = data.CreatedBy,
                UpdatedBy = data.CreatedBy,
                Type = "PARAMETER"
            });
            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            return result;
        }

        public async Task<ResponseViewModel<TestingProcedureParameterViewModel>> InsertMultipleDeviation(InsertExceptionBindingModel data)
        {
            ResponseViewModel<TestingProcedureParameterViewModel> result = new ResponseViewModel<TestingProcedureParameterViewModel>();
            List<TestingProcedureParameterViewModel> listData = new List<TestingProcedureParameterViewModel>();
            foreach (var parameter in data.Parameters)
            {
                var procedureParameterException = await _dataProvider.PatchExceptionParameter(parameter.ProcedureParameterId, parameter.ExceptionNote);
                foreach (var attachment in parameter.Attachments)
                {
                    var exception = await _dataProvider.InsertAttachmentException(parameter.CreatedBy, attachment.MediaLink, attachment.Filename, parameter.ProcedureParameterId, attachment.Ext);
                    procedureParameterException.exceptionParameter.Add(exception); // error here
                }
                listData.Add(procedureParameterException);
            }
            result.StatusCode = (int)HttpStatusCode.OK;
            result.Data = listData;
            return result;
        }

        private bool _IsSameAttachmentList(List<TransactionTestingProcedureParameterAttachment> currentAttachment, List<ListInsertParameterAttachmentViewModel> newAttachment)
        {
            bool result = false;
            if (currentAttachment.Count == newAttachment.Count)
            {
                int sameDataCounter = 0;
                foreach (TransactionTestingProcedureParameterAttachment current in currentAttachment)
                {
                    if (
                        newAttachment.Any(x => x.Filename == current.Filename)
                        && newAttachment.Any(x => x.Ext == current.Ext)
                        && newAttachment.Any(x => x.MediaLink == current.MediaLink)
                    )
                    {
                        sameDataCounter++;
                    }
                }
                if (sameDataCounter == currentAttachment.Count)
                {
                    result = true;
                }
            }
            return result;
        }

    }
}
