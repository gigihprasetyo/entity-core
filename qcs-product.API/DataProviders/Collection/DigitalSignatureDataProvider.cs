using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using qcs_product.API.Models;
using qcs_product.API.Infrastructure;
using qcs_product.Constants;
using qcs_product.API.Helpers;
using System.Security.Cryptography;
using System.Text;
using BC = BCrypt.Net.BCrypt;

namespace qcs_product.API.DataProviders
{

    public class DigitalSignatureDataProvider : IDigitalSignatureDataProvider
    {
        private readonly QcsProductContext _context;

        [ExcludeFromCodeCoverage]
        public DigitalSignatureDataProvider(QcsProductContext context)
        {
            _context = context;
        }

        [ExcludeFromCodeCoverage]
        public async Task<DigitalSignature> GetDigitalSignatureById(string serialNumber, string nik)
        {
            return await
            (
                from data in _context.digitalSigantures
                where
                    data.Nik == nik && data.EndDate >= DateTime.UtcNow.AddHours(7) &&
                    data.BeginDate <= DateHelper.Now()
                orderby data.CreatedAt descending
                select data
            ).FirstOrDefaultAsync(); ;
        }

        public async Task<bool> Authenticate(string serialNumber, string nik)
        {
            DateTime now = DateTime.UtcNow;
            DateTime nowTimestamp = now.AddHours(ApplicationConstant.TIMEZONE);
            var user = await
            (
                from data in _context.digitalSigantures
                where data.Nik == nik &&
                        data.EndDate >= nowTimestamp &&
                        data.BeginDate <= nowTimestamp
                orderby data.CreatedAt descending
                select data
            ).FirstOrDefaultAsync();

            if (user == null || !BC.Verify(serialNumber, user.SerialNumber))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<DigitalSignature> Insert(DigitalSignature data)
        {
            await _context.digitalSigantures.AddAsync(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<DigitalSignature> Update(DigitalSignature data)
        {
            _context.digitalSigantures.Update(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<DigitalSignature> GetLastByNIK(string nik)
        {
            DateTime now = DateTime.UtcNow;
            DateTime nowTimestamp = DateHelper.Now();

            return await
            (
                from data in _context.digitalSigantures
                where data.Nik == nik &&
                        data.EndDate >= nowTimestamp
                orderby data.CreatedAt descending
                select data
            ).Skip(1).Take(1).FirstOrDefaultAsync();
        }

        public async Task<DigitalSignature> GetLastByNIKStep1(string NIK)
        {
            DateTime now = DateTime.UtcNow;
            DateTime nowTimestamp = now.AddHours(ApplicationConstant.TIMEZONE);

            return await
            (
                from data in _context.digitalSigantures
                where data.Nik == NIK &&
                        data.EndDate >= nowTimestamp
                orderby data.CreatedAt descending
                select data
            ).Skip(1).Take(1).FirstOrDefaultAsync();
        }


    }
}