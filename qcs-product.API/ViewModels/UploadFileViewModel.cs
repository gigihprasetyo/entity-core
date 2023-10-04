using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class UploadFileViewModel
    {
        public string ObjectName { get; set; }
        public string FileName { get; set; }
        public string Ext { get; set; }
        public string MediaLink { get; set; }
        public ulong? Size { get; set; }
    }
}
