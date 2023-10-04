using System.ComponentModel.DataAnnotations;

namespace qcs_product.API.BindingModels
{
    public partial class InsertParameterNoteBindingModel
    {
        [Required]
        public int ProcedureParameterId { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Position { get; set; }
        [Required]
        public string Note { get; set; }
    }
}
