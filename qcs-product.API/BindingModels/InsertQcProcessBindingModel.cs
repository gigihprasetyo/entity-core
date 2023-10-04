using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BindingModels
{
    [ExcludeFromCodeCoverage]
    public partial class InsertQcProcessBindingModel
    {
        public int Sequence { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public int? RoomId { get; set; }
        public int? IsInputForm { get; set; }
        public string CreatedBy { get; set; }

        public virtual List<InserFormMaterialBindingModel> FormMaterial { get; set; }
        public virtual List<InsertFormProcedureBindingModel> FormProcedure { get; set; }
        public virtual List<InsertFormToolBindingModel> FormTool { get; set; }
    }

    public partial class InserFormMaterialBindingModel
    {
        public int Sequence { get; set; }
        public int ItemId { get; set; }
        public decimal DefaultPackageQty { get; set; }
        public int UomPackage { get; set; }
        public decimal DefaultQty { get; set; }
        public int Uom { get; set; }
        public string GroupName { get; set; }
    }

    public partial class InsertFormProcedureBindingModel
    {
        public int Sequence { get; set; }
        public string Description { get; set; }
        public List<InsertFormParameterBindingModel> FormParameter { get; set; }
    }

    public partial class InsertFormToolBindingModel
    {
        public int Sequence { get; set; }
        public int Type { get; set; }
        public int ToolId { get; set; }
        public int? ItemId { get; set; }
        public decimal Qty { get; set; }
    }

    public partial class InsertFormParameterBindingModel
    {
        public int Sequence { get; set; }
        public string Label { get; set; }
        public string Code { get; set; }
        public int InputType { get; set; }
        public int Uom { get; set; }
        public int? ThresholdOperator { get; set; }
        public decimal? ThresholdValue { get; set; }
        public decimal? ThresholdValueTo { get; set; }
        public decimal? ThresholdValueFrom { get; set; }
        public bool NeedAttachment { get; set; }
        public string Note { get; set; }
        public bool IsForAllSample { get; set; }
        public bool IsResult { get; set; }
        public string DefaultValue { get; set; }
    }
}
