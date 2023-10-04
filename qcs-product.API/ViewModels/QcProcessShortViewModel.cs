using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class QcProcessShortViewModel
    {
        public int Id { get; set; }
        public int Sequence { get; set; }
        public string Name { get; set; }
        public int? RoomId { get; set; }
        public int? IsInputForm { get; set; }
        public int? AddSampleLayoutType { get; set; }
        public int? PurposeId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public List<QcProcessChildViewModel> QcProcessChild { get; set; }
    }

    public partial class QcProcessChildViewModel
    {
        public int Id { get; set; }
        public int Sequence { get; set; }
        public string Name { get; set; }
        public int? RoomId { get; set; }
        public int? IsInputForm { get; set; }
        public int? AddSampleLayoutType { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}
