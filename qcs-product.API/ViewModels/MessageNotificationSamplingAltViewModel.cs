using qcs_product.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class MessageNotificationSamplingAltViewModel
    {
        public string Subject { get; set; }
        public string Name { get; set; }
        public string PicNames { get; set; }
        public string EmailAddress { get; set; }
        public string NoHandphone { get; set; }
        public string MessageEmail { get; set; }
        public string MessageWhatsApp { get; set; }
        public string NoRequest { get; set; }
        public string ItemName { get; set; }
        public string Purposes { get; set; }
        public string NoBatch { get; set; }
        public string Status { get; set; }
        public string MenuName { get; set; }

        public MessageNotificationSamplingAltViewModel(string noRequest, int? typeRequestId, string noBatch, string itemName, string purpose, string emailAddress, string status, string name, string noHandphone, string PicName)
        {
            NoRequest = noRequest;
            NoBatch = noBatch;
            ItemName = itemName;
            Purposes = purpose;
            EmailAddress = emailAddress;
            Status = status;
            Name = name;
            NoHandphone = noHandphone;
            PicNames = PicName;
            Subject = GenerateSubject(typeRequestId);
            GenerateMessageEmail();
        }

        private string GenerateSubject(int? typeRequestId)
        {
            switch (Status)
            {
                case ApplicationConstant.NEW_ACTION_NOTIF:
                    Subject = $"{Status} {getRequestName(typeRequestId)} {NoRequest}{GenSubjectItemBatch(ItemName, NoBatch)}";
                    break;
                case ApplicationConstant.UPDATED_ACTION_NOTIF:
                    Subject = $"{getRequestName(typeRequestId)} {NoRequest}{GenSubjectItemBatch(ItemName, NoBatch)}";
                    break;
                case ApplicationConstant.CANCELLED_ACTION_NOTIF:
                    Subject = $"{Status} {getRequestName(typeRequestId)} {NoRequest}{GenSubjectItemBatch(ItemName, NoBatch)}";
                    break;
                case ApplicationConstant.APPROVED_ACTION_NOTIF:
                    Subject = $"{getRequestName(typeRequestId)} {NoRequest}{GenSubjectItemBatch(ItemName, NoBatch)} has been {Status}";
                    break;
                case ApplicationConstant.REJECTED_ACTION_NOTIF:
                    Subject = $"{getRequestName(typeRequestId)} {NoRequest}{GenSubjectItemBatch(ItemName, NoBatch)} has been {Status}";
                    break;

            }

            return Subject;
        }

        private void GenerateMessageEmail()
        {

            switch (Status)
            {
                case ApplicationConstant.NEW_ACTION_NOTIF:
                    // MessageEmail = $"<b>[Q100+]</b> Terdapat Data Sampling baru untuk " +
                    //     $"{GenMessageItemBatch(ItemName, NoBatch)}" +
                    //     $"nomor permohonan <b>{NoRequest}</b>. " +
                    //     $"pada <b>Tujuan Pengujian {Purposes}</b> telah tersedia. " +
                    //     $"Silahkan cek aplikasi Q100 untuk melakukan review dan approval data pengambilan sample.";
                    // MessageWhatsApp = $"Hallo *{Name}*, terdapat Data Sampling baru untuk " +
                    //     $"{GenMessageWAItemBatch(ItemName, NoBatch)}" +
                    //     $"nomor permohonan *{NoRequest}* " +
                    //     $"pada *Tujuan Pengujian {Purposes}* telah tersedia. " +
                    //     $"Silahkan cek aplikasi Q100 untuk melakukan review dan approval data pengambilan sample.";
                    MessageEmail = $"<h4>[Q100-Sampling-Info]</h4> " +
                                $"" +
                                $"<p>Hallo <b>{Name}</b>,</p> " +
                                $"<p>Mohon untuk <b>me-<i>review</i></b> data <b>SAMPLING</b> berikut:</p> " +
                                $"<p>Nomor Batch        : <b>{NoBatch}</b>,</p> " +
                                $"<p>Nomor Permohonan   : <b>{NoRequest}</b>,</p> " +
                                $"<p>Tujuan Pengujian   : <b>{Purposes}</b>.</p> " +
                                $"" +
                                $"<p>Silahkan cek aplikasi Q100+ untuk melihat data tersebut. Terima kasih.</p>";
                    MessageWhatsApp = $"*[Q100-Sampling-Info]*, Hallo *{Name}*, Mohon untuk *me-* *_review_* data *SAMPLING* berikut, Nomor Batch : *{NoBatch}*, Nomor Permohonan : *{NoRequest}*, Tujuan Pengujian : *{Purposes}*. Silahkan cek aplikasi Q100+ untuk melihat data tersebut. Terima kasih.";
                    break;
                case ApplicationConstant.UPDATED_ACTION_NOTIF:
                    // MessageEmail = $"<b>[Q100+]</b> Data Sampling " +
                    //     $"{GenMessageItemBatch(ItemName, NoBatch)}" +
                    //     $"nomor permohonan <b>{NoRequest}</b>. " +
                    //     $"pada <b>Tujuan Pengujian {Purposes}</b> telah <b>diperbaharui</b>. " +
                    //     $"Silahkan cek aplikasi Q100 untuk melakukan review dan approval data pengambilan sample.";
                    // MessageWhatsApp = $"Hallo *{Name}*, Data Sampling " +
                    //     $"{GenMessageWAItemBatch(ItemName, NoBatch)}" +
                    //     $"nomor permohonan *{NoRequest}* " +
                    //     $"pada *Tujuan Pengujian {Purposes}* telah *diperbaharui*. " +
                    //     $"Silahkan cek aplikasi Q100 untuk melakukan review dan approval data pengambilan sample.";
                    MessageEmail = $"<h4>[Q100-Sampling-Reminder]</h4> " +
                                $"" +
                                $"<p>Hallo <b>{Name}</b>,</p> " +
                                $"<p>telah <b>diperbarui</b> oleh <b>{PicNames}</b> " +
                                $"<p>pada data <b>SAMPLING</b> berikut:</p> " +
                                $"<p>Nomor Batch        : <b>{NoBatch}</b>,</p> " +
                                $"<p>Nomor Permohonan   : <b>{NoRequest}</b>,</p> " +
                                $"<p>Tujuan Pengujian   : <b>{Purposes}</b>.</p> " +
                                $"" +
                                $"<p>Silahkan cek aplikasi Q100+ untuk melakukan <b><i>review</i></b> pada data tersebut. Terima kasih.</p>";
                    MessageWhatsApp = $"*[Q100-Sampling-Reminder]*, Hallo *{Name}*, telah *diperbarui* oleh *{PicNames}* pada data *SAMPLING* berikut, Nomor Batch : *{NoBatch}*, Nomor Permohonan : *{NoRequest}*, Tujuan Pengujian : *{Purposes}*. Silahkan cek aplikasi Q100+ untuk mereview data tersebut. Terima kasih.";
                    break;
                case ApplicationConstant.CANCELLED_ACTION_NOTIF:
                    MessageEmail = $"<b>[Q100+]</b> Data Sampling " +
                        $"{GenMessageItemBatch(ItemName, NoBatch)}" +
                        $"nomor permohonan <b>{NoRequest}</b> " +
                        $"pada <b>Tujuan Pengujian {Purposes}</b> telah <b>dibatalkan</b> oleh <b>{PicNames}</b>. ";
                    MessageWhatsApp = $"Hallo *{Name}*, Data Sampling " +
                        $"{GenMessageWAItemBatch(ItemName, NoBatch)}" +
                        $"nomor permohonan *{NoRequest}*. " +
                        $"pada *Tujuan Pengujian {Purposes}* telah *dibatalkan* oleh *{PicNames}*. ";
                    break;
                case ApplicationConstant.APPROVED_ACTION_NOTIF:
                    MessageEmail = $"<b>[Q100+]</b> Data Sampling " +
                        $"{GenMessageItemBatch(ItemName, NoBatch)}" +
                        $"nomor permohonan <b>{NoRequest}</b>. " +
                        $"pada <b>Tujuan Pengujian {Purposes}</b> telah <b>disetujui</b> oleh <b>{PicNames}</b>. ";
                    MessageWhatsApp = $"Hallo *{Name}*, Data Sampling " +
                        $"{GenMessageWAItemBatch(ItemName, NoBatch)}" +
                        $"nomor permohonan *{NoRequest}* " +
                        $"pada *Tujuan Pengujian {Purposes}* telah *disetujui* oleh *{PicNames}*. ";
                    break;
                case ApplicationConstant.REJECTED_ACTION_NOTIF:
                    MessageEmail = $"<b>[Q100+]</b> Data Sampling " +
                        $"{GenMessageItemBatch(ItemName, NoBatch)}" +
                        $"nomor permohonan <b>{NoRequest}</b>. " +
                        $"pada <b>Tujuan Pengujian {Purposes}</b> telah <b>ditolak</b> oleh <b>{PicNames}</b>. " +
                        $"Silahkan cek aplikasi Q100 untuk melakukan review dan approval data pengambilan sample.";
                    MessageWhatsApp = $"Hallo *{Name}*, Data Sampling " +
                        $"{GenMessageWAItemBatch(ItemName, NoBatch)}" +
                        $"nomor permohonan *{NoRequest}* " +
                        $"pada *Tujuan Pengujian {Purposes}* telah *ditolak* oleh *{PicNames}*. " +
                        $"Silahkan cek aplikasi Q100 untuk melakukan review dan approval data pengambilan sample.";
                    break;
            }

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
        private string GenSubjectItemBatch(string itemNames, string noBatchs)
        {
            var SubResult = $"";

            if (itemNames == "" && noBatchs == "")
            {
                SubResult = $"";
            }
            else if (itemNames != "" && noBatchs == "")
            {
                SubResult = $" - {itemNames}";
            }
            else if (itemNames == "" && noBatchs != "")
            {
                SubResult = $" - {noBatchs}";
            }
            else
            {
                SubResult = $" - {itemNames}/{noBatchs}";
            }

            return SubResult;
        }

        private string GenMessageItemBatch(string itemNames, string noBatchs)
        {
            var SubResult = $"";

            if (itemNames == "" && noBatchs == "")
            {
                SubResult = $"";
            }
            else if (itemNames != "" && noBatchs == "")
            {
                SubResult = $"<b>{ItemName}</b> dengan ";
            }
            else if (itemNames == "" && noBatchs != "")
            {
                SubResult = $"no Batch <b>{NoBatch}</b> dengan ";
            }
            else
            {
                SubResult = $"<b>{ItemName}</b> no Batch <b>{NoBatch}</b> dengan ";
            }

            return SubResult;
        }

        private string GenMessageWAItemBatch(string itemNames, string noBatchs)
        {
            var SubResult = $"";

            if (itemNames == "" && noBatchs == "")
            {
                SubResult = $"";
            }
            else if (itemNames != "" && noBatchs == "")
            {
                SubResult = $"*{ItemName}* dengan ";
            }
            else if (itemNames == "" && noBatchs != "")
            {
                SubResult = $"no Batch *{NoBatch}* dengan ";
            }
            else
            {
                SubResult = $"*{ItemName}* no Batch *{NoBatch}* dengan ";
            }

            return SubResult;
        }

    }
}
