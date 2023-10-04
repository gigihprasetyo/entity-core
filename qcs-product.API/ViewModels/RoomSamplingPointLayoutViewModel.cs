using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class RoomSamplingPointLayoutViewModel
    {
        public int SamplingId { get; set; }
        public int RoomId { get; set; }
        public string RoomCode { get; set; }
        public string RoomName { get; set; }
        public List<SamplingPointLayoutViewModel> SamplingPointLayouts { get; set; }
    }

    public class SamplingPointLayoutViewModel
    {
        public string AttachmentFile { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
    }
}
