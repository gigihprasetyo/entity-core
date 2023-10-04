using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class StorageTemperature
    {
        public Int32 Id { get; set; }
        public Int32 ItemId { get; set; }
        public string Name { get; set; }
        public string TresholdOperator { get; set; }
        public double TresholdValue { get; set; }
        public double TresholdMin { get; set; }
        public double TresholdMax { get; set; }
        public string RowStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
