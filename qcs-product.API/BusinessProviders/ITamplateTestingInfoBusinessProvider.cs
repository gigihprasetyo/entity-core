using qcs_product.API.BindingModels;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders
{
    public interface ITemplateTestingInfoBusinessProvider
    {
        public Task<ResponseOneDataViewModel<TemplateTestingInfoViewModel>> InfoByTestingTemplate(int templateTestingId);
        public Task<ResponseViewModel<TemplateTestingPersonnel>> InsertPersonnel(List<TemplateTestingPersonnel> listPersonnel);
        public Task<ResponseOneDataViewModel<TemplateTestingNote>> InsertNote(TemplateTestingNote listNote);
        public Task<ResponseViewModel<TemplateTestingAttachment>> InsertAttachment(List<TemplateTestingAttachment> listAttachment);
        public ResponseViewModel<TemplateTestingAttachment> DeleteAttachment(List<int> listId);
        public Task<ResponseOneDataViewModel<TemplateTestingPersonnel>> CheckInCheckOut(InsertCheckInCheckOutPersonnel data);
        public Task<ResponseOneDataViewModel<TemplateTestingInfoViewModel>> DetailAttachment(int templateTestingAttachmentId);
    }
}
