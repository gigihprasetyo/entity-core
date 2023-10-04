using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace qcs_product.API.Models
{
    public partial class TransactionTestScenario
    {
        public TransactionTestScenario()
        {
            TransactionRelGradeRoomScenario = new HashSet<TransactionRelGradeRoomScenario>();
            TransactionRelTestScenarioParam = new HashSet<TransactionRelTestScenarioParam>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string RowStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Label { get; set; }

        public virtual ICollection<TransactionRelGradeRoomScenario> TransactionRelGradeRoomScenario { get; set; }
        public virtual ICollection<TransactionRelTestScenarioParam> TransactionRelTestScenarioParam { get; set; }
    }
}
