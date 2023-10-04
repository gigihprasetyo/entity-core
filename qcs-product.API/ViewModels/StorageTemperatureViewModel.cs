using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class StorageTemperatureViewModel
    {
        public Int32 Id { get; set; }
        public Int32 ItemId { get; set; }
        public string Name { get; set; }
        public string TresholdOperator { get; set; }
        public string TresholdOperatorName { get; set; }
        public double TresholdValue { get; set; }
        public double TresholdMin { get; set; }
        public double TresholdMax { get; set; }
    }
}
