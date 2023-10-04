using qcs_product.API.Models;
using System;
using System.Collections.Generic;

namespace qcs_product.API.ViewModels
{
    public class TransactionTestingViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime TestingDate { get; set; }
        public int ObjectStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string TestTypeNameIdn { get; set; }
        public string TestTypeNameEn { get; set; }
        public string TestTypeCode { get; set; }
        public int TestTypeId { get; set; }
        public string TestTypeMethodName { get; set; }
        public string TestTypeMethodCode { get; set; }
        public int TestTypeMethodId { get; set; }
        public int? TestTemplateId { get; set; }
        public int TotalSampling { get; set; }
        public List<TransactionTestingProcedureViewModel> procedures { get; set; }
    }

    public class TransactionTestingProcedureViewModel
    {
        public int Id { get; set; }
        public int? TransactionTestTypeMethodId { get; set; }
        public string TestTypeMethodCode { get; set; }
        public string Title { get; set; }
        public string Instruction { get; set; }
        public string RowStatus { get; set; }
        public int Sequence { get; set; }
        public string AttachmentStorageName { get; set; }
        public string AttachmentFile { get; set; }
        public bool IsEachSample { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Status { get; set; }
        public List<TransactionTestingProcedureParameterViewModel> parameters { get; set; }
        public List<ParameterTestingProcedureSample> parameterSamples { get; set; }
    }

    public class ParameterTestingProcedureSample
    {
        public int Id { get; set; }
        public int SampleId { get; set; }
        public string SampleName { get; set; }
        public List<TransactionTestingProcedureParameterAttachment> Attachments { get; set; }
        public List<TransactionTestingProcedureParameterNote> Notes { get; set; }
        public List<TransactionHtrTestingProcedureParameter> Histories { get; set; }
    }

    public class TransactionTestingProcedureParameterViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int? DeviationLevel { get; set; }
        public string DeviationNote { get; set; }
        public bool HasAttachment { get; set; }
        public int? InputTypeId { get; set; }
        public bool IsNullable { get; set; }
        public string Name { get; set; }
        public int TransactionTestingProcedureId { get; set; }
        public string TestTypeProcedureCode { get; set; }
        public object Properties { get; set; }
        public object PropertiesValue { get; set; }
        public string RowStatus { get; set; }
        public int Sequence { get; set; }
        public bool? IsDeviation { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public List<TransactionTestingProcedureParameterAttachment> Attachments { get; set; }
        public List<TransactionHtrProcessProcedureParameterAttachment> AttachmentHistories { get; set; }
        public List<TransactionTestingProcedureParameterNote> Notes { get; set; }
        public List<TransactionHtrTestingProcedureParameter> Histories { get; set; }

    }
}
