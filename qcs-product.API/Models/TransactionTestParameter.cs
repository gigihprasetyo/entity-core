using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace qcs_product.API.Models
{
    public partial class TransactionTestParameter
    {
        public TransactionTestParameter()
        {
            TransactionRelTestScenarioParam = new HashSet<TransactionRelTestScenarioParam>();
        }

        public int Id { get; set; }
        public int TestGroupId { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Sequence { get; set; }
        public int? OrganizationId { get; set; }
        public int? QcProcessId { get; set; }
        public string ShortName { get; set; }

        public virtual TransactionOrganization Organization { get; set; }
        public virtual QcProcess QcProcess { get; set; }
        public virtual EnumConstant TestGroup { get; set; }
        public virtual ICollection<TransactionRelTestScenarioParam> TransactionRelTestScenarioParam { get; set; }
    }
}
