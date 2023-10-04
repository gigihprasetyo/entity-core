using qcs_product.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class MessageNotificationMonitoringViewModel
    {
        public string Subject { get; set; }
        public string Name { get; set; }
        // public string PicRole { get; set; }
        public string PicName { get; set; }
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
        public string TestType { get; set; }
        public string TestCode { get; set; }
        // public string SendPicRole { get; set; }
        public string SendPicName { get; set; }

        public MessageNotificationMonitoringViewModel(string noRequest, string noBatch, string itemName, string purpose, string emailAddress, string status, string name, string noHandphone, string picName, string testType, string testCode, string editor)
        {
            NoRequest = noRequest;
            NoBatch = noBatch;
            ItemName = itemName;
            Purposes = purpose;
            EmailAddress = emailAddress;
            Status = status;
            Name = name;
            NoHandphone = noHandphone;
            // PicRole = picRole;
            PicName = picName;
            TestCode = testCode;
            TestType = testType;
            // SendPicRole = editor;
            SendPicName = editor;
            Subject = GenerateSubject();
            GenerateMessageEmail();
        }

        private string GenerateSubject()
        {
            switch (Status)
            {
                case ApplicationConstant.APPROVED_ACTION_NOTIF:
                    Subject = String.Format(ApplicationConstant.NOTIFICATION_EMAIL_SUBJECT_SAMPLING_KABAG_APPROVED, NoRequest, ItemName, NoBatch);
                    break;
                case ApplicationConstant.APPROVED_ACTION_NOTIF_TO_OPERATOR_SAMPLING:
                    Subject = String.Format(ApplicationConstant.NOTIFICATION_EMAIL_SUBJECT_SAMPLING_KABAG_APPROVED, NoRequest, ItemName, NoBatch);
                    break;
                case ApplicationConstant.REJECTED_ACTION_SAMPLING_NOTIF:
                    Subject = String.Format(ApplicationConstant.NOTIFICATION_EMAIL_SUBJECT_SAMPLING_KABAG_REJECT, NoRequest, ItemName, NoBatch);
                    break;
                case ApplicationConstant.REJECTED_ACTION_TESTING_NOTIF:
                    Subject = String.Format(ApplicationConstant.NOTIFICATION_EMAIL_SUBJECT_TESTING_KABAG_REJECT, TestType, TestCode);
                    break;
                case ApplicationConstant.EDIT_ACTION_SAMPLING_NOTIF:
                    Subject = String.Format(ApplicationConstant.NOTIFICATION_EMAIL_SUBJECT_SAMPLING_EDIT_OPERATOR_SAMPLING, NoRequest, ItemName, NoBatch);
                    break;
                case ApplicationConstant.EDIT_ACTION_TESTING_NOTIF:
                    Subject = String.Format(ApplicationConstant.NOTIFICATION_EMAIL_SUBJECT_TESTING_EDIT_OPERATOR_SAMPLING, TestType, TestCode);
                    break;
                // case ApplicationConstant.EDIT_APPROVE_ACTION_SAMPLING_NOTIF:
                //     Subject = String.Format(ApplicationConstant.NOTIFICATION_EMAIL_SUBJECT_SAMPLING_EDIT_OPERATOR_SAMPLING, NoRequest, ItemName, NoBatch);
                //     break;
                // case ApplicationConstant.EDIT_APPROVE_ACTION_TESTING_NOTIF:
                //     Subject = String.Format(ApplicationConstant.NOTIFICATION_EMAIL_SUBJECT_TESTING_EDIT_OPERATOR_SAMPLING, NoRequest, ItemName, NoBatch);
                //     break;
                case ApplicationConstant.APPROVED_ACTION_NOTIF_RESULT_TO_QA:
                    Subject = String.Format(ApplicationConstant.NOTIFICATION_EMAIL_SUBJECT_SAMPLING_KABAG_APPROVED, NoRequest, ItemName, NoBatch);
                    break;
                case ApplicationConstant.APPROVED_ACTION_NOTIF_RESULT_TO_OPERATOR_SAMPLING:
                    Subject = String.Format(ApplicationConstant.NOTIFICATION_EMAIL_SUBJECT_SAMPLING_KABAG_APPROVED, NoRequest, ItemName, NoBatch);
                    break;
                case ApplicationConstant.APPROVED_ACTION_SELF_NOTIF_TO_QA:
                    Subject = String.Format(ApplicationConstant.EMAIL_SUBJECT_SELF_NOTIFICATION_APPROVE_TO_QA, NoRequest, ItemName, NoBatch);
                    break;
                case ApplicationConstant.REJECTED_ACTION_SELF_NOTIF_TO_QA:
                    Subject = String.Format(ApplicationConstant.EMAIL_SUBJECT_SELF_NOTIFICATION_REJECT_TO_QA, NoRequest, ItemName, NoBatch);
                    break;
            }

            return Subject;
        }

        private void GenerateMessageEmail()
        {
            switch (Status)
            {
                case ApplicationConstant.APPROVED_ACTION_NOTIF:
                    // MessageEmail = string.Format(ApplicationConstant.NOTIFICATION_EMAIL_BODY_SAMPLING_KABAG_APPROVED, NoRequest, NoBatch, PicRole);
                    // MessageWhatsApp = string.Format(ApplicationConstant.NOTIFICATION_WHATSAPP_SAMPLING_KABAG_APPROVE, Name, NoRequest, NoBatch, PicRole);

                    MessageEmail = string.Format(ApplicationConstant.NOTIFICATION_EMAIL_BODY_SAMPLING_KABAG_APPROVED, Name, NoBatch, NoRequest, Purposes);
                    MessageWhatsApp = string.Format(ApplicationConstant.NOTIFICATION_WHATSAPP_SAMPLING_KABAG_APPROVE, Name, NoBatch, NoRequest, Purposes);
                    break;
                case ApplicationConstant.APPROVED_ACTION_NOTIF_TO_OPERATOR_SAMPLING:
                    MessageEmail = string.Format(ApplicationConstant.NOTIFICATION_EMAIL_BODY_OPERATOR_SAMPLING_APPROVED, Name, PicName, NoBatch, NoRequest, Purposes);
                    MessageWhatsApp = string.Format(ApplicationConstant.NOTIFICATION_WHATSAPP_OPERATOR_SAMPLING_APPROVED, Name, PicName, NoBatch, NoRequest, Purposes);
                    break;
                case ApplicationConstant.REJECTED_ACTION_SAMPLING_NOTIF:
                    // MessageEmail = string.Format(ApplicationConstant.NOTIFICATION_EMAIL_BODY_SAMPLING_KABAG_REJECT, ItemName, NoBatch, NoRequest, Purposes, PicName);
                    // MessageWhatsApp = string.Format(ApplicationConstant.NOTIFICATION_WHATSAPP_SAMPLING_KABAG_REJECT, Name, ItemName, NoBatch, NoRequest, Purposes, PicName);

                    MessageEmail = string.Format(ApplicationConstant.NOTIFICATION_EMAIL_BODY_SAMPLING_KABAG_REJECT, Name, PicName, NoBatch, NoRequest, Purposes);
                    MessageWhatsApp = string.Format(ApplicationConstant.NOTIFICATION_WHATSAPP_SAMPLING_KABAG_REJECT, Name, PicName, NoBatch, NoRequest, Purposes);
                    break;
                case ApplicationConstant.REJECTED_ACTION_TESTING_NOTIF:
                    MessageEmail = string.Format(ApplicationConstant.NOTIFICATION_EMAIL_BODY_TESTING_KABAG_REJECT, TestType, TestCode, PicName);
                    MessageWhatsApp = string.Format(ApplicationConstant.NOTIFICATION_WHATSAPP_TESTING_KABAG_REJECT, Name, TestType, TestCode, PicName);
                    break;
                case ApplicationConstant.EDIT_ACTION_SAMPLING_NOTIF:
                    // MessageEmail = string.Format(ApplicationConstant.NOTIFICATION_EMAIL_BODY_SAMPLING_EDIT_OPERATOR_SAMPLING, ItemName, NoBatch, NoRequest, Purposes);
                    // MessageWhatsApp = string.Format(ApplicationConstant.NOTIFICATION_WHATSAPP_SAMPLING_EDIT_OPERATOR_SAMPLING, Name, ItemName, NoBatch, NoRequest, Purposes);

                    MessageEmail = string.Format(ApplicationConstant.NOTIFICATION_EMAIL_BODY_SAMPLING_EDIT_OPERATOR_SAMPLING, Name, PicName, NoBatch, NoRequest, Purposes);
                    MessageWhatsApp = string.Format(ApplicationConstant.NOTIFICATION_WHATSAPP_SAMPLING_EDIT_OPERATOR_SAMPLING, Name, PicName, NoBatch, NoRequest, Purposes);
                    break;
                case ApplicationConstant.EDIT_ACTION_TESTING_NOTIF:
                    MessageEmail = string.Format(ApplicationConstant.NOTIFICATION_EMAIL_BODY_TESTING_EDIT_OPERATOR_SAMPLING, TestType, TestCode, PicName);
                    MessageWhatsApp = string.Format(ApplicationConstant.NOTIFICATION_WHATSAPP_TESTING_EDIT_OPERATOR_SAMPLING, Name, TestType, TestCode, PicName);
                    break;
                // case ApplicationConstant.EDIT_APPROVE_ACTION_SAMPLING_NOTIF:
                //     // MessageEmail = string.Format(ApplicationConstant.NOTIFICATION_EMAIL_BODY_SAMPLING_EDIT_APPROVE, ItemName, NoBatch, NoRequest, Purposes, SendPicName, PicName);
                //     // MessageWhatsApp = string.Format(ApplicationConstant.NOTIFICATION_WHATSAPP_SAMPLING_EDIT_KASIE_APPROVE, Name, ItemName, NoBatch, NoRequest, Purposes, SendPicName, PicName);
                //     MessageEmail = string.Format(ApplicationConstant.NOTIFICATION_EMAIL_BODY_SAMPLING_EDIT_APPROVE, Name, NoBatch, NoRequest, Purposes);
                //     MessageWhatsApp = string.Format(ApplicationConstant.NOTIFICATION_WHATSAPP_SAMPLING_EDIT_KASIE_APPROVE, Name, NoBatch, NoRequest, Purposes);
                //     break;
                // case ApplicationConstant.EDIT_APPROVE_ACTION_TESTING_NOTIF:
                //     MessageEmail = string.Format(ApplicationConstant.NOTIFICATION_EMAIL_BODY_TESTING_EDIT_APPROVE, ItemName, NoBatch, NoRequest, Purposes, SendPicName, PicName);
                //     MessageWhatsApp = string.Format(ApplicationConstant.NOTIFICATION_WHATSAPP_TESTING_EDIT_APPROVE, Name, ItemName, NoBatch, NoRequest, Purposes, SendPicName, PicName);
                //     break;
                case ApplicationConstant.APPROVED_ACTION_NOTIF_RESULT_TO_QA:
                    MessageEmail = string.Format(ApplicationConstant.NOTIFICATION_EMAIL_BODY_RESULT_APPROVED_TO_QA, Name, NoBatch, NoRequest, TestCode, Purposes);
                    MessageWhatsApp = string.Format(ApplicationConstant.NOTIFICATION_WHATSAPP_RESULT_APPROVED_TO_QA, Name, NoBatch, NoRequest, TestCode, Purposes);
                    break;
                case ApplicationConstant.APPROVED_ACTION_NOTIF_RESULT_TO_OPERATOR_SAMPLING:
                    MessageEmail = string.Format(ApplicationConstant.NOTIFICATION_EMAIL_BODY_RESULT_APPROVED_TO_OPERATOR_SAMPLING, Name, PicName, NoBatch, NoRequest, TestCode, Purposes);
                    MessageWhatsApp = string.Format(ApplicationConstant.NOTIFICATION_WHATSAPP_RESULT_APPROVED_TO_OPERATOR_SAMPLING, Name, PicName, NoBatch, NoRequest, TestCode, Purposes);
                    break;
                case ApplicationConstant.APPROVED_ACTION_SELF_NOTIF_TO_QA:
                    MessageEmail = string.Format(ApplicationConstant.EMAIL_BODY_SELF_NOTIFICATION_APPROVED_TO_QA, Name, NoBatch, NoRequest, Purposes, TestCode);
                    MessageWhatsApp = string.Format(ApplicationConstant.WHATSAPP_SELF_NOTIFICATION_APPROVED_TO_QA, Name, NoRequest, NoBatch, Purposes, TestCode);
                    break;
                case ApplicationConstant.REJECTED_ACTION_SELF_NOTIF_TO_QA:
                    MessageEmail = string.Format(ApplicationConstant.EMAIL_BODY_SELF_NOTIFICATION_REJECTED_TO_QA, Name, NoBatch, NoRequest, Purposes, TestCode);
                    MessageWhatsApp = string.Format(ApplicationConstant.WHATSAPP_SELF_NOTIFICATION_REJECTED_TO_QA, Name, NoRequest, NoBatch, Purposes, TestCode);
                    break;
            }

        }

    }
}
