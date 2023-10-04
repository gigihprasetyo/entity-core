using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace qcs_product.API.BindingModels
{
    [ExcludeFromCodeCoverage]
    public partial class EditRequestQcsBindingModel
    {
        [Required]
        public Int32 Id { get; set; }
        [Required]
        public Int32 TypeRequestId { get; set; }
        [Required]
        public string TypeRequest { get; set; }
        [Required]
        public DateTime Date { get; set; }

        public Int32 PurposeId { get; set; }

        public string PurposeName { get; set; }

        public Int32? RequestQcsId { get; set; }

        public string RequestQcsNo { get; set; }

        public Int32? ItemId { get; set; }

        public string ItemName { get; set; }

        public Int32? ProductFormId { get; set; }
        public string ProductFormName { get; set; }
        public Int32? ProductGroupId { get; set; }
        public string ProductGroupName { get; set; }
        public Int32? ProductPresentationId { get; set; }
        public string ProductPresentationName { get; set; }
        public Int32? StorageTemperatureId { get; set; }
        public string StorageTemperatureName { get; set; }
        public string NoBatch { get; set; }
        public int? ProcessId { get; set; }
        public string ProcessName { get; set; }
        public Int32 ProductPhaseId { get; set; }
        public string ProductPhaseName { get; set; }
        [Required]
        public string UpdatedBy { get; set; }
        [Required]
        public bool IsSubmit { get; set; }
        [Required]
        public List<TestTypeQcsBindingModel> TestTypesQcs { get; set; }
    }
}
