namespace qcs_product.API.ViewModels
{
    public class TransactionBatchLineViewModel
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string NoBatch { get; set; }
        public string Notes { get; set; }

        public int TrsBatchId { get; set; }
    }
}
