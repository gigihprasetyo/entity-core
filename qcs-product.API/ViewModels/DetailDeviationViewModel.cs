using System;
using System.Collections.Generic;

namespace qcs_product.API.ViewModels
{
    public class DetailDeviationViewModel
    {
        public string SampleId { get; set; }
        public int TransactionTestingProcedureParameter { get; set; }
        public string Note { get; set; }
        public List<ListAttachment> Attachments { get; set; }
        public int DeviationLevel { get; set; }
        public string Result { get; set; }
        public List<ListHistoryDeviation> History { get; set; }

    }
    public partial class ListAttachment
    {
        public string AttachmentFile { get; set; }
        public string Ext { get; set; }
        public string AttachmentStorageName { get; set; }
    }
    public partial class ListAttachmentTesting
    {
        public string Filename { get; set; }
        public string Ext { get; set; }
        public string MediaLink { get; set; }
    }

    public partial class ListHistoryDeviation
    {
        public string Note { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public DateTime Date { get; set; }
    }
}
