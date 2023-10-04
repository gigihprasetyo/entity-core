using System;

namespace qcs_product.API.Models
{
    public class TransactionTestingPersonnel
    {
        public string NewNIK { get; set; }
        public string Nama { get; set; }
        public string CreatedBy { get; set; }
        public string Nik { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int TestingId { get; set; }
        public int Id { get; set; }
        public string PosisiCode { get; set; }
        public int PosisiId { get; set; }
        public string UpdatedBy { get; set; }
        public string RowStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Posisi { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public string TestingCode { get; set; }
    }
}
