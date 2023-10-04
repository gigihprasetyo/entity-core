using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class UserTesting
    {
        public UserTesting()
        {
            Id = (int) DatabaseGeneratedOption.Identity;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string JenisKelamin { get; set; }
    }
}
