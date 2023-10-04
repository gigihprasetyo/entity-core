using qcs_product.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public partial class MessageNotificationRequestQcsViewModel
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

        public MessageNotificationRequestQcsViewModel()
        {

        }

        public MessageNotificationRequestQcsViewModel(string noRequest, string noBatch, string itemName, string emailAddress, string status, string name, string noHandphone)
        {
            NoRequest = noRequest;
            NoBatch = noBatch;
            ItemName = itemName;
            EmailAddress = emailAddress;
            Status = status;
            Name = name;
            NoHandphone = noHandphone;
            Subject = GenerateSubject();
            GenerateMessageEmail();
        }

        private string GenerateSubject()
        {
            switch (Status)
            {
                case ApplicationConstant.NEW_ACTION_NOTIF:
                    Subject = $"{Status} {ApplicationConstant.QC_REQUEST_MENU_NAME} {NoRequest} - {ItemName}/{NoBatch}";
                    break;
                case ApplicationConstant.UPDATED_ACTION_NOTIF:
                    Subject = $"{ApplicationConstant.QC_REQUEST_MENU_NAME} {NoRequest} - {ItemName}/{NoBatch} {Status}";
                    break;
                case ApplicationConstant.CANCELLED_ACTION_NOTIF:
                    Subject = $"{Status} {ApplicationConstant.QC_REQUEST_MENU_NAME} {NoRequest} - {ItemName}/{NoBatch}";
                    break;
                case ApplicationConstant.APPROVED_ACTION_NOTIF:
                    Subject = $"{ApplicationConstant.QC_REQUEST_MENU_NAME} {NoRequest} - {ItemName}/{NoBatch} has been {Status}";
                    break;
                case ApplicationConstant.REJECTED_ACTION_NOTIF:
                    Subject = $"{ApplicationConstant.QC_REQUEST_MENU_NAME} {NoRequest} - {ItemName}/{NoBatch} has been {Status}";
                    break;

            }

            return Subject;
        }

        private void GenerateMessageEmail()
        {


            switch (Status)
            {
                case ApplicationConstant.NEW_ACTION_NOTIF:
                    MessageEmail = $"<b>[Q100+]</b> Terdapat Data Request baru untuk <b>{ItemName}</b> no Batch <b>{NoBatch}</b> dengan nomor permohonan <b>{NoRequest}</b>. " +
                      $"Silahkan cek aplikasi Q100+ untuk melakukan review dan approval data permohonan uji";
                    MessageWhatsApp = $"Hallo *{Name}*, terdapat Data Request baru untuk *{ItemName}* no Batch *{NoBatch}* dengan nomor permohonan *{NoRequest}*. " +
                        $"Silahkan cek aplikasi Q100+ untuk melakukan review dan approval data permohonan uji";
                    break;
                case ApplicationConstant.UPDATED_ACTION_NOTIF:
                    MessageEmail = $"<b>[Q100+]</b> Data Request <b>{ItemName}</b> no Batch <b>{NoBatch}</b> dengan nomor permohonan <b>{NoRequest}</b> telah <b>diperbaharui</b>. " +
                      $"Silahkan cek aplikasi Q100+ untuk melakukan review dan approval data permohonan uji";
                    MessageWhatsApp = $"Hallo *{Name}*, Data Request *{ItemName}* no Batch *{NoBatch}* dengan nomor permohonan *{NoRequest}* telah *diperbaharui*. " +
                        $"Silahkan cek aplikasi Q100+ untuk melakukan review dan approval data permohonan uji";
                    break;
                case ApplicationConstant.CANCELLED_ACTION_NOTIF:
                    MessageEmail = $"<b>[Q100+]</b> Data Request <b>{ItemName}</b> no Batch <b>{NoBatch}</b> dengan nomor permohonan <b>{NoRequest}</b> telah <b>dibatalkan</b>. ";
                    MessageWhatsApp = $"Hallo *{Name}*, Data Request *{ItemName}* no Batch *{NoBatch}* dengan nomor permohonan *{NoRequest}* telah *dibatalkan*. ";
                    break;
                case ApplicationConstant.APPROVED_ACTION_NOTIF:
                    MessageEmail = $"<b>[Q100+]</b> Request untuk <b>{ItemName}</b> no Batch <b>{NoBatch}</b> dengan nomor permohonan <b>{NoRequest}</b>. telah <b>disetujui</b>" +
                      $"Silahkan cek aplikasi Q100 untuk melakukan review dan approval data permohonan uji";
                    MessageWhatsApp = $"Hallo *{Name},* Data Request *{ItemName}* no Batch *{NoBatch}* dengan nomor permohonan *{NoRequest}* telah *disetujui*. " +
                        $"Silahkan cek aplikasi Q100+ untuk melakukan review dan approval data permohonan uji";
                    break;
                case ApplicationConstant.REJECTED_ACTION_NOTIF:
                    MessageEmail = $"<b>[Q100+]</b> Request untuk <b>{ItemName}</b> no Batch <b>{NoBatch}</b> dengan nomor permohonan <b>{NoRequest}</b>. telah <b>ditolak</b>" +
                      $"Silahkan cek aplikasi Q100+ untuk melakukan review dan approval data permohonan uji";
                    MessageWhatsApp = $"Hallo *{Name}*, Data Request *{ItemName}* no Batch *{NoBatch}* dengan nomor permohonan *{NoRequest}* telah *ditolak*. " +
                        $"Silahkan cek aplikasi Q100+ untuk melakukan review dan approval data permohonan uji";
                    break;
            }


        }
    }
}
