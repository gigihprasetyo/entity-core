using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace qcs_product.API.Models
{
    public partial class TransactionGradeRoom
    {
        public TransactionGradeRoom()
        {
            TransactionRelGradeRoomScenario = new HashSet<TransactionRelGradeRoomScenario>();
        }

        public int Id { get; set; }
        public int TestGroupId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string RowStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool? GradeRoomIdentificationTest { get; set; }
        public string GradeRoomDefault { get; set; }
        public int? ObjectStatus { get; set; }

        public virtual ICollection<TransactionRelGradeRoomScenario> TransactionRelGradeRoomScenario { get; set; }
    }
}
