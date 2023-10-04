using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class QcProcessViewModel
    {
        public int Id { get; set; }
        public int Sequence { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public int? RoomId { get; set; }
        public int? IsInputForm { get; set; }
        public int? AddSampleLayoutType { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public List<FormMaterialViewModel> FormMaterial { get; set; }
        public List<FormToolViewModel> FormTool { get; set; }
        public List<FormProcedureViewModel> FormProcedure { get; set; }
        public List<FormGeneralViewModel> FormGeneral { get; set; }
        public List<QcProcessViewModel> QcProcess { get; set; }
    }

    public partial class FormMaterialViewModel
    {
        public int Id { get; set; }
        public int Sequence { get; set; }
        public int ItemId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string BatchNumber { get; set; }
        public decimal DefaultPackageQty { get; set; }
        public int UomPackage { get; set; }
        public decimal DefaultQty { get; set; }
        public int Uom { get; set; }
        public string GroupName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }

    public partial class FormProcedureViewModel
    {
        public int Id { get; set; }
        public int Sequence { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public List<FormParameterViewModel> FormParameter { get; set; }
    }

    public partial class FormGeneralViewModel
    {
        public int Id { get; set; }
        public int Sequence { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public List<FormParameterViewModel> FormParameter { get; set; }
    }

    public partial class FormToolViewModel
    {
        public int Id { get; set; }
        public int Sequence { get; set; }
        public int Type { get; set; }
        public int ToolId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? ItemId { get; set; }
        public decimal Qty { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }

    public partial class FormParameterViewModel
    {
        public int Id { get; set; }
        public int Sequence { get; set; }
        public string Label { get; set; }
        public string Code { get; set; }
        public int InputType { get; set; }
        public int? Uom { get; set; }
        public int? ThresholdOperator { get; set; }
        public decimal? ThresholdValue { get; set; }
        public decimal? ThresholdValueTo { get; set; }
        public decimal? ThresholdValueFrom { get; set; }
        public bool NeedAttachment { get; set; }
        public string Note { get; set; }
        public bool IsForAllSample { get; set; }
        public bool IsResult { get; set; }
        public string DefaultValue { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
