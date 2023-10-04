namespace qcs_product.API.ViewModels
{
    public class TransactionBatchAttachmentViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AttachmentFile { get; set; }
        public string FileName { get; set; }
        public string AttachmentStorageName { get; set; }

        public int TrsBatchId { get; set; }
    }
}
