using qcs_product.API.BindingModels;
using qcs_product.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders
{
    public interface ITemplateTestingInfoDataProvider
    {
        public Task<List<TemplateTestingPersonnel>> InsertPersonnel(List<TemplateTestingPersonnel> listPersonnel);
        public Task<TemplateTestingNote> InsertNote(TemplateTestingNote listNote);
        public Task<List<TemplateTestingAttachment>> InsertAttachment(List<TemplateTestingAttachment> listAttachment);
        public List<TemplateTestingAttachment> DeleteAttachment(List<int> listId);
        public Task<List<TemplateTestingAttachment>> GetAttachmentByTemplateTestingId(int templateTestingId);
        public Task<List<TemplateTestingNote>> GetNoteByTemplateTestingId(int templateTestingId);
        public Task<List<TemplateTestingPersonnel>> GetPersonnelByTemplateTestingId(int templateTestingId);
        public Task<TemplateTestingPersonnel> CheckInCheckOut(InsertCheckInCheckOutPersonnel data);
        public Task<TemplateTestingAttachment> GetAttachmentById(int templateTestingAttachmentId);
    }
}
