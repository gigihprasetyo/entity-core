using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    [ExcludeFromCodeCoverage]
    public partial class InsertNotificationServiceViewModel
    {
        public int RecipientType { get; set; }
        public string Recipient { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public int PriorityId { get; set; }
        public int NotificationType { get; set; }
        public string ObjectMethod { get; set; }
        public string ObjectId { get; set; }
    }
}
