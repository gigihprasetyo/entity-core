using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BindingModels
{
    [ExcludeFromCodeCoverage]
    public partial class StartQcTestBindingModel
    {
        [Required]
        public Int32 QcTestId { get; set; }

        public string PersonelNik { get; set; }

        public string PersonelName { get; set; }

        public string PersonelPairingNik { get; set; }

        public string PersonelPairingName { get; set; }

        [Required]
        public string UpdatedBy { get; set; }
        [Required]
        public bool IsSubmit { get; set; }
        [Required]
        public virtual List<GroupProcessChild> qcTransactionGroupProcessChild { get; set; }
        [Required]
        public bool UpdateStatus { get; set; }

    }

    public partial class GroupProcessChild
    {
        [Required]
        public Int32 Id { get; set; }
        /*[Required]
        public virtual List<GroupFormMaterial> qcTransactionGroupFormMaterial { get; set; }*/
        [Required]
        public virtual List<GroupFormTool> qcTransactionGroupFormTool { get; set; }
        [Required]
        public virtual List<GroupFormProcedure> qcTransactionGroupFormProcedure { get; set; }
        [Required]
        public virtual List<GroupFormGeneral> qcTransactionGroupFormGeneral { get; set; }
    }

    /*public partial class GroupFormMaterial
    {
        public Int32? Id { get; set; }
        [Required]
        public Int32 ItemId { get; set; }
        [Required]
        public string ItemCode { get; set; }
        [Required]
        public string ItemName { get; set; }
        [Required]
        public decimal DefaultPackageQty { get; set; }
        [Required]
        public decimal DefaultQty { get; set; }
    }*/

    public partial class GroupFormTool
    {
        public Int32? Id { get; set; }
        [Required]
        public Int32 ToolId { get; set; }
        public string? ToolCode { get; set; }
        public string? ToolName { get; set; }
        [Required]
        public decimal Quantity { get; set; }
        public Int32? ItemId { get; set; }
    }

    public partial class GroupFormProcedure
    {
        [Required]
        public Int32 Id { get; set; }
        [Required]
        public virtual List<GroupFormParameter> qcTransactionGroupFormParameter { get; set; }
    }

    public partial class GroupFormGeneral
    {
        [Required]
        public Int32 Id { get; set; }
        [Required]
        public virtual List<GroupFormParameter> qcTransactionGroupFormParameter { get; set; }
    }

    public partial class GroupFormParameter
    {
        [Required]
        public Int32 Id { get; set; }
        public virtual List<GroupValues> GroupValues { get; set; }
        public virtual List<GroupSampleValues> GroupSampleValues { get; set; }
    }

    public partial class GroupValues
    {
        public Int32? Id { get; set; }
        public string Value { get; set; }
        public string AttchmentFile { get; set; }
        public Int32? QcTransactionGroupFormMaterialId { get; set; }
        public Int32? QcTransactionGroupFormToolId { get; set; }
    }

    public partial class GroupSampleValues
    {
        public Int32? Id { get; set; }
        public Int32 qcTransactionSampleId { get; set; }
        public string Value { get; set; }
        public string AttchmentFile { get; set; }
        public Int32? QcTransactionGroupFormMaterialId { get; set; }
        public Int32? QcTransactionGroupFormToolId { get; set; }
    }


}
