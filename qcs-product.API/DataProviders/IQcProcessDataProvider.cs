using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders
{
    public interface IQcProcessDataProvider
    {
        public Task<QcProcess> GetById(int qcProcessId);
        public Task<QcProcess> Delete(QcProcess data);
        public Task<List<QcProcessShortViewModel>> ListShort(string search, int limit, int page, int roomId, int? purposeId);
        public Task<List<QcProcessViewModel>> List();
        public Task<QcProcessViewModel> GetByIdDetail(int qcProcessId);
        public Task<QcProcess> Insert(QcProcess data);

        //get qc per tabel relation
        public Task<QcProcess> GetQcProcessById(int id);
        public Task<List<QcProcess>> GetQcProcessByParentId(int ParentId);
        public Task<List<FormMaterial>> GetFormMaterial(int qcProcessId);
        public Task<List<FormTool>> GetFormTool(int qcProcessId);
        public Task<List<FormProcedure>> GetFormProcedure(int qcProcessId);
        public Task<List<FormProcedure>> GetFormProcedureSection(int qcProcessId, int SectionTypeId);
        public Task<List<FormParameter>> GetFormParameter(int procedureId);
        public Task<List<FormSection>> GetFormSection();
    }
}
