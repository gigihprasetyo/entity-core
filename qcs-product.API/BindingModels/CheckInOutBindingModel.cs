using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace qcs_product.API.BindingModels
{
    [ExcludeFromCodeCoverage]
    public class CheckInOutBindingModel
    {
        [Required]
        public string NewNIK { get; set; }
        [Required]
        public string Nama { get; set; }
        [Required]
        public string TestingCode { get; set; }
        [Required]
        public string Nik { get; set; }
        [Required]
        public int TestingId { get; set; }
        [Required]
        public int Id { get; set; }
        [Required]
        public string PosisiCode { get; set; }
        [Required]
        public int PosisiId { get; set; }
        [Required]
        public string Posisi { get; set; }
        [Required]
        public string Pin { get; set; }

        public string? Note { get; set; }
    }
}
