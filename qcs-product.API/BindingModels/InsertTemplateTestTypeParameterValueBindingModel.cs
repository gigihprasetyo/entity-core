using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace qcs_product.API.BindingModels
{
    [ExcludeFromCodeCoverage]
    public class InsertTemplateTestTypeParameterValueBindingModel
    {

        [Required]
        public int transactionTemplateTestingId { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        [Required]
        public string testTypeProcessCode { get; set; }
        [Required]
        public int testTypeProcessId { get; set; }
        [Required]
        public string title { get; set; }
        public string instruction { get; set; }
        public string attachmentFile { get; set; }
        public string attachmentStorageName { get; set; }


        public List<InsertParameterValuesBindingModel> ProcedureParameterValues { get; set; }

    }

    [ExcludeFromCodeCoverage]
    public partial class InsertParameterValuesBindingModel
    {
        [Required]
        public int testTypeProcessProcedureId { get; set; }
        public int id { get; set; }
        public string procedureParameterCode { get; set; }
        public string parameterName { get; set; }
        public int sequence { get; set; }
        public string componentName { get; set; }
        public Boolean needAttachment { get; set; }
        public Boolean isNullable { get; set; }
        public object Properties { get; set; }
        public object PropertiesValue { get; set; }

    }


}
