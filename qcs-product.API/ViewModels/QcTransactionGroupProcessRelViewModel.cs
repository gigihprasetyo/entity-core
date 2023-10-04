using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class QcTransactionGroupProcessRelViewModel
    {
        public Int32 Id { get; set; }
        public Int32 QcTransactionGroupId { get; set; }
        public int Sequence { get; set; }
        public string Name { get; set; }
        public Int32? ParentId { get; set; }
        public Int32? RoomId { get; set; }
        public int? IsInputForm { get; set; }
        public Int32 QcProcessId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public bool IsLastProcess { get; set; }
        public List<WorkflowHistoryQcSampling> WorkflowHistory { get; set; }
        public List<QcTransactionGroupFormMaterialViewModel> QcTransactionGroupFormMaterial { get; set; }
        public List<QcTransactionGroupFormToolViewModel> QcTransactionGroupFormTool { get; set; }
        public List<QcTransactionGroupFormProcedureViewModel> QcTransactionGroupFormProcedure { get; set; }
        public List<QcTransactionGroupFormGeneralViewModel> QcTransactionGroupFormGeneral { get; set; }
        public List<QcTransactionGroupProcessRelViewModel> QcTransactionGroupProcess { get; set; }
    }

    public partial class QcTransactionGroupFormMaterialViewModel
    {
        public Int32 Id { get; set; }
        public Int32 QcTransactionGroupProcessId { get; set; }
        public int Sequence { get; set; }
        public Int32 ItemId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal DefaultPackageQty { get; set; }
        public int UomPackage { get; set; }
        public string UomUomPackageLabel { get; set; }
        public decimal DefaultQty { get; set; }
        public int Uom { get; set; }
        public string UomLabel { get; set; }
        public Int32 QcProcessId { get; set; }
        public string GroupName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
    public partial class QcTransactionGroupFormToolViewModel
    {
        public Int32 Id { get; set; }
        public Int32 QcTransactionGroupProcessId { get; set; }
        public int Sequence { get; set; }
        public Int32? ToolId { get; set; }
        public Int32? ItemId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public Int32 QcProcessId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }

    public class QcTransactionGroupFormProcedureViewModel
    {
        public Int32 Id { get; set; }
        public Int32 QcTransactionGroupProcessId { get; set; }
        public int Sequence { get; set; }
        public string Description { get; set; }
        public Int32 FormProcedureId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public List<QcTransactionGroupFormParameterViewModel> QcTransactionGroupFormParameter { get; set; }
    }
    public class QcTransactionGroupFormGeneralViewModel
    {
        public Int32 Id { get; set; }
        public Int32 QcTransactionGroupProcessId { get; set; }
        public int Sequence { get; set; }
        public string Description { get; set; }
        public Int32 FormProcedureId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public List<QcTransactionGroupFormParameterViewModel> QcTransactionGroupFormParameter { get; set; }
    }

    public class QcTransactionGroupFormParameterViewModel
    {
        public Int32 Id { get; set; }
        public Int32 QcTransactionGroupFormProcedureId { get; set; }
        public int Sequence { get; set; }
        public string Label { get; set; }
        public string Code { get; set; }
        public int InputType { get; set; }
        public string InputTypeLabel { get; set; }
        public string Reference { get; set; }
        public string ReferenceType { get; set; }
        public int? Uom { get; set; }
        public string UomLabel { get; set; }
        public int? ThresholdOperator { get; set; }
        public decimal? ThresholdValue { get; set; }
        public decimal? ThresholdValueTo { get; set; }
        public decimal? ThresholdValueFrom { get; set; }
        public bool NeedAttachment { get; set; }
        public string Note { get; set; }
        public Int32 FormProcedureId { get; set; }
        public bool IsForAllSample { get; set; }
        public bool IsResult { get; set; }
        public string DefaultValue { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public List<GroupValue> GroupValues { get; set; }
        public List<GroupSampleValue> GroupSampleValues { get; set; }
    }

    public class GroupValue
    {
        public Int32 Id { get; set; }
        public int Sequence { get; set; }
        public string Value { get; set; }
        public string AttchmentFile { get; set; }
        public Int32? QcTransactionGroupFormMaterialId { get; set; }
        public Int32? QcTransactionGroupFormToolId { get; set; }
        public int QcTransactionGroupFormParameterId { get; internal set; }
    }

    public class GroupSampleValue
    {
        public Int32 Id { get; set; }
        public Int32 QcTransactionSampleId { get; set; }
        public string SampleCode { get; set; }
        public int Sequence { get; set; }
        public string Value { get; set; }
        public string AttchmentFile { get; set; }
        public Int32? QcTransactionGroupFormMaterialId { get; set; }
        public Int32? QcTransactionGroupFormToolId { get; set; }
        public int QcTransactionGroupFormParameterId { get; internal set; }
    }


}
