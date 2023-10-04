using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class ShortDataListViewModel
    {
        public Int32 Id { get; set; }
        public string Label { get; set; }
        public string Code { get; set; }
        public Int32? GroupId { get; set; }
        public string GroupName { get; set; }
    }
}
