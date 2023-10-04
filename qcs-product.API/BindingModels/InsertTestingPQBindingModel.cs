using qcs_product.API.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace qcs_product.API.BindingModels
{
    public partial class InsertTestingPQBindingModel
    {
        [Required]
        public int TestingId { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Position { get; set; }
        public string Note { get; set; }
        public string TestingCode { get; set; }
        public List<TransactionTestingPersonnel> Personnels { get; set; }
    }
}
