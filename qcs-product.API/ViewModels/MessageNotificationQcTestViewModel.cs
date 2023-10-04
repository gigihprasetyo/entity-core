using qcs_product.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class MessageNotificationQcTestViewModel
    {
        public string Subject { get; set; }
        public string Name { get; set; }
        public string PicNames { get; set; }
        public string EmailAddress { get; set; }
        public string NoHandphone { get; set; }
        public string MessageEmail { get; set; }
        public string MessageWhatsApp { get; set; }
        public string NoTests { get; set; }
        public string QcProcessNames { get; set; }
        public string Status { get; set; }
        public string MenuName { get; set; }
        public string NoBatch { get; set; }
        public string NoRequest { get; set; }
        public string Purposes { get; set; }

        public MessageNotificationQcTestViewModel(string NoTest, string QcProcessName, string emailAddress, string status, string name, string noHandphone, string PicName, int type_msg = ApplicationConstant.MSG_TYPE_NEXT_PIC, string noBatch = null, string noRequest = null, string purposes = null)
        {
            NoTests = NoTest;
            QcProcessNames = QcProcessName;
            EmailAddress = emailAddress;
            Status = status;
            Name = name;
            NoHandphone = noHandphone;
            PicNames = PicName;
            NoBatch = noBatch;
            NoRequest = noRequest;
            Purposes = purposes;
            Subject = GenerateSubject();
            GenerateMessageEmail(type_msg);
        }

        private string GenerateSubject()
        {
            switch (Status)
            {
                case ApplicationConstant.NEW_ACTION_NOTIF:
                    Subject = $"{Status} Data Uji {QcProcessNames} - {NoTests}";
                    break;
                case ApplicationConstant.UPDATED_ACTION_NOTIF:
                    Subject = $"Data Uji {QcProcessNames} - {NoTests} {Status}";
                    break;
                case ApplicationConstant.CANCELLED_ACTION_NOTIF:
                    Subject = $"{Status} Data Uji {QcProcessNames} - {NoTests}";
                    break;
                case ApplicationConstant.APPROVED_ACTION_NOTIF:
                    Subject = $"Data Uji {QcProcessNames} - {NoTests} has been {Status}";
                    break;
                case ApplicationConstant.REJECTED_ACTION_NOTIF:
                    Subject = $"[Q100+] Data Uji {QcProcessNames} - {NoTests} has been {Status}";
                    break;
            }

            Subject = $"Data Uji {QcProcessNames} - {NoTests}";

            return Subject;
        }

        private void GenerateMessageEmail(int type_msg = ApplicationConstant.MSG_TYPE_NEXT_PIC)
        {
            switch (Status)
            {
                case ApplicationConstant.NEW_ACTION_NOTIF:
                    MessageEmail = $"Terdapat Data Uji baru <b>{QcProcessNames}</b> dengan <b>ID Testing {NoTests}</b>." +
                        (type_msg == ApplicationConstant.MSG_TYPE_NEXT_PIC ?
                            $"Silahkan cek aplikasi Q100+ untuk melakukan review dan approval data hasil uji." :
                            $"Silahkan cek aplikasi Q100+ untuk melakukan proses review."
                         );
                    MessageWhatsApp = $"Hallo *{Name}*, terdapat Data Uji baru *{QcProcessNames}* dengan *ID Testing {NoTests}*." +
                        (type_msg == ApplicationConstant.MSG_TYPE_NEXT_PIC ?
                            $"Silahkan cek aplikasi Q100+ untuk melakukan review dan approval data hasil uji." :
                            $"Silahkan cek aplikasi Q100+ untuk melakukan proses review."
                         );
                    break;

                case ApplicationConstant.UPDATED_ACTION_NOTIF:
                    MessageEmail = $"Data Uji <b>{QcProcessNames}</b> dengan <b>ID Testing {NoTests}</b> telah <b>diperbaharui</b>." +
                        (type_msg == ApplicationConstant.MSG_TYPE_NEXT_PIC ?
                            $"Silahkan cek aplikasi Q100+ untuk melakukan review dan approval data hasil uji." :
                            $"Silahkan cek aplikasi Q100+ untuk melakukan proses review."
                         );
                    MessageWhatsApp = $"Hallo *{Name}*, Data Uji *{QcProcessNames}* dengan *ID Testing {NoTests}* telah *diperbaharui*." +
                        (type_msg == ApplicationConstant.MSG_TYPE_NEXT_PIC ?
                            $"Silahkan cek aplikasi Q100+ untuk melakukan review dan approval data hasil uji." :
                            $"Silahkan cek aplikasi Q100+ untuk melakukan proses review."
                         );
                    break;

                case ApplicationConstant.CANCELLED_ACTION_NOTIF:
                    MessageEmail = $"Data Uji <b>{QcProcessNames}</b> dengan <b>ID Testing {NoTests}</b> telah <b>dibatalkan</b>.";
                    MessageWhatsApp = $"Hallo *{Name}*, Data Uji *{QcProcessNames}* dengan *ID Testing {NoTests}* telah *dibatalkan*. ";
                    break;

                case ApplicationConstant.APPROVED_ACTION_NOTIF:
                    // MessageEmail = $"Data Uji <b>{QcProcessNames}</b> dengan <b>ID Testing {NoTests}</b> telah tersedia telah <b>disetujui</b> oleh <b>{PicNames}</b>." +
                    //     (type_msg == ApplicationConstant.MSG_TYPE_NEXT_PIC ?
                    //         $"Silahkan cek aplikasi Q100+ untuk melakukan review dan approval data hasil uji." :
                    //         $"Silahkan cek aplikasi Q100+ untuk melakukan proses review."
                    //      );
                    // MessageWhatsApp = $"Hallo *{Name}*, Data Uji untuk *ID Testing {NoTests}* dengan pengujian *{QcProcessNames}* telah *disetujui* oleh *{PicNames}*. " +
                    //     (type_msg == ApplicationConstant.MSG_TYPE_NEXT_PIC ?
                    //         $"Silahkan cek aplikasi Q100+ untuk melakukan review dan approval data hasil uji." :
                    //         $"Silahkan cek aplikasi Q100+ untuk melakukan proses review."
                    //      );
                    MessageEmail = $"<h4>[Q100-Testing-Info]</h4> " +
                                $"" +
                                $"<p>Hallo <b>{Name}</b>,</p> " +
                                $"<p>Mohon untuk <b>me-<i>review</i></b> data <b>TESTING</b> berikut:</p> " +
                                $"<p>Nomor Permohonan   : <b>{NoRequest}</b>,</p> " +
                                $"<p>Nomor Pengujian   : <b>{NoTests}</b>,</p> " +
                                $"" +
                                $"<p>Silahkan cek aplikasi Q100+ untuk melihat data tersebut. Terima kasih.</p>";
                    MessageWhatsApp = $"*[Q100-Testing-Info]*, Hallo *{Name}*, mohon untuk *me-* *_review_* data *TESTING* berikut, Nomor Permohonan: *{NoRequest}*, Nomor Pengujian: *{NoTests}*. Silahkan cek aplikasi Q100+ untuk melihat data tersebut. Terima kasih.";
                    break;

                case ApplicationConstant.REJECTED_ACTION_NOTIF:
                    // MessageEmail = $"Data Uji <b>{QcProcessNames}</b> dengan <b>ID Testing {NoTests}</b> telah tersedia telah <b>ditolak</b> oleh <b>{PicNames}</b>." +
                    //     (type_msg == ApplicationConstant.MSG_TYPE_NEXT_PIC ?
                    //         $"Silahkan cek aplikasi Q100+ untuk melakukan review dan approval data hasil uji." :
                    //         $"Silahkan cek aplikasi Q100+ untuk melakukan proses review."
                    //      );
                    // MessageWhatsApp = $"*[Q100+]* Hallo *{Name}*, Data Uji untuk *ID Testing {NoTests}* dengan pengujian *{QcProcessNames}* telah *ditolak* oleh *{PicNames}*. " +
                    //     (type_msg == ApplicationConstant.MSG_TYPE_NEXT_PIC ?
                    //         $"Silahkan cek aplikasi Q100+ untuk melakukan review dan approval data hasil uji." :
                    //         $"Silahkan cek aplikasi Q100+ untuk melakukan proses review."
                    //      );
                    MessageEmail = $"<h4>[Q100-Testing-Info]</h4> " +
                                $"<p>Hallo <b>{Name}</b>,</p> " +
                                $"<p>Data uji untuk ID Testing: <b>'{NoTests}'</b> dengan pengujian *{QcProcessNames}*</p>" + 
                                $"<p>Telah <b>di-<i>tolak</i></b> oleh <b>{PicNames}</b>.</p> " +
                                $"<p>Silakan cek aplikasi Q100+ untuk melakukan proses correction.</p>";
                    MessageWhatsApp = $"*[Q100-Testing-Info]*, Hallo *{Name}*, Data uji untuk ID Testing: *'{NoTests}'* dengan pengujian *{QcProcessNames}* Telah ditolak oleh *{PicNames}*. Silakan cek aplikasi Q100+ untuk melakukan proses correction.";
                    break;
            }

        }

    }
}
