﻿using System;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace qcs_product.Auth.Authorization.Models
{
    public partial class Role
    {
        public int Id { get; set; }
        public string ApplicationCode { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
