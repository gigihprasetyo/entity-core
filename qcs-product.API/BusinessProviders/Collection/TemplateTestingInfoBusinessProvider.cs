using qcs_product.API.BindingModels;
using qcs_product.API.DataProviders;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.Constants;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders.Collection
{
    public class TemplateTestingInfoBusinessProvider : ITemplateTestingInfoBusinessProvider
    {
        private readonly ITemplateTestingInfoDataProvider _dataProvider;

        [ExcludeFromCodeCoverage]
        public TemplateTestingInfoBusinessProvider(ITemplateTestingInfoDataProvider dataProvider)
        {
            _dataProvider = dataProvider;

        }


        public async Task<ViewModels.ResponseOneDataViewModel<TemplateTestingPersonnel>> CheckInCheckOut(InsertCheckInCheckOutPersonnel data)
        {
            ResponseOneDataViewModel<TemplateTestingPersonnel> result = new ResponseOneDataViewModel<TemplateTestingPersonnel>();
            TemplateTestingPersonnel updatedData = new TemplateTestingPersonnel();

            updatedData = await _dataProvider.CheckInCheckOut(data);

            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            result.Data = updatedData;

            return result;
        }

        public ViewModels.ResponseViewModel<TemplateTestingAttachment> DeleteAttachment(List<int> listId)
        {
            ResponseViewModel<TemplateTestingAttachment> result = new ResponseViewModel<TemplateTestingAttachment>();
            List<TemplateTestingAttachment> deletedData = new List<TemplateTestingAttachment>();

            deletedData = _dataProvider.DeleteAttachment(listId);

            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            result.Data = deletedData;

            return result;
        }

        public async Task<ResponseOneDataViewModel<TemplateTestingInfoViewModel>> DetailAttachment(int templateTestingAttachmentId)
        {
            TemplateTestingAttachment data = new TemplateTestingAttachment();
            data = await _dataProvider.GetAttachmentById(templateTestingAttachmentId);

            ResponseOneDataViewModel<TemplateTestingInfoViewModel> result = new ResponseOneDataViewModel<TemplateTestingInfoViewModel>()
            {
                StatusCode = 200,
                Message = ApplicationConstant.OK_MESSAGE,
                Data = data
            };

            return result;
        }

        public async Task<ViewModels.ResponseOneDataViewModel<TemplateTestingInfoViewModel>> InfoByTestingTemplate(int templateTestingId)
        {
            TemplateTestingInfoViewModel dataInfo = new TemplateTestingInfoViewModel();
            dataInfo.listPersonnel = await _dataProvider.GetPersonnelByTemplateTestingId(templateTestingId);
            dataInfo.listNote = await _dataProvider.GetNoteByTemplateTestingId(templateTestingId);
            dataInfo.listAttachment = await _dataProvider.GetAttachmentByTemplateTestingId(templateTestingId);

            ResponseOneDataViewModel<TemplateTestingInfoViewModel> result = new ResponseOneDataViewModel<TemplateTestingInfoViewModel>()
            {
                StatusCode = 200,
                Message = ApplicationConstant.OK_MESSAGE,
                Data = dataInfo
            };

            return result;
        }

        public async Task<ViewModels.ResponseViewModel<TemplateTestingAttachment>> InsertAttachment(List<TemplateTestingAttachment> listAttachment)
        {
            ResponseViewModel<TemplateTestingAttachment> result = new ResponseViewModel<TemplateTestingAttachment>();
            List<TemplateTestingAttachment> insertedData = new List<TemplateTestingAttachment>();

            insertedData = await _dataProvider.InsertAttachment(listAttachment);

            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            result.Data = insertedData;

            return result;
        }

        public async Task<ViewModels.ResponseOneDataViewModel<TemplateTestingNote>> InsertNote(TemplateTestingNote listNote)
        {
            ResponseOneDataViewModel<TemplateTestingNote> result = new ResponseOneDataViewModel<TemplateTestingNote>();
            TemplateTestingNote insertedData = new TemplateTestingNote();

            insertedData = await _dataProvider.InsertNote(listNote);

            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            result.Data = insertedData;

            return result;
        }

        public async Task<ViewModels.ResponseViewModel<TemplateTestingPersonnel>> InsertPersonnel(List<TemplateTestingPersonnel> listPersonnel)
        {
            ResponseViewModel<TemplateTestingPersonnel> result = new ResponseViewModel<TemplateTestingPersonnel>();
            List<TemplateTestingPersonnel> insertedData = new List<TemplateTestingPersonnel>();

            insertedData = await _dataProvider.InsertPersonnel(listPersonnel);

            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            result.Data = insertedData;

            return result;
        }
    }
}
