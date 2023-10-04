using qcs_product.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public partial class MessageNotificationSamplingViewModel
    {
        public string Subject { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string NoHandphone { get; set; }
        public string MessageEmail { get; set; }
        public string MessageWhatsApp { get; set; }
        public string NoRequest { get; set; }
        public string ItemName { get; set; }
        public string NoBatch { get; set; }
        public string Status { get; set; }
        public string MenuName { get; set; }


        public MessageNotificationSamplingViewModel(string noRequest, int? typeRequestId, string noBatch, string itemName, string emailAddress, string status, string name, string noHandphone)
        {
            NoRequest = noRequest;
            NoBatch = noBatch;
            ItemName = itemName;
            EmailAddress = emailAddress;
            Status = status;
            Name = name;
            NoHandphone = noHandphone;
            Subject = GenerateSubject(typeRequestId);
            GenerateMessageEmail();
        }

        private string GenerateSubject(int? typeRequestId)
        {
            switch (Status)
            {
                case ApplicationConstant.NEW_ACTION_NOTIF:
                    Subject = $"{Status} {getRequestName(typeRequestId)} {NoRequest} - {ItemName}/{NoBatch}";
                    break;
                case ApplicationConstant.UPDATED_ACTION_NOTIF:
                    Subject = $"{getRequestName(typeRequestId)} {NoRequest} - {ItemName}/{NoBatch} {Status}";
                    break;
                case ApplicationConstant.CANCELLED_ACTION_NOTIF:
                    Subject = $"{Status} {getRequestName(typeRequestId)} {NoRequest} - {ItemName}/{NoBatch}";
                    break;
                case ApplicationConstant.APPROVED_ACTION_NOTIF:
                    Subject = $"{getRequestName(typeRequestId)} {NoRequest} - {ItemName}/{NoBatch} has been {Status}";
                    break;
                case ApplicationConstant.REJECTED_ACTION_NOTIF:
                    Subject = $"{getRequestName(typeRequestId)} {NoRequest} - {ItemName}/{NoBatch} has been {Status}";
                    break;

            }

            return Subject;
        }
        private string getRequestName(int? typeRequestId)
        {
            var RequestName = "";
            switch (typeRequestId)
            {
                case ApplicationConstant.REQUEST_SAPMLING_EMM:
                    RequestName = "Sampling EM Mikrobiolog";
                    break;
                case ApplicationConstant.REQUEST_SAPMLING_PC:
                    RequestName = "Sampling EM Particle Counter";
                    break;
            }

            return RequestName;

        }

        private void GenerateMessageEmail()
        {


            switch (Status)
            {
                case ApplicationConstant.NEW_ACTION_NOTIF:
                    MessageEmail = $"Terdapat Data Sampling baru untuk {ItemName} no Batch {NoBatch} dengan nomor permohonan {NoRequest}. " +
                      $"Silahkan cek aplikasi Q100 untuk melakukan review dan approval data pengambilan sample";
                    MessageWhatsApp = $"Hallo {Name}, terdapat Data Sampling baru untuk {ItemName} no Batch {NoBatch} dengan nomor permohonan {NoRequest}. " +
                        $"Silahkan cek aplikasi Q100 untuk melakukan review dan approval data pengambilan sample";
                    break;
                case ApplicationConstant.UPDATED_ACTION_NOTIF:
                    MessageEmail = $"Data Sampling {ItemName} no Batch {NoBatch} dengan nomor permohonan {NoRequest} telah diperbaharui. " +
                      $"Silahkan cek aplikasi Q100 untuk melakukan review dan approval data pengambilan sample";
                    MessageWhatsApp = $"Hallo {Name}, Data Sampling {ItemName} no Batch {NoBatch} dengan nomor permohonan {NoRequest} telah diperbaharui. " +
                        $"Silahkan cek aplikasi Q100 untuk melakukan review dan approval data pengambilan sample";
                    break;
                case ApplicationConstant.CANCELLED_ACTION_NOTIF:
                    MessageEmail = $"Data Sampling {ItemName} no Batch {NoBatch} dengan nomor permohonan {NoRequest} telah dibatalkan. ";
                    MessageWhatsApp = $"Hallo {Name}, Data Sampling {ItemName} no Batch {NoBatch} dengan nomor permohonan {NoRequest} telah dibatalkan. ";
                    break;
                case ApplicationConstant.APPROVED_ACTION_NOTIF:
                    MessageEmail = $"Request untuk {ItemName} no Batch {NoBatch} dengan nomor permohonan {NoRequest}. telah disetujui" +
                      $"Silahkan cek aplikasi Q100 untuk melakukan review dan approval data pengambilan sample";
                    MessageWhatsApp = $"Hallo {Name}, Data Sampling {ItemName} no Batch {NoBatch} dengan nomor permohonan {NoRequest} telah disetujui. " +
                        $"Silahkan cek aplikasi Q100 untuk melakukan review dan approval data pengambilan sample";
                    break;
                case ApplicationConstant.REJECTED_ACTION_NOTIF:
                    MessageEmail = $"Sampling untuk {ItemName} no Batch {NoBatch} dengan nomor permohonan {NoRequest}. telah ditolak" +
                      $"Silahkan cek aplikasi Q100 untuk melakukan review dan approval data pengambilan sample";
                    MessageWhatsApp = $"Hallo {Name}, Data Sampling {ItemName} no Batch {NoBatch} dengan nomor permohonan {NoRequest} telah ditolak. " +
                        $"Silahkan cek aplikasi Q100 untuk melakukan review dan approval data pengambilan sample";
                    break;
            }


        }

    }

    
}
