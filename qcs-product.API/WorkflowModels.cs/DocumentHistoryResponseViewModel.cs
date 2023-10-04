using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace qcs_product.API.WorkflowModels
{
    [ExcludeFromCodeCoverage]
    public class DocumentHistoryResponseViewModel
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public List<DocumentHistoryViewModel> History { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class DocumentHistoryViewModel
    {
        public string StatusName { get; set; }
        public string StatusChangeDate { get; set; }
        public List<DocumentHistoryPICViewModel> PICs { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class DocumentHistoryPICViewModel
    {
        public string ActionName { get; set; }
        public bool IsPending { get; set; }
        public string OrgId { get; set; }
        public string OrgName { get; set; }
        public string OrgPositionId { get; set; }
        public string OrgPositionName { get; set; }
        public string Notes { get; set; }
        public string ActionDate { get; set; }
    }
    
    [ExcludeFromCodeCoverage]
    public class DocumentHistoryWithCodeViewModel
    {
        public DocumentHistoryResponseViewModel HistoryResponse { get; set; }
        public string DocumentCode { get; set; }
    }
}