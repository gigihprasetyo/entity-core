namespace qcs_product.API.BindingModels
{
    public class EditBatchLineRequestQcBindingModel
    {
        public int? Id { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string NoBatch { get; set; }
        public string Notes { get; set; }
    }
}
