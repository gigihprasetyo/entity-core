namespace qcs_product.API.BindingModels
{
    public class InsertBatchAttachmentRequestQcBindingModel
    {
        public string Title { get; set; }
        public string AttachmentFile { get; set; }
        public string AttachmentStorageName { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
    }
}
