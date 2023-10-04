using qcs_product.API.DataProviders;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.Constants;
using System.Diagnostics.CodeAnalysis;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using qcs_product.API.BindingModels;
using qcs_product.API.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace qcs_product.API.BusinessProviders.Collection
{
    public class OperatorTestingBusinessProvider : IOperatorTestingBusinessProvider
    {
        private readonly IOperatorTestingDataProvider _dataProvider;
        private readonly QcsProductContext _context;
        private readonly ILogger<OperatorTestingBusinessProvider> _logger;

        [ExcludeFromCodeCoverage]
        public OperatorTestingBusinessProvider(
            IOperatorTestingDataProvider dataProvider,
            QcsProductContext context,
            ILogger<OperatorTestingBusinessProvider> logger)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public async Task<ResponseOneDataViewModel<GeneralOperatorTestingInfoViewModel>> InfoGeneralByTestingId(int testingId)
        {
            ResponseOneDataViewModel<GeneralOperatorTestingInfoViewModel> result = new ResponseOneDataViewModel<GeneralOperatorTestingInfoViewModel>();
            GeneralOperatorTestingInfoViewModel testingInfoGeneral = new GeneralOperatorTestingInfoViewModel();
            TransactionTesting trxTesting = await _dataProvider.GetTrxTestingById(testingId);

            if (trxTesting == null)
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
            }
            else
            {
                testingInfoGeneral = await _dataProvider.InfoGeneralByTestingId(testingId);
                testingInfoGeneral.IsPersonnelSaved = false;
                testingInfoGeneral.IsAttachmentSaved = false;
                if (testingInfoGeneral.listHtrPersonnel != null && testingInfoGeneral.listHtrPersonnel.Any())
                {
                    testingInfoGeneral.IsPersonnelSaved = true;
                }
                if (testingInfoGeneral.listHtrAttachment != null && testingInfoGeneral.listHtrAttachment.Any())
                {
                    testingInfoGeneral.IsAttachmentSaved = true;
                }
                result.StatusCode = 200;
                result.Message = ApplicationConstant.OK_MESSAGE;
                result.Data = testingInfoGeneral;
            }
            return result;
        }


        public async Task<TransactionTesting> SetEndDate(int testingId)
        {
            TransactionTesting result = new TransactionTesting();
            result = await _dataProvider.SetEndDate(testingId);

            return result;
        }

        public async Task<TransactionTesting> SetStartDate(int testingId)
        {
            TransactionTesting result = new TransactionTesting();
            result = await _dataProvider.SetStartDate(testingId);

            return result;
        }

        public async Task<ResponseOneDataViewModel<InsertTestingPQViewModel>> InsertUserPQ(InsertTestingPQBindingModel data)
        {
            var nowTimestamp = _context.NowTimestamp.FromSqlRaw(ApplicationConstant.GET_DB_CURRENT_TIMESTAMP_QUERY).FirstOrDefault();

            ResponseOneDataViewModel<InsertTestingPQViewModel> result = new ResponseOneDataViewModel<InsertTestingPQViewModel>()
            {
                StatusCode = 200,
                Data = new InsertTestingPQViewModel()
            };
            List<TransactionTestingPersonnel> resultData = new List<TransactionTestingPersonnel>();
            List<TransactionHtrTestingPersonnel> resultDataHistory = new List<TransactionHtrTestingPersonnel>();
            List<TransactionTestingPersonnel> latestPersonnelData = new List<TransactionTestingPersonnel>();
            List<TransactionHtrTestingPersonnel> personnelHistoryDataList = new List<TransactionHtrTestingPersonnel>();
            if (data.Personnels != null)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var personnel in data.Personnels)
                        {
                            TransactionTestingPersonnel currentProcessPersonel = await _dataProvider.GetPersonelByNewNIK(data.TestingId, personnel.NewNIK);
                            if (currentProcessPersonel == null)
                            {
                                TransactionTestingPersonnel productionProcessData = new TransactionTestingPersonnel()
                                {
                                    TestingId = data.TestingId,
                                    TestingCode = data.TestingCode,
                                    Nik = personnel.Nik,
                                    NewNIK = personnel.NewNIK,
                                    Nama = personnel.Nama,
                                    PosisiId = personnel.PosisiId,
                                    PosisiCode = personnel.PosisiCode,
                                    Posisi = personnel.Posisi,
                                    CreatedBy = data.CreatedBy,
                                    UpdatedBy = data.CreatedBy
                                };
                                TransactionTestingPersonnel insertedData = await _dataProvider.InsertPersonel(productionProcessData);
                                latestPersonnelData.Add(insertedData);
                                personnelHistoryDataList.Add(new TransactionHtrTestingPersonnel()
                                {
                                    TestingId = insertedData.TestingId,
                                    TestingCode = insertedData.TestingCode,
                                    Nik = insertedData.Nik,
                                    NewNik = insertedData.NewNIK,
                                    Name = insertedData.Nama,
                                    PositionId = insertedData.PosisiId,
                                    PositionCode = insertedData.PosisiCode,
                                    Position = insertedData.Posisi,
                                    Note = data.Note,
                                    Action = ApplicationConstant.HISTORY_ADD_ACTION,
                                    CreatedBy = data.CreatedBy,
                                    UpdatedBy = data.CreatedBy,
                                    CreatedAt = nowTimestamp.CurrentTimestamp,
                                    UpdatedAt = nowTimestamp.CurrentTimestamp
                                });
                            }
                            else
                            {
                                currentProcessPersonel.TestingId = data.TestingId;
                                currentProcessPersonel.TestingCode = data.TestingCode;
                                currentProcessPersonel.Nik = personnel.Nik;
                                currentProcessPersonel.NewNIK = personnel.NewNIK;
                                currentProcessPersonel.Nama = personnel.Nama;
                                currentProcessPersonel.PosisiId = personnel.PosisiId;
                                currentProcessPersonel.PosisiCode = personnel.PosisiCode;
                                currentProcessPersonel.Posisi = personnel.Posisi;
                                currentProcessPersonel.UpdatedBy = data.CreatedBy;
                                TransactionTestingPersonnel updatedData = await _dataProvider.UpdatePersonel(currentProcessPersonel);
                                latestPersonnelData.Add(updatedData);
                            }
                            resultData.Add(personnel);
                        }
                        List<TransactionTestingPersonnel> deletedDataList = await _dataProvider.DeleteNotInRangePersonel(data.TestingId, latestPersonnelData);
                        if (deletedDataList != null)
                        {
                            foreach (var deletedData in deletedDataList)
                            {
                                personnelHistoryDataList.Add(new TransactionHtrTestingPersonnel()
                                {
                                    TestingId = deletedData.TestingId,
                                    TestingCode = deletedData.TestingCode,
                                    Nik = deletedData.Nik,
                                    NewNik = deletedData.NewNIK,
                                    Name = deletedData.Nama,
                                    PositionId = deletedData.PosisiId,
                                    PositionCode = deletedData.PosisiCode,
                                    Position = deletedData.Posisi,
                                    Note = data.Note,
                                    Action = ApplicationConstant.HISTORY_DELETE_ACTION,
                                    CreatedBy = data.CreatedBy,
                                    UpdatedBy = data.CreatedBy,
                                    CreatedAt = nowTimestamp.CurrentTimestamp,
                                    UpdatedAt = nowTimestamp.CurrentTimestamp
                                });
                            }
                        }
                        //insert history data
                        personnelHistoryDataList.Reverse();
                        foreach (var personnelHistoryData in personnelHistoryDataList)
                        {
                            TransactionHtrTestingPersonnel insertedPersonnelHtrData = await _dataProvider.InsertHtrPersonel(personnelHistoryData);
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _logger.LogError(ex, "{Message}", ex.Message);
                        result.StatusCode = 500;
                        result.Message = ex.Message;
                    }
                    if (result.StatusCode == 200)
                    {
                        resultDataHistory = await _dataProvider.GetHtrPersonelByTestingId(data.TestingId);
                        var temp = new InsertTestingPQViewModel { Personnel = resultData, PersonnelHistory = resultDataHistory };
                        result.Data = temp;
                        result.Message = ApplicationConstant.OK_MESSAGE;
                    }
                }
            }
            else
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
            }
            return result;
        }

        public async Task<ResponseViewModel<TransactionTestingNote>> InsertTestingNote(InsertTestingNoteBindingModel data)
        {
            ResponseViewModel<TransactionTestingNote> result = new ResponseViewModel<TransactionTestingNote>()
            {
                StatusCode = 200
            };
            List<TransactionTestingNote> resultData = new List<TransactionTestingNote>();

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    TransactionTestingNote insertedTestingData = await _dataProvider.InsertNote(new TransactionTestingNote()
                    {
                        TestingId = data.TestingId,
                        TestingCode = data.TestingCode,
                        Note = data.Note,
                        Name = data.Name,
                        Position = data.Position,
                        CreatedBy = data.CreatedBy,
                        UpdatedBy = data.CreatedBy
                    });

                    TransactionHtrTestingNote insertedTestingHtrData = await _dataProvider.InsertHtrNote(new TransactionHtrTestingNote()
                    {
                        TestingId = data.TestingId,
                        TestingCode = data.TestingCode,
                        Note = data.Note,
                        Name = data.Name,
                        Position = data.Position,
                        CreatedBy = data.CreatedBy,
                        UpdatedBy = data.CreatedBy
                    });

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogError(ex, "{Message}", ex.Message);
                    result.StatusCode = 500;
                    result.Message = ex.Message;
                }
                if (result.StatusCode == 200)
                {
                    resultData = await _dataProvider.GetNoteByTestingId(data.TestingId);
                    result.Data = resultData;
                    result.Message = ApplicationConstant.OK_MESSAGE;
                }
            }
            return result;
        }

        public async Task<ResponseOneDataViewModel<InsertTestingAttachmentViewModel>> InsertTestingAttachment(InsertTestingAttachmentBindingModel data)
        {
            var nowTimestamp = _context.NowTimestamp.FromSqlRaw(ApplicationConstant.GET_DB_CURRENT_TIMESTAMP_QUERY).FirstOrDefault();

            ResponseOneDataViewModel<InsertTestingAttachmentViewModel> result = new ResponseOneDataViewModel<InsertTestingAttachmentViewModel>()
            {
                StatusCode = 200,
                Data = new InsertTestingAttachmentViewModel()
            };
            List<TransactionTestingAttachment> latestAttachmentData = new List<TransactionTestingAttachment>();
            List<TransactionHtrTestingAttachment> attachmentHistoryDataList = new List<TransactionHtrTestingAttachment>();
            if (data.Attachments != null)
            {
                List<TransactionTestingAttachment> currentProcessAttachmentList = await _dataProvider.GetAttachmentByTestingId(data.TestingId);
                List<TransactionHtrTestingAttachment> resultDataHistory;
                List<TransactionTestingAttachment> resultData;
                if (!this._IsSameAttachmentList(currentProcessAttachmentList, data.Attachments))
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            foreach (var attachment in data.Attachments)
                            {
                                TransactionTestingAttachment currentProcessAttachment = await _dataProvider.GetAttachmentByMediaLink(attachment.MediaLink);
                                if (currentProcessAttachment == null)
                                {
                                    TransactionTestingAttachment insertedData = await _dataProvider.InsertAttachment(new TransactionTestingAttachment()
                                    {
                                        TestingId = data.TestingId,
                                        Filename = attachment.Filename,
                                        MediaLink = attachment.MediaLink,
                                        ExecutorName = data.Name,
                                        ExecutorPosition = data.Position,
                                        ExecutorNik = data.CreatedBy,
                                        Ext = attachment.Ext,
                                        CreatedBy = data.CreatedBy,
                                        UpdatedBy = data.CreatedBy,
                                        TestingCode = data.TestingCode
                                    });
                                    attachmentHistoryDataList.Add(new TransactionHtrTestingAttachment()
                                    {
                                        TestingId = insertedData.TestingId,
                                        TestingCode = insertedData.TestingCode,
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
                                        CreatedAt = nowTimestamp.CurrentTimestamp,
                                        UpdatedAt = nowTimestamp.CurrentTimestamp
                                    });
                                    latestAttachmentData.Add(insertedData);
                                }
                                else
                                {
                                    latestAttachmentData.Add(currentProcessAttachment);
                                }
                            }
                            List<TransactionTestingAttachment> deletedDataList = await _dataProvider.DeleteNotInRangeAttachment(data.TestingId, latestAttachmentData);
                            if (deletedDataList != null)
                            {
                                foreach (var deletedData in deletedDataList)
                                {
                                    attachmentHistoryDataList.Add(new TransactionHtrTestingAttachment()
                                    {
                                        TestingId = deletedData.TestingId,
                                        TestingCode = deletedData.TestingCode,
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
                                        CreatedAt = nowTimestamp.CurrentTimestamp,
                                        UpdatedAt = nowTimestamp.CurrentTimestamp
                                    });
                                }
                            }
                            //insert history data
                            attachmentHistoryDataList.Reverse();
                            foreach (var attachmentHistoryData in attachmentHistoryDataList)
                            {
                                TransactionHtrTestingAttachment insertedAttachmentHtrData = await _dataProvider.InsertHtrAttachment(attachmentHistoryData);
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            _logger.LogError(ex, "{Message}", ex.Message);
                            result.StatusCode = 500;
                            result.Message = ApplicationConstant.GENERAL_ERROR;
                        }
                        if (result.StatusCode == 200)
                        {
                            resultDataHistory = await _dataProvider.GetHtrAttachmentByTestingId(data.TestingId);
                            resultData = await _dataProvider.GetAttachmentByTestingId(data.TestingId);
                            var temp = new InsertTestingAttachmentViewModel { Attachment = resultData, AttachmentHistory = resultDataHistory };
                            result.Data = temp;
                            result.Message = ApplicationConstant.OK_MESSAGE;
                        }
                    }
                }
                else
                {
                    resultDataHistory = await _dataProvider.GetHtrAttachmentByTestingId(data.TestingId);
                    resultData = await _dataProvider.GetAttachmentByTestingId(data.TestingId);
                    result.StatusCode = 409;
                    result.Message = ApplicationConstant.DATA_CANNOT_SAME;
                    var temp = new InsertTestingAttachmentViewModel { Attachment = resultData, AttachmentHistory = resultDataHistory };
                    result.Data = temp;
                }
            }
            else
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
            }
            return result;
        }

        private bool _IsSameAttachmentList(List<TransactionTestingAttachment> currentAttachment, List<ListAttachmentTesting> newAttachment)
        {
            bool result = false;
            if (currentAttachment.Count == newAttachment.Count)
            {
                int sameDataCounter = 0;
                foreach (TransactionTestingAttachment current in currentAttachment)
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

        public async Task<ResponseOneDataViewModel<UserPresenceViewModel>> CheckInByPin(CheckInOutBindingModel data)
        {
            ResponseOneDataViewModel<UserPresenceViewModel> result = new ResponseOneDataViewModel<UserPresenceViewModel>();
            bool isValid = await _dataProvider.Authenticate(data.Pin, data.NewNIK);
            if (!isValid)
            {
                result.StatusCode = (int)HttpStatusCode.Unauthorized;
                result.Message = ApplicationConstant.NOT_OK_MESSAGE;
                return result;
            }

            TransactionTestingPersonnel existingData = new TransactionTestingPersonnel();
            existingData = await _dataProvider.GetCheckedInUserPresenceByUsername(data.Nik);
            if (existingData.CheckIn != null)
            {
                result.StatusCode = (int)HttpStatusCode.AlreadyReported;
                result.Message = ApplicationConstant.ALREADY_CHECKIN_MESSAGE;
                return result;
            }
            TransactionTestingPersonnel user = await _dataProvider.CheckedInUserPresence(data);

            TransactionHtrTestingPersonnel userHtr = new TransactionHtrTestingPersonnel()
            {
                PositionId = data.PosisiId,
                Position = data.Posisi,
                Name = data.Nama,
                CreatedBy = data.Nama,
                CreatedAt = DateTime.UtcNow.AddHours(ApplicationConstant.TIMEZONE),
                UpdatedBy = data.Nama,
                UpdatedAt = DateTime.UtcNow.AddHours(ApplicationConstant.TIMEZONE),
                Action = ApplicationConstant.HISTORY_ADD_ACTION,
                Nik = data.Nik,
                NewNik = data.NewNIK,
                Note = data.Note,
                PositionCode = data.PosisiCode,
                TestingCode = data.TestingCode,
                TestingId = data.TestingId
            };

            TransactionHtrTestingPersonnel insertedUserPresence = await _dataProvider.InsertHtrPresence(userHtr);

            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            result.Data = new UserPresenceViewModel()
            {
                PosId = insertedUserPresence.PositionId,
                Name = insertedUserPresence.Name,
                Username = insertedUserPresence.Nik,
                CheckIn = user.CheckIn,
                CheckOut = user.CheckOut,
                TestingCode = insertedUserPresence.TestingCode,
                TestingId = insertedUserPresence.TestingId

            };
            return result;
        }

        public async Task<ResponseOneDataViewModel<UserPresenceViewModel>> CheckOutByPin(CheckInOutBindingModel data)
        {
            ResponseOneDataViewModel<UserPresenceViewModel> result = new ResponseOneDataViewModel<UserPresenceViewModel>();
            bool isValid = await _dataProvider.Authenticate(data.Pin, data.NewNIK);
            if (!isValid)
            {
                result.StatusCode = (int)HttpStatusCode.Unauthorized;
                result.Message = ApplicationConstant.NOT_OK_MESSAGE;
                return result;
            }

            TransactionTestingPersonnel existingData = new TransactionTestingPersonnel();
            existingData = await _dataProvider.GetCheckedInUserPresenceByUsername(data.Nik);
            if (existingData == null)
            {
                result.StatusCode = 400;
                result.Message = ApplicationConstant.PRESENCE_DATA_NOT_FOUND;
                return result;
            }

            existingData.UpdatedAt = DateTime.UtcNow.AddHours(ApplicationConstant.TIMEZONE);
            existingData.CheckOut = DateTime.UtcNow.AddHours(ApplicationConstant.TIMEZONE);

            TransactionTestingPersonnel user = await _dataProvider.CheckedOutUserPresence(existingData);

            TransactionHtrTestingPersonnel userHtr = new TransactionHtrTestingPersonnel()
            {
                PositionId = data.PosisiId,
                Position = data.Posisi,
                Name = data.Nama,
                CreatedBy = data.Nama,
                CreatedAt = DateTime.UtcNow.AddHours(ApplicationConstant.TIMEZONE),
                UpdatedBy = data.Nama,
                UpdatedAt = DateTime.UtcNow.AddHours(ApplicationConstant.TIMEZONE),
                Action = ApplicationConstant.HISTORY_ADD_ACTION,
                Nik = data.Nik,
                NewNik = data.NewNIK,
                Note = data.Note,
                PositionCode = data.PosisiCode,
                TestingCode = data.TestingCode,
                TestingId = data.TestingId
            };

            TransactionHtrTestingPersonnel insertedUserPresence = await _dataProvider.InsertHtrPresence(userHtr);
            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            result.Data = new UserPresenceViewModel()
            {
                PosId = insertedUserPresence.PositionId,
                Name = insertedUserPresence.Name,
                Username = insertedUserPresence.Nik,
                CheckIn = user.CheckIn,
                CheckOut = user.CheckOut,
                TestingCode = insertedUserPresence.TestingCode,
                TestingId = insertedUserPresence.TestingId
            };
            return result;
        }
    }
}
