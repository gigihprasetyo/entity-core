using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using qcs_product.API.Models;

namespace qcs_product.API.DataProviders
{
    public interface IDigitalSignatureDataProvider
    {
        public Task<DigitalSignature> GetDigitalSignatureById(string data, string nik);
        public Task<DigitalSignature> Insert(DigitalSignature data);
        public Task<DigitalSignature> Update(DigitalSignature data);
        public Task<DigitalSignature> GetLastByNIK(string nik);
        public Task<bool> Authenticate(string nik, string pin);
        public Task<DigitalSignature> GetLastByNIKStep1(string NIK);
    }
}