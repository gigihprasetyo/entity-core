using qcs_product.API.Models;
using System.Collections.Generic;

namespace qcs_product.API.ViewModels
{
    public class InsertTestingPQViewModel
    {
        public List<TransactionTestingPersonnel> Personnel { get; set; }
        public List<TransactionHtrTestingPersonnel> PersonnelHistory { get; set; }
    }
}
