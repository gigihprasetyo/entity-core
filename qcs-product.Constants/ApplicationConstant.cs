using System;
using System.Collections.Generic;

namespace qcs_product.Constants
{
    public class ApplicationConstant
    {
        public const double TIMEZONE = +7;
        public const string APPSETTING_PATH = "secrets/appsettings.json";
        public const string ALLOWED_ORIGIN_SETTING_SECTION = "AllowedOrigin";
        public const string ENVIRONMENT_SETTING_SECTION = "EnvironmentSetting";
        public const string GOOGLE_APPLICATION_CREDENTIALS = "GOOGLE_APPLICATION_CREDENTIALS";
        public const string BIOFARMA_UAM_SERVICE_SETTING_SECTION = "BiofarmaUAMServiceSetting";
        public const string BIOHR_SERVICE_SETTING_SECTION = "BioHRServiceSetting";
        public const string SUBSCRIPTION_CLIENT_NAME_SECTION = "SubscriptionClientName";
        public const string EVENT_BUS_BROKER_NAME_SECTION = "EventBusBrokerName";
        public const string EVENT_BUS_RETRY_COUNT_SECTION = "EventBusRetryCount";
        public const string DB_CONTEXT_CONECTION_STRING_SECTION = "QCProductConnection";
        public const string EVENT_BUS_CONNECTION_SECTION = "EventBusConnection";
        public const string EVENT_BUS_USERNAME_SECTION = "EventBusUserName";
        public const string EVENT_BUS_PASSWORD_SECTION = "EventBusPassword";
        public const string API_CORS_POLICY_NAME = "QCProductApiCorsPolicy";
        public const string SUCCESS_PUBLISH_DATA_MESSAGE = "Succcess publish data";
        public const string OK_MESSAGE = "ok";
        public const string NOT_OK_MESSAGE = "There are empty mandatory field that havent been filled";
        public const string ALREADY_CHECKIN_MESSAGE = "You are already checked in";
        public const string PRESENCE_DATA_NOT_FOUND = "You haven’t checked in yet";
        public const string ERROR_MESSAGE = "Error";
        public const string NO_CONTENT_MESSAGE = "Data is empty";
        public const string NO_CONTENT_ATTACHMENT = "Attachment sample is required";
        public const string END_DATE_LESS_THAN_BEGIN_DATE_ERROR_MESSAGE = "END DATE must be GREATER than BEGIN DATE";
        public const string WRONG_MASTER_STATUS_MESSAGE = "Wrong Status for List of Master";
        public const string WRONG_LABEL_TEST_SCENARIO = "Wrong Label Test Scenario";
        public const string WRONG_APPLICATION_USER_MESSAGE = "default application user is wrong, please contact q100 admin";
        public const string WRONG_APPLICATION_CODE_MESSAGE = "Wrong Application Code";
        public const string WRONG_ENDPOINT_CODE_MESSAGE = "Wrong Endpoint Code";
        public const string WRONG_ROLE_OR_MENU_CODE_MESSAGE = "Wrong Role or Menu Code";
        public const string WRONG_ROLE_CODE_MESSAGE = "Wrong Role Code";
        public const string WRONG_ROLE_OR_ENDPOINT_CODE_MESSAGE = "Wrong Role or Endpoint Code";
        public const string EXIST_ROLE_TO_MENU_MESSAGE = "Role to Menu data already exist";
        public const string EXIST_ROLE_TO_ENDPOINT_MESSAGE = "Role to Endpoint data already exist";
        public const string EXIST_POSITION_TO_ROLE_MESSAGE = "Position to Role data already exist";
        public const string WRONG_ROLE_TO_MENU_ID = "Wrong id Role to Menu";
        public const string WRONG_ROLE_TO_ENDPOINT_ID = "Wrong id for Role to Endpoint";
        public const string WRONG_POSITION_TO_ROLE_ID = "Wrong id for Position to Role";
        public const string WRONG_MENU_CODE_PARENT_MESSAGE = "Wrong Menu Code for Parent Field";
        public const string VIEW_LOG_MESSAGE = "please view log for more detail";
        public const string ERROR_REQUIRED_PAYLOAD_MSG = "is required";
        public const string ERROR_REQUIRED_PAYLOAD_FIELD = "Field";
        public const string COMMA_DELIMITER = ",";
        public const string STRIP_DELIMITER = "-";
        public const string FIRST_APPLICATION_CODE = "AC-001";
        public const string FIRST_ROLE_CODE = "AR-001";
        public const string FIRST_MENU_CODE = "AM-001";
        public const string FIRST_ENDPOINT_CODE = "AE-001";
        public const string APPLICATION_CODE_PREFIX = "AC";
        public const string ROLE_CODE_PREFIX = "AR";
        public const string MENU_CODE_PREFIX = "AM";
        public const string ENDPOINT_CODE_PREFIX = "AE";
        public const string APPLICATION_CODE_Q100 = "AC-046";
        public const string ROLE_CODE_KASIE_PEMFAS = "AR-052";
        public const string ROLE_CODE_KABAG_PEMFAS = "AR-061";
        public const string ROLE_CODE_REVIEWER_EM = "AR-063";

        public const string INTEGER_LENGTH_3 = "000";
        public const string DATE_FORMAT = "dd-MM-yyyy";
        public const string JSON_MEDIA_TYPE = "application/json";
        public const string BEARER_TOKEN_PREFIX = "Bearer";
        public const string ACCEPT_HEADER = "Accept";
        public const string CONTENT_TYPE_HEADER = "Content-Type";
        public const string GET_DB_CURRENT_TIMESTAMP_QUERY = "SELECT CURRENT_TIMESTAMP as current_timestamp";
        public const string APPLICATION_CODE_QUERYSTRING = "applicationCode";
        public const string ROLE_CODE_QUERYSTRING = "roleCode";
        public const string MENU_CODE_QUERYSTRING = "menuCode";
        public const string ID_QUERYSTRING = "id";
        public const string STATUS_QUERYSTRING = "status";
        public const string STATUS_ACTIVE = "active";
        public const string STATUS_PLANNED = "planned";
        public const string STATUS_ARCHIVED = "archived";
        public const string ENDPOINT_CODE_QUERYSTRING = "endpointCode";
        public const string POSITION_ID_QUERYSTRING = "positionId";
        public const string ROW_STATUS_DELETE = "deleted";
        public const string INSERT_OPERATION = "insert";
        public const string UPDATE_OPERATION = "update";
        public const string DELETE_OPERATION = "delete";
        public const string WRONG_MENU_SEQUENCE_MESSAGE = "Sequence already exist, please provide another";
        public const string UNAUTHORIZED_ENDPOINT = "Unauthorized Endpoint";
        public const string UNAUTHORIZED_MENU = "Unauthorized Menu";
        public const string WRONG_STATUS_CODE_REQUEST = "Wrong Status Code";
        public const string Q100_AUTHORIZATION_SETTING_SECTION = "Q100AuthorizationSetting";
        public const string NEW_ACTION_NOTIF = "New";
        public const string UPDATED_ACTION_NOTIF = "Updated";
        public const string CANCELLED_ACTION_NOTIF = "Cancelled";
        public const string APPROVED_ACTION_NOTIF = "Approved";
        public const string APPROVED_ACTION_NOTIF_TO_OPERATOR_SAMPLING = "Approved To Operator Sampling";
        public const string APPROVED_ACTION_NOTIF_RESULT_TO_QA = "Approved Result To QA";
        public const string APPROVED_ACTION_SELF_NOTIF_TO_QA = "Approved Self Notif To QA";
        public const string APPROVED_ACTION_NOTIF_RESULT_TO_OPERATOR_SAMPLING = "Approved Result To Operator Sampling";
        public const string REJECTED_ACTION_SAMPLING_NOTIF = "Reject Sampling";
        public const string REJECTED_ACTION_SELF_NOTIF_TO_QA = "Reject Self Notif To QA";
        public const string REJECTED_ACTION_TESTING_NOTIF = "Reject Testing";
        public const string EDIT_ACTION_SAMPLING_NOTIF = "Edit Operator Sampling";
        public const string EDIT_APPROVE_ACTION_SAMPLING_NOTIF = "Approve Oleh Kasie";
        public const string EDIT_APPROVE_ACTION_SAMPLING_NOTIF_TO_OPERATOR_SAMPLING = "Approve Oleh Kasie To Operator Sampling";
        public const string EDIT_APPROVE_ACTION_TESTING_NOTIF = "Approve Testing Revisi";
        public const string EDIT_ACTION_TESTING_NOTIF = "Edit Operator Sampling Testing";
        public const string REJECTED_ACTION_NOTIF = "Rejected";
        public const string REJECTED_ACTION_NOTIF_ALT = "Reject";
        public const string QC_REQUEST_MENU_NAME = "QC Request";
        public const string QC_REQUEST_ALREADY_CANCELED = "QC Request Already Canceled";
        public const string QC_SAMPLING_EMM_MENU_NAME = "Sampling EM Mikrobiologi";
        public const string QC_SAMPLING_EMPC_MENU_NAME = "Sampling EM Particle Counter";
        public const string NOTIFICATION_SECTION_SETTING = "NotificationSettingOptions";
        public const string POS_CODE_KABAG = "POS - 1";
        public const int NOTIFICATION_TYPE_EMAIL = 1;
        public const int NOTIFICATION_TYPE_WHATSAPP = 2;
        public const int NOTIFICATION_RECEIVED_TYPE_PERSONAL = 1;
        public const string EVENT_BUS_PUBSUB_PROJECT_ID_SECTION = "PubSubProjectId";
        public const string EVENT_BUS_PUBSUB_TOPIC_ID_SECTION = "PubSubTopicId";
        public const string EVENT_BUS_PUBSUB_SUBSCRIPTION_ID_SECTION = "PubSubSubscriptionId";
        public const int APPLICATION_ID = 1;
        public const int HARD_CODE_FOR_ORG_ID = 3;
        public const string GROUP_NAME_REQUEST_QCS = "request_qcs";
        public const string MESSAGE_MUST_REQUIRED = "The field must be required";
        public const string TOOLS_SAMPING_MUST_REQUIRED = "Tools sampling must be required";
        public const string PERSONAL_MUST_REQUIRED = "Personal data sampling must be required";
        public const string TOOLS_SAMPLING_MUST_REQUIRED = "Tools data sampling must be required";
        public const string MATERIAL_SAMPLING_MUST_REQUIRED = "Material data sampling must be required";
        public const string SAMPLE_MUST_REQUIRED = "Sample data sampling must be required";
        public const string MATERIALS_SAMPING_MUST_REQUIRED = "Materials sampling field must be required";
        public const string DATA_CANT_EDIT = "Data can't edit";
        public const string PAIRING_OPERATOR_NIK_CANNOT_NULL = "Please choose pairing test operator";
        public const string MESSAGE_SHIPMENT_NOT_LATE = "Shipment is not late transfer";
        public const string MESSAGE_SHIPMENT_NOT_YET_APPROVED = "Shipment is not yet approve";
        public const string MESSAGE_SHIPMENT_ALREADY_APPROVED = "Shipment is already approved";
        public const string MESSAGE_SHIPMENT_ALREADY_RECIVED = "Shipment is already recived";
        public const string MESSAGE_NOTE_REQUIRED = "Note field must be required";
        public const string MESSAGE_INVALID_SAMPLE = "Invalid Sample";
        public const string MESSAGE_NO_BATCH_ALREADY_EXIST = "Batch number is already exist";

        public const int REQUEST_TYPE_PRODUCT = 1;
        public const int REQUEST_TYPE_EM = 2;
        public const int REQUEST_TYPE_WP = 3;
        public const int REQUEST_TYPE_RM = 4;

        public const int REQUEST_SAPMLING_EMM = 15;
        public const int REQUEST_SAPMLING_PC = 16;

        public const string ENUM_KEYGROUP_REQUEST_QC = "request_type_qcs";
        public const string ENUM_KEYGROUP_SAMPLING_QC = "sampling_type_qcs";

        public const string DATA_CANNOT_SAME = "Cannot update data with same value";

        //test paramter
        public const int TEST_PARAMETER_SP = 1;
        public const int TEST_PARAMETER_AS = 2;
        public const int TEST_PARAMETER_CA = 3;
        public const int TEST_PARAMETER_FD = 4;
        public const int TEST_PARAMETER_PC05 = 5;
        public const int TEST_PARAMETER_PC50 = 6;
        public const int TEST_PARAMETER_GV = 7;

        public const int TEST_TYPE_COMPLETE = 4;

        //test paramter
        // public const string TEST_PARAMETER_LABEL_SP = 1;
        // public const string TEST_PARAMETER_LABEL_AS = 2;
        // public const string TEST_PARAMETER_LABEL_CA = 3;
        public const string TEST_PARAMETER_LABEL_FD = "Finger DAB";
        // public const string TEST_PARAMETER_LABEL_PC05 = 5;
        // public const string TEST_PARAMETER_LABEL_PC50 = 6;
        // public const string TEST_PARAMETER_LABEL_GV = 7;

        public const string TEST_SCENARIO_IN_OPERATION = "in_operation";
        public const string TEST_SCENARIO_AT_REST = "at_rest";

        public const string GCS_BUCKET_DEV_VENDOR = "q100-dev";
        public const string GCS_BUCKET_DEV = "q100-bucket-dev";
        public const string GCS_BUCKET_TESTING = "bf-q100-test-bucket";
        public const string GCS_BUCKET_SOFTLIVE = "bf-q100-softlive-bucket";

        //Status QC
        public const int STATUS_IN_REVIEW_KASIE_KABAG = 6;
        public const int STATUS_READY_TO_TRANSFER = 5;
        public const int STATUS_IN_REVIEW_QA = 4;
        public const int STATUS_IN_REVIEW_KASIE = 2;
        public const int STATUS_IN_REVIEW_KABAG = 3;
        public const int STATUS_APPROVED = 7;
        public const int STATUS_SUBMIT = 1;
        public const int STATUS_DRAFT = 0;
        public const int STATUS_CANCEL = -1;
        public const int STATUS_REJECT = -2;

        public const string QC_REQUEST_STATUS_LABEL_DRAFT = "DRAFT";
        public const string QC_REQUEST_STATUS_LABEL_EDIT = "EDIT";
        public const string QC_REQUEST_STATUS_LABEL_REJECT = "REJECT";
        public const string QC_REQUEST_STATUS_LABEL_CANCEL = "CANCEL";
        public const string QC_REQUEST_STATUS_LABEL_APPROVE = "APPROVE";
        public const string QC_REQUEST_STATUS_LABEL_COMPLETED = "COMPLETED";

        //Status Shipment
        public const int STATUS_SHIPMENT_NOTHING = 0;
        public const int STATUS_SHIPMENT_SENDING = 1;
        public const int STATUS_SHIPMENT_INTRANSIT = 2;
        public const int STATUS_SHIPMENT_RECEIVED = 3;
        public const int STATUS_SHIPMENT_LATE_SAMPLE = 4;
        public const int STATUS_SHIPMENT_LATE_REVIEWED = 5;
        public const int STATUS_SHIPMENT_LATE_RECIVED = 6;

        //BIOHRSetting
        public const string BASEURL_BIOHR = "https://apps.biofarma.co.id/BiofarmaService/";
        public const string APP_CODE_BIOHR = "ESS";

        //BIOHR Parameter
        public const string POSITION_TYPE_KABAG = "04";
        public const string POSITION_TYPE_KASIE = "07";
        public const string POSITION_NAME_KABAG = "Kepala Bagian";
        public const string POSITION_NAME_KASIE = "Kepala Seksi";

        //Type Sending Tracker
        public const string TRACKER_TYPE_SEND = "SEND";
        public const string TRACKER_TYPE_RECEIVE = "RECEIVE";
        public const string FAILED_INSERT_MESSAGE = "Failed publish data";
        public const string FAILED_QRCODE_NOT_FOUND_MESSAGE = "QRCode Not Found";
        public const string FAILED_PACKAGE_NOT_YET_RECIVED_MESSAGE = "Package has been sent and has not been received";
        public const string FAILED_PACKAGE_NOT_YET_SEND_MESSAGE = "Package has not been sent";
        public const string FAILED_PACKAGE_ALREADY_RECIVED_MESSAGE = "Package has been received";
        public const string FAILED_PACKAGE_SEND_DATE_GREATER_THAN_RECEIVE_DATE = "Receive date of package {0} must greater than send date";
        public const string FAILED_PACKAGE_RECEIVE_MESSAGE = "Failed receive package {0} on {1}";
        public const string SUCCESS_PACKAGE_ALREADY_SEND_MESSAGE = "Package sent successfully";
        public const string SUCCESS_PACKAGE_ALREADY_RECIVED_MESSAGE = "Package received successfully";


        public const string STATUS_SUCCESS_STR = "Success";
        public const string STATUS_FAILED_STR = "Failed";

        //Status QC Test
        public const int STATUS_TEST_DRAFT = 0;
        public const int STATUS_TEST_READYTOTEST = 1;
        public const int STATUS_TEST_INPROGRESS = 2;
        public const int STATUS_TEST_INREVIEW_PAIRING = 3;
        public const int STATUS_TEST_INREVIEW_AHLI_MUDA_QC = 4;
        public const int STATUS_TEST_INREVIEW_KABAG_QC = 5;
        public const int STATUS_TEST_INREVIEW_KABAG_PRODUKSI = 6;
        public const int STATUS_TEST_INREVIEW_QA = 7;
        public const int STATUS_TEST_APPROVED = 8;
        public const int STATUS_TEST_REJECTED = -1;
        public const string APPLICATION_NAME = "Quality Control System Product Service API V1";
        public const string HEALTH_CHECK_PATH = "/healthz";
        public const string SWAGGER_PATH = "/qcs_product/swagger/v1/swagger.json";
        public const string SWAGGER_ROUTE_TEMPLATE = "qcs_product/swagger/{documentName}/swagger.json";
        public const string SWAGGER_ROUTE_PREFIX = "qcs_product/swagger";
        public const string ENDPOINT_FORMAT = "qcs_product/v1/[controller]/[action]";

        //Workflow Setting
        public const string APP_CODE = "QCS";
        public const string WORKFLOW_CODE_SAMPLING_PHASE_1 = "QCS-1";
        public const string WORKFLOW_CODE_SAMPLING_PHASE_2 = "QCS-5";
        public const string WORKFLOW_CODE_TESTING_PHASE_1 = "QCS-3";
        public const string WORKFLOW_CODE_TESTING_PHASE_2 = "QCS-6";
        public const string WORKFLOW_ACTION_SUBMIT_NAME = "Submit";
        public const string WORKFLOW_ACTION_SUBMIT_REJECT_NAME = "Submit Reject";
        public const string WORKFLOW_ACTION_EDIT_NAME = "Edit";
        public const string WORKFLOW_ACTION_EDIT_REJECT_KABAG_NAME = "Edit KABAG";
        public const string WORKFLOW_ACTION_EDIT_REJECT_QA_NAME = "Edit QA";
        public const string WORKFLOW_ACTION_APPROVE_NAME = "Approve";
        public const string WORKFLOW_ACTION_REJECT_NAME = "Reject";
        public const string WORKFLOW_ACTION_REJECT_COMPLETE_NAME = "Reject Complete";
        public const string WORKFLOW_ACTION_REJECT_KABAG_NAME = "Reject by KABAG";
        public const string WORKFLOW_ACTION_REJECT_QA_NAME = "Reject by QA";

        public const string WORKFLOW_STATUS_COMPLETE_NAME = "Complete";

        public const string PREFIX_EM_M_TESTING_WORKFLOW_CODE = "EMMT";
        public const string PREFIX_EM_PC_WORKFLOW_CODE = "EMP";
        public const string PREFIX_EM_M_WORKFLOW_CODE = "EMM";
        public const int SAMPLING_TYPE_ID_EMM = 15;
        public const int SAMPLING_TYPE_ID_EMP = 16;

        public const int TEST_TYPE_ID = 17;
        public const string WORKFLOW_INITIAL_DESC = "Approval Qc Sampling Phase 1";

        public const string WRONG_DIGITAL_SIGNATURE = "Incorrect Digital Signature";
        public const string NOTES_TO_LONG_MESSAGE = "Maximum 200 Characters";
        public const string EMPTY_PIC = "PIC for next phase is empty. Please Contact Admin";
        public const string WORKFLOW_STATUS_NAME_REVIEW_DRAFT = "Draft";
        public const string WORKFLOW_STATUS_NAME_REVIEW_KASIE = "Review Kasie";
        public const string WORKFLOW_STATUS_NAME_REVIEW_KABAG = "Review Kabag";
        public const string WORKFLOW_STATUS_NAME_REVIEW_QA = "Review QA";
        public const string WORKFLOW_STATUS_NAME_COMPLETE = "Complete";
        public const string WORKFLOW_STATUS_NAME_REVIEW_PAIRING = "Review Pairing";
        public const string WORKFLOW_STATUS_NAME_REVIEW_AHLI_MUDA = "Review Ahli Muda";
        public const string WORKFLOW_STATUS_NAME_REVIEW_KABAG_QC = "Review Kabag QC";

        // fase proses untuk list monitoring
        public const int PROCESS_STATUS_PHASE_REQUEST = 1;
        public const int PROCESS_STATUS_PHASE_SAMPLING = 2;
        public const int PROCESS_STATUS_PHASE_TRANSFER = 3;
        public const int PROCESS_STATUS_PHASE_TESTING = 4;
        public const int PROCESS_STATUS_PHASE_RESULT = 5;

        public const int APPROVAL_DATA_TYPE_SAMPLING = 1;
        public const int APPROVAL_DATA_TYPE_TESTING = 2;

        public const string TEST_VARIABLE_ALERT = "Alert";
        public const string TEST_VARIABLE_ACTION_LIMIT = "Action Limit";
        public const string TEST_VARIABLE_SPESIFICATION = "Specification";
        public const string WORKFLOW_FAILED_DEV_NUMBER_MESSAGE = "Deviation number is required";
        public const string WORKFLOW_SUCCESS_APPROVE_MESSAGE = "Data has been approved";
        public const string WORKFLOW_SUCCESS_REJECT_MESSAGE = "Data has been rejected";
        public const string WORKFLOW_STATUS_PENDING = "Pending";
        public const string MESSAGE_EDIT_DEVIASI_ALREADY_EXIST = "Deviation number is already exist";
        public const string MESSAGE_EDIT_DEVIASI_SUCCESS = "Deviation Number has been updated";
        public const string MESSAGE_SAVE_NOTE_SUCCESS = "Notes has been saved";
        public const string MESSAGE_EDIT_DEVIASI_NOT_SUCCESS = "Can't Update Deviation Number, Please Contact Admin";
        public const string MESSAGE_EDIT_DEVIASI_DUPLICATE = "Deviation number is already exist";


        public const string MESSAGE_EDIT_CONC_SUCCESS = "Conclusion has been updated";
        public const string MESSAGE_EDIT_CONC_NOT_SUCCESS = "Can't Update Conclusion, Please Contact Admin";
        public const string GENERAL_ERROR = "Error occured, please contact admin";

        public const int TEST_SCENARIO_TYPE_IN_OPERATIONS = 1;
        public const int TEST_SCENARIO_TYPE_AT_REST = 2;


        public const int TEST_SCENARIO_CODE_UNDEFINED = 9;
        public const int TEST_SCENARIO_CODE_IN_OPERATIONS = 1;
        public const int TEST_SCENARIO_CODE_AT_REST = 0;

        public const string TEST_SCENARIO_LABEL_IN_OPERATIONS = "in_operation";
        public const string TEST_SCENARIO_LABEL_AT_REST = "at_rest";

        public const string TEST_SCENARIO_LABELALT_IN_OPERATIONS = "In Operation";
        public const string TEST_SCENARIO_LABELALT_AT_REST = "At Rest";

        public const string REQUEST_LOCATION_QC = "qc";

        //TODO rename supaya lebih informatif
        public const int ERROR_CODE_400 = 400;

        public const int PROCESS_INKUBASI = 4;
        public const int PROCESS_OBSERVASI = 5;
        public const int PROCESS_UJI_IDENTIFIKASI = 6;

        public const int THRESHOLD_EQUAL = 1; /* = */
        public const int THRESHOLD_GREATER_THAN = 2; /* > */
        public const int THRESHOLD_LESS_THAN = 3; /* < */
        public const int THRESHOLD_GREATER_THAN_OR_EQUAL = 4; /* >= */
        public const int THRESHOLD_LESS_THAN_OR_EQUAL = 5; /* <= */
        public const int THRESHOLD_IN_BETTWEEN = 6; /* <> */
        public const int THRESHOLD_IN_BETTWEEN_OR_EQUAL = 7; /* <=> */
        public const string QS_SAMPLING_STATUS_LABEL_APPROVE = "APPROVE";
        public const string QS_SAMPLING_STATUS_LABEL_REJECT = "REJECT";
        public const string QS_SAMPLING_STATUS_LABEL_DRAFT = "DRAFT";
        public const string QS_SAMPLING_STATUS_LABEL_SUBMIT = "SUBMIT";
        public const string QS_SAMPLING_STATUS_LABEL_EDIT = "EDIT";
        public const string QS_SAMPLING_STATUS_LABEL_SEND = "SEND";
        public const string QS_SAMPLING_STATUS_LABEL_RECEIVE = "RECEIVE";
        public const string QS_SAMPLING_STATUS_LABEL_REVIEW = "REVIEW";
        public const string QS_SAMPLING_STATUS_LABEL_LATE_SAMPLE = "LATE TRANSFER";
        public const string QS_SAMPLING_STATUS_LABEL_LATE_REVIEWED = "LATE REVIEW";
        public const string QS_SAMPLING_STATUS_LABEL_LATE_RECIVED = "LATE RECIVE";
        public const string QS_SAMPLING_STATUS_LABEL_CANCEL = "CANCEL";

        public const string QC_TEST_STATUS_LABEL_CREATE = "CREATE";
        public const string QC_TEST_STATUS_LABEL_DRAFT = "DRAFT";
        public const string QC_TEST_STATUS_LABEL_EDIT = "READY TO TEST";
        public const string QC_TEST_STATUS_LABEL_START = "START TEST";
        public const string QC_TEST_STATUS_LABEL_SUBMIT = "SUBMIT";
        public const string QC_TEST_STATUS_LABEL_INPROGRESS = "IN PROGRESS";
        public const string QC_TEST_STATUS_LABEL_APPROVE = "APPROVE";
        public const string QC_TEST_STATUS_LABEL_REJECT = "REJECT";

        public const string MODULE_NAME_QC_REQUEST = "REQ";
        public const string MODULE_NAME_QC_REQUEST_TEST_TYPE = "REQ-TPE";
        public const string MODULE_NAME_QC_SAMPLING = "SAM";
        public const string MODULE_NAME_QC_SAMPLING_SAMPLE = "SAM-SPL";
        public const string MODULE_NAME_QC_SAMPLING_MATERIAL = "SAM-MTR";
        public const string MODULE_NAME_QC_SAMPLING_TOOL = "SAM-TOL";
        public const string MODULE_NAME_QC_TRANSFER = "TRA";
        public const string MODULE_NAME_QC_TEST = "TES";
        public const string MODULE_NAME_QC_TEST_MATERIAL = "TES-MTR";
        public const string MODULE_NAME_QC_TEST_TOOL = "TES-TOL";
        public const string MODULE_NAME_QC_TEST_PARAMATER = "TES-PAR";
        public const string MODULE_NAME_QC_TEST_PROCEDURE = "TES-PCD";
        public const string MODULE_NAME_QC_TEST_SAMPLE = "TES-SPL";
        public const string MODULE_NAME_QC_TEST_RESULT_GROUP = "TES-RSG";
        public const string MODULE_NAME_QC_TEST_RESULT = "TES-RST";
        public const string MODULE_NAME_QC_TEST_PROCESS = "TES-PCS";
        public const string MODUL_NAME_ORGANIZATION = "Organization";



        //Tool Activity Const
        public const int TOOL_ACTIVITY_CALIBRATION = 3;
        public const int TOOL_ACTIVITY_QUALIFICATION = 4;
        public const int TOOL_ACTIVITY_VALIDATION = 5;
        public const string TOOL_ACTIVITY_CALIBRATION_LABEL = "Calibration";
        public const string TOOL_ACTIVITY_VALIDATION_LABEL = "Validation";
        public const string MAX_DATETIME = "12/31/9999 23:59:59";
        public const string APP_CODE_LOGIN = "QAS";
        public const string TEST_VARIABLE_CONCLUSION_PASS = "PASS";

        //Conclusion Label
        public const string LABEL_CONCLUSION_PASS = "PASS";
        public const string LABEL_CONCLUSION_OOAEM = "OOA-EM";
        public const string LABEL_CONCLUSION_OOAEM_WARNING = "WARNING";
        public const string LABEL_CONCLUSION_OEM = "OEM";
        public const string LABEL_CONCLUSION_OEM_FAIL = "FAIL";
        public const string LABEL_CONCLUSION_STRIPE = "-";

        //Conclusion Messages
        public const string MSG_CONCLUSION_PASS = "Memenuhi Syarat";
        public const string MSG_CONCLUSION_NOT_PASS = "Tidak Memenuhi Syarat";

        public const int PURPOSE_ID_KLASIFIKASI = 1;
        public const int PURPOSE_ID_RE_KLASIFIKASI = 2;
        public const int PURPOSE_ID_UJI_RUTIN = 3;
        public const int PURPOSE_ID_UJI_BULANAN = 4;
        public const int PURPOSE_ID_KUALIFIKASI = 5;
        public const int PURPOSE_ID_SWAB = 6;
        public const int PURPOSE_ID_VALIDASI_ASEPTIS = 10;
        public const int PURPOSE_ID_KLASIFIKASI_VALIDASI_ASEPTIS = 11;

        //QcProcess Section Form
        public const int SECTION_TYPE_ID_TOOL = 1;
        public const int SECTION_TYPE_ID_MATERIAL = 2;
        public const int SECTION_TYPE_ID_PROCEDURE = 3;
        public const int SECTION_TYPE_ID_NOTE = 4;
        //Notifikasi 
        public const string NOTIFICATION_MESSAGE_WHATSAPP_RIJECT = "{0} Hallo {1}, Terdapat Data {2} dengan nomor {3} {4} yang ditolak oleh Kabag. Silahkan cek aplikasi Q100+ untuk melakukan perbaikan terhadap data {5}.";

        //notifikasi yang di terima kabag approve by kasie em-m
        //subject : [Q100+] Sampling [Nomor Permohonan] – [Nama Sediaan]/[No Batch] has been updated
        //body : Terdapat Data Sampling [Nama sediaan] nomor batch [No Batch] dengan nomor permohonan [Nomor Permohonan] pada tujuan pengujian [Tujuan Pengujian] telah diperbaharui oleh [nama role Operator Sampling pada Workflow History] dan telah disetujui oleh [nama role Kasie Pemilik Fasilitas yang disesuaikan dengan Workflow History]. Silakan cek aplikasi Q100+ untuk melakukan review.

        //notifikasi yang diterima QA approve by kabag
        //subject : [Q100+] Sampling [Nomor Permohonan] – [Nama Sediaan]/[No Batch] has been approved
        //body : Terdapat Data Sampling [Nama sediaan] nomor batch [No Batch] dengan nomor permohonan [Nomor Permohonan] pada tujuan pengujian [Tujuan Pengujian] sudah disetujui oleh [Nama role yang sesuai dengan Workflow History untuk Kabag Pemilik Fasilitas]. Silakan cek aplikasi Q100+ untuk melihat data tersebut.

        // notifikasi email 
        /* 
         (sampling)
         KABAG - appproved */
        public const string NOTIFICATION_EMAIL_SUBJECT_SAMPLING_KABAG_APPROVED = "[Q100+] Data {0} – {1}/{2} has been approved";
        // public const string NOTIFICATION_EMAIL_BODY_SAMPLING_KABAG_APPROVED = "Terdapat nomor permohonan {0} dengan nomor batch {1} sudah disetujui oleh {2}. Silakan cek aplikasi Q100+ untuk melihat data tersebut.";
        public const string NOTIFICATION_EMAIL_BODY_SAMPLING_KABAG_APPROVED = "<h4>[Q100-Sampling-Info]</h4> " +
                                                                            "" +
                                                                            "<p>Hallo <b>{0}</b>,</p> " +
                                                                            "<p>Mohon untuk <b>me-<i>review</i></b> data <b>SAMPLING</b> berikut:</p> " +
                                                                            "<p>Nomor Batch        : <b>{1}</b>,</p> " +
                                                                            "<p>Nomor Permohonan   : <b>{2}</b>,</p> " +
                                                                            "<p>Tujuan Pengujian   : <b>{3}</b>.</p> " +
                                                                            "" +
                                                                            "<p>Silahkan cek aplikasi Q100+ untuk melihat data tersebut. Terima kasih.</p>";

        // KABAG - reject
        public const string NOTIFICATION_EMAIL_SUBJECT_SAMPLING_KABAG_REJECT = "[Q100+] Sampling {0} – {1}/{2} has been rejected";
        // public const string NOTIFICATION_EMAIL_BODY_SAMPLING_KABAG_REJECT = "Terdapat Data Sampling {0} nomor batch {1} dengan nomor permohonan {2} pada tujuan pengujian {3} ditolak oleh {4}. Silakan cek aplikasi Q100+ untuk melakukan perbaikan terhadap data tersebut.";
        public const string NOTIFICATION_EMAIL_BODY_SAMPLING_KABAG_REJECT = "<h4>[Q100-Sampling-Info]</h4> " +
                                                                            "" +
                                                                            "<p>Hallo <b>{0}</b>,</p> " +
                                                                            "<p>telah <b>di-<i>tolak</i></b> oleh <b>{1}</b></p> " +
                                                                            "<p>pada data <b>SAMPLING</b> berikut:</p> " +
                                                                            "<p>Nomor Batch        : <b>{2}</b>,</p> " +
                                                                            "<p>Nomor Permohonan   : <b>{3}</b>,</p> " +
                                                                            "<p>Tujuan Pengujian   : <b>{4}</b>.</p> " +
                                                                            "" +
                                                                            "<p>Silakan cek aplikasi Q100+ untuk melihat data tersebut. Terima kasih.</p>";

        // KABAG - appproved
        public const string NOTIFICATION_EMAIL_SUBJECT_TESTING_KABAG_REJECT = "[Q100+] Data Uji {0} – {1} has been rejected";
        // public const string NOTIFICATION_EMAIL_BODY_TESTING_KABAG_REJECT = "Terdapat Data Uji {0} dengan ID Testing {1} sudah ditolak oleh {2}. Silakan cek aplikasi Q100+ untuk melakukan perbaikan terhadap data tersebut.";
        public const string NOTIFICATION_EMAIL_BODY_TESTING_KABAG_REJECT = "<h4>[Q100-Testing-Info]</h4> " +
                                                                            "" +
                                                                            "<p>Hallo <b>{0}</b>,</p> " +
                                                                            "<p>telah <b>di-<i>tolak</i></b> oleh <b>{1}</b></p> " +
                                                                            "<p>pada data <b>TESTING</b> berikut:</p> " +
                                                                            "<p>Nomor Batch        : <b>{2}</b>,</p> " +
                                                                            "<p>Nomor Permohonan   : <b>{3}</b>,</p> " +
                                                                            "<p>Nomor Pengujian    : <b>{4}</b>,</p> " +
                                                                            "<p>Tujuan Pengujian   : <b>{5}</b>.</p> " +
                                                                            "" +
                                                                            "<p>Silakan cek aplikasi Q100+ untuk melihat data tersebut. Terima kasih.</p>";

        //notifikasi whatsapp
        /*
        (SAMPLING)
        KABAG*/
        // public const string NOTIFICATION_WHATSAPP_SAMPLING_KABAG_APPROVE = "Hallo *{0}*, terdapat nomor permohonan *{1}* dengan nomor batch *{2}* sudah *disetujui* oleh {3}. Silakan cek aplikasi Q100+ untuk melihat data tersebut.";
        public const string NOTIFICATION_WHATSAPP_SAMPLING_KABAG_APPROVE = "*[Q100-Sampling-Info]*, Hallo *{0}*, mohon untuk *me-* *_review_* data *SAMPLING* berikut, Nomor Batch: *{1}*, Nomor Permohonan : *{2}*, Tujuan Pengujian : *{3}*. Silahkan cek aplikasi Q100+ untuk melihat data tersebut. Terima kasih.";
        // public const string NOTIFICATION_WHATSAPP_SAMPLING_KABAG_REJECT = "Hallo *{0}*, terdapat *Data Sampling {1}* nomor batch *{2}* dengan nomor permohonan *{3}* pada tujuan pengujian *{4} ditolak* oleh {5}. Silakan cek aplikasi Q100+ untuk melakukan perbaikan terhadap data tersebut.";
        public const string NOTIFICATION_WHATSAPP_SAMPLING_KABAG_REJECT = "*[Q100-Sampling-Info]*, Hallo *{0}*, telah *di-* *_tolak_* oleh *{1}*, pada data *SAMPLING* berikut, Nomor Batch : *{2}*, Nomor Permohonan : *{3}*, Tujuan Pengujian : *{4}*. Silakan cek aplikasi Q100+ untuk melihat data tersebut. Terima kasih.";

        // public const string NOTIFICATION_WHATSAPP_TESTING_KABAG_REJECT = "Hallo *{0}*, terdapat *Data Uji {1}* dengan ID Testing *{2}* sudah *ditolak* oleh {3}. Silakan cek aplikasi Q100+ untuk melakukan perbaikan terhadap data tersebut.";
        public const string NOTIFICATION_WHATSAPP_TESTING_KABAG_REJECT = "*[Q100-Testing-Info]*, Hallo *{0}*, telah *di-* *_tolak_* oleh *{1}*, pada data *TESTING* berikut, Nomor Batch : *{2}*, Nomor Permohonan : *{3}*, Nomor Pengujian : *{4}*, Tujuan Pengujian : *{5}*. Silakan cek aplikasi Q100+ untuk melihat data tersebut. Terima kasih.";

        /*  motifikasi email 
            operator sampling perbaikan data.
            (sampling)
            */
        public const string NOTIFICATION_EMAIL_SUBJECT_SAMPLING_EDIT_OPERATOR_SAMPLING = "[Q100+] Sampling {0} – {1}/{2} has been updated";
        // public const string NOTIFICATION_EMAIL_BODY_SAMPLING_EDIT_OPERATOR_SAMPLING = "Terdapat Data Sampling {0} nomor batch {1} dengan nomor permohonan {2} pada tujuan pengujian {3} telah diperbaharui. Silakan cek aplikasi Q100+ untuk melakukan review.";
        public const string NOTIFICATION_EMAIL_BODY_SAMPLING_EDIT_OPERATOR_SAMPLING = "<h4>[Q100-Sampling-Info]</h4> " +
                                                                                    "" +
                                                                                    "<p>Hallo <b>{0}</b>,</p> " +
                                                                                    "<p>telah <b>di-<i>perbarui</i></b> oleh <b>{1}</b></p> " +
                                                                                    "<p>pada data <b>SAMPLING</b> berikut:</p> " +
                                                                                    "<p>Nomor Batch        : <b>{2}</b>,</p> " +
                                                                                    "<p>Nomor Permohonan   : <b>{3}</b>,</p> " +
                                                                                    "<p>Tujuan Pengujian   : <b>{4}</b>.</p> " +
                                                                                    "" +
                                                                                    "<p>Silakan cek aplikasi Q100+ untuk melihat data tersebut. Terima kasih.</p>";
        // public const string NOTIFICATION_EMAIL_BODY_SAMPLING_EDIT_APPROVE = "Terdapat Data Sampling {0} nomor batch {1} dengan nomor permohonan {2} pada tujuan pengujian {3} telah diperbaharui oleh {4} dan telah disetujui oleh {5}. Silakan cek aplikasi Q100+ untuk melakukan review";
        public const string NOTIFICATION_EMAIL_BODY_SAMPLING_EDIT_APPROVE = "<h4>[Q100-Sampling-Reminder]</h4> " +
                                                                            "" +
                                                                            "<p>Hallo <b>{0}</b>,</p> " +
                                                                            "<p>Mohon untuk <b>me-<i>review</i></b> data <b>SAMPLING</b> berikut:</p> " +
                                                                            "<p>Nomor Batch        : <b>{1}</b>,</p> " +
                                                                            "<p>Nomor Permohonan   : <b>{2}</b>,</p> " +
                                                                            "<p>Tujuan Pengujian   : <b>{3}</b>.</p> " +
                                                                            "" +
                                                                            "<p>Silahkan cek aplikasi Q100+ untuk melihat data tersebut. Terima kasih.</p>";
        public const string NOTIFICATION_EMAIL_BODY_OPERATOR_SAMPLING_APPROVED = "<h4>[Q100-Sampling-Info]</h4> " +
                                                                                "" +
                                                                                "<p>Hallo <b>{0}</b>,</p> " +
                                                                                "<p>telah <b>disetujui</b> oleh <b>{1}</b></p> " +
                                                                                "<p>pada data <b>SAMPLING</b> berikut:</p> " +
                                                                                "<p>Nomor Batch        : <b>{2}</b>,</p> " +
                                                                                "<p>Nomor Permohonan   : <b>{3}</b>,</p> " +
                                                                                "<p>Tujuan Pengujian   : <b>{4}</b>.</p> " +
                                                                                "" +
                                                                                "<p>Silakan cek aplikasi Q100+ untuk melihat data tersebut. Terima kasih.</p>";
        public const string NOTIFICATION_EMAIL_BODY_SAMPLING_KASIE_REJECT = "<h4>[Q100-Sampling-Info]</h4> " +
                                                                                        "" +
                                                                                        "<p>Hallo <b>{0}</b>,</p> " +
                                                                                        "<p>telah <b>ditolak</b> oleh <b>{1}</b></p> " +
                                                                                        "<p>pada data <b>SAMPLING</b> berikut:</p> " +
                                                                                        "<p>Nomor Batch        : <b>{2}</b>,</p> " +
                                                                                        "<p>Nomor Permohonan   : <b>{3}</b>,</p> " +
                                                                                        "<p>Tujuan Pengujian   : <b>{4}</b>.</p> " +
                                                                                        "" +
                                                                                        "<p>Silakan cek aplikasi Q100+ untuk melakukan <b><i>perbaikan</i></b> pada data tersebut. Terima kasih.</p>";
        public const string NOTIFICATION_EMAIL_BODY_SAMPLING_EDIT_APPROVE_OPERATOR_SAMPLING = "<h4>[Q100-Sampling-Info]</h4> " +
                                                                                            "" +
                                                                                            "<p>Hallo <b>{0}</b>,</p> " +
                                                                                            "<p>telah <b>disetujui<b> oleh <b>{1}</b></p> " +
                                                                                            "<p>pada data <b>SAMPLING</b> berikut:</p> " +
                                                                                            "<p>Nomor Batch        : <b>{2}</b>,</p> " +
                                                                                            "<p>Nomor Permohonan   : <b>{3}</b>,</p> " +
                                                                                            "<p>Tujuan Pengujian   : <b>{4}</b>.</p> " +
                                                                                            "" +
                                                                                            "<p>Silakan cek aplikasi Q100+ untuk melihat data tersebut. Terima kasih.</p>";

        /*
            operator sampling perbaikan data. 
            (testing)
        */
        public const string NOTIFICATION_EMAIL_SUBJECT_TESTING_EDIT_OPERATOR_SAMPLING = "[Q100+] Data Uji {0} – {1} has been updated";
        // public const string NOTIFICATION_EMAIL_BODY_TESTING_EDIT_OPERATOR_SAMPLING = "Terdapat Data Uji {0} dengan ID Testing {1} yang telah diperbaharui oleh {2}. Silakan cek aplikasi Q100+ untuk melakukan review.";
        public const string NOTIFICATION_EMAIL_BODY_TESTING_EDIT_OPERATOR_SAMPLING = "<h4>[Q100-Testing-Info]</h4> " +
                                                                                    "" +
                                                                                    "<p>Hallo <b>{0}</b>,</p> " +
                                                                                    "<p>telah <b>di-<i>perbarui</i></b> oleh <b>{1}</b></p> " +
                                                                                    "<p>pada data <b>TESTING</b> berikut:</p> " +
                                                                                    "<p>Nomor Batch        : <b>{2}</b>,</p> " +
                                                                                    "<p>Nomor Permohonan   : <b>{3}</b>,</p> " +
                                                                                    "<p>Nomor Pengujian   : <b>{4}</b>,</p> " +
                                                                                    "<p>Tujuan Pengujian   : <b>{5}</b>.</p> " +
                                                                                    "" +
                                                                                    "<p>Silakan cek aplikasi Q100+ untuk melihat data tersebut. Terima kasih.</p>";
        // public const string NOTIFICATION_WHATSAPP_SAMPLING_EDIT_OPERATOR_SAMPLING = "Hallo *{0}*, terdapat *Data Sampling {1}* nomor batch *{2}* dengan nomor permohonan *{3}* pada tujuan pengujian *{4} telah diperbaharui*. Silakan cek aplikasi Q100+ untuk melakukan review.";
        public const string NOTIFICATION_WHATSAPP_SAMPLING_EDIT_OPERATOR_SAMPLING = "*[Q100-Sampling-Info]*, Hallo *{0}*, telah *di-* *_perbarui_* oleh *{1}* pada data *SAMPLING* berikut, Nomor Batch : *{2}*, Nomor Permohonan : *{3}*, Tujuan Pengujian : *{4}*. Silakan cek aplikasi Q100+ untuk melihat data tersebut. Terima kasih.";

        // public const string NOTIFICATION_WHATSAPP_TESTING_EDIT_OPERATOR_SAMPLING = "Hallo *{0}*, terdapat *Data Uji {1}* dengan ID Testing *{2}* yang *telah diperbaharui* oleh {3}. Silakan cek aplikasi Q100+ untuk melakukan review.";
        public const string NOTIFICATION_WHATSAPP_TESTING_EDIT_OPERATOR_SAMPLING = "*[Q100-Testing-Info]*, Hallo *{0}*, telah *di-* *_perbarui_* oleh *{1}* pada data *TESTING* berikut, Nomor Batch : *{2}*, Nomor Permohonan : *{3}*, Nomor Pengujian : *{4}*, Tujuan Pengujian : *{5}*. Silakan cek aplikasi Q100+ untuk melihat data tersebut. Terima kasih.";
        public const string NOTIFICATION_WHATSAPP_SAMPLING_KASIE_REJECT = "*[Q100-Sampling-Info]*, Hallo *{0}*, telah *ditolak* oleh *{1}* pada data *SAMPLING* berikut, Nomor Batch : *{2}*, Nomor Permohonan : *{3}*, Tujuan Pengujian : *{4}*. Silakan cek aplikasi Q100+ untuk melakukan *_perbaikan_* pada data tersebut. Terima kasih.";


        // public const string NOTIFICATION_WHATSAPP_SAMPLING_EDIT_KASIE_APPROVE = "Hallo *{0}*, terdapat *Data Sampling {1}* nomor batch *{2}* dengan nomor permohonan *{3}* pada tujuan pengujian *{4} telah diperbaharui* oleh {5} dan telah disetujui oleh {6}. Silakan cek aplikasi Q100+ untuk melakukan review.";
        public const string NOTIFICATION_WHATSAPP_SAMPLING_EDIT_KASIE_APPROVE = "*[Q100-Sampling-Reminder]*, Hallo *{0}*, mohon untuk *me-* *_review_* data *SAMPLING* berikut, Nomor Batch: *{1}*, Nomor Permohonan : *{2}*, Tujuan Pengujian : *{3}*. Silahkan cek aplikasi Q100+ untuk melihat data tersebut. Terima kasih.";

        //[Q100+] Data Uji[Jenis Uji] – [ID Testing] has been updated
        // public const string NOTIFICATION_EMAIL_BODY_TESTING_EDIT_APPROVE = "Terdapat Data Sampling {0} nomor batch {1} dengan nomor permohonan {2} pada tujuan pengujian {3} telah diperbaharui oleh {4} dan telah disetujui oleh {5}. Silakan cek aplikasi Q100+ untuk melakukan review.";
        public const string NOTIFICATION_EMAIL_BODY_TESTING_EDIT_APPROVE = "<h4>[Q100-Sampling-Reminder]</h4> " +
                                                                            "" +
                                                                            "<p>Hallo <b>{0}</b>,</p> " +
                                                                            "<p>Mohon untuk <b>me-<i>review</i></b> data <b>SAMPLING</b> berikut:</p> " +
                                                                            "<p>Nomor Batch        : <b>{1}</b>,</p> " +
                                                                            "<p>Nomor Permohonan   : <b>{2}</b>,</p> " +
                                                                            "<p>Tujuan Pengujian   : <b>{3}</b>.</p> " +
                                                                            "" +
                                                                            "<p>Silahkan cek aplikasi Q100+ untuk melihat data tersebut. Terima kasih.</p>";
        // public const string NOTIFICATION_WHATSAPP_TESTING_EDIT_APPROVE = "Hallo *{0}*, terdapat *Data Sampling {1}* nomor batch *{2}* dengan nomor permohonan *{3}* pada tujuan pengujian *{4} telah diperbaharui* oleh {5} dan telah disetujui oleh {6}. Silakan cek aplikasi Q100+ untuk melakukan review.";
        public const string NOTIFICATION_WHATSAPP_TESTING_EDIT_APPROVE = "*[Q100-Sampling-Reminder]*, Hallo *{0}*, mohon untuk *me-* *_review_* data *SAMPLING* berikut, Nomor Batch: *{1}*, Nomor Permohonan : *{2}*, Nomor Pengujian : *{3}*, Tujuan Pengujian : *{4}*. Silahkan cek aplikasi Q100+ untuk melihat data tersebut. Terima kasih.";
        public const string NOTIFICATION_WHATSAPP_OPERATOR_SAMPLING_APPROVED = "*[Q100-Sampling-Info]*, Hallo *{0}*, telah *disetujui* oleh *{1}* pada data *SAMPLING* berikut, Nomor Batch : *{2}*, Nomor Permohonan : *{3}*, Tujuan Pengujian : *{4}*. Silakan cek aplikasi Q100+ untuk melihat data tersebut. Terima kasih.";
        public const string NOTIFICATION_WHATSAPP_SAMPLING_EDIT_KASIE_APPROVE_OPERATOR_SAMPLING = "*[Q100-Sampling-Info]*, Hallo *{0}*, telah *disetujui* oleh *{1}* pada data *SAMPLING* berikut, Nomor Batch : *{2}*, Nomor Permohonan : *{3}*, Tujuan Pengujian : *{4}*. Silakan cek aplikasi Q100+ untuk melihat data tersebut. Terima kasih.";

        public const string NOTIFICATION_EMAIL_BODY_RESULT_APPROVED_TO_QA = "<h4>[Q100-Data Pengujian-EM-Info]</h4> " +
                                                                                        "" +
                                                                                        "<p>Hallo <b>{0}</b>,</p> " +
                                                                                        "<p>Mohon untuk <b>me-<i>review</i></b> Data Pengujian EM berikut:</p> " +
                                                                                        "<p>Nomor Batch        : <b>{1}</b>,</p> " +
                                                                                        "<p>Nomor Permohonan   : <b>{2}</b>,</p> " +
                                                                                        "<p>Nomor Pengujian    : <b>{3}</b>,</p> " +
                                                                                        "<p>Tujuan Pengujian   : <b>{4}</b>.</p> " +
                                                                                        "" +
                                                                                        "<p>Silahkan cek aplikasi Q100+ untuk melihat data tersebut. Terima kasih.</p>";
        public const string NOTIFICATION_WHATSAPP_RESULT_APPROVED_TO_QA = "*[Q100-Data Pengujian-EM-Info]*, Hallo *{0}*, mohon untuk *me-* *_review_* Data Pengujian EM berikut, Nomor Batch: *{1}*, Nomor Permohonan : *{2}*, Nomor Pengujian : *{3}*, Tujuan Pengujian : *{4}*. Silahkan cek aplikasi Q100+ untuk melihat data tersebut. Terima kasih.";

        public const string NOTIFICATION_EMAIL_BODY_RESULT_APPROVED_TO_OPERATOR_SAMPLING = "<h4>[Q100-Sampling-Info]</h4> " +
                                                                                        "" +
                                                                                        "<p>Hallo <b>{0}</b>,</p> " +
                                                                                        "<p>telah <b>disetujui</b> oleh <b>{1}</b></p> " +
                                                                                        "<p>Pada data <b>SAMPLING</b> berikut:</p> " +
                                                                                        "<p>Nomor Batch        : <b>{2}</b>,</p> " +
                                                                                        "<p>Nomor Permohonan   : <b>{3}</b>,</p> " +
                                                                                        "<p>Nomor Pengujian    : <b>{4}</b>,</p> " +
                                                                                        "<p>Tujuan Pengujian   : <b>{5}</b>.</p> " +
                                                                                        "" +
                                                                                        "<p>Silakan cek aplikasi Q100+ untuk melihat data tersebut. Terima kasih.</p>";
        public const string NOTIFICATION_WHATSAPP_RESULT_APPROVED_TO_OPERATOR_SAMPLING = "*[Q100-Sampling-Info]*, Hallo *{0}*, telah *disetujui* oleh *{1}*, pada data *SAMPLING* berikut, Nomor Batch : *{2}*, Nomor Permohonan : *{3}*, Nomor Pengujian : *{4}*, Tujuan Pengujian : *{5}*. Silakan cek aplikasi Q100+ untuk melihat data tersebut. Terima kasih.";


        /** Self Notification Approved/Reject To QA */
        public const string EMAIL_SUBJECT_SELF_NOTIFICATION_APPROVE_TO_QA = "[Q100+] Data {0} – {1}/{2} has been approved";
        public const string EMAIL_BODY_SELF_NOTIFICATION_APPROVED_TO_QA = "<h4>[Q100-Data Pengujian EM-Info]</h4> " +
                                                                        "<p>Hallo <b>{0}</b>,</p> " +
                                                                        "<p>telah <b>disetujui</b></p>" +
                                                                        "<p>pada Data Pengujian EM berikut :</p> " +
                                                                        "<p>Nomor Batch        : <b>{1}</b>,</p> " +
                                                                        "<p>Nomor Permohonan   : <b>{2}</b>,</p> " +
                                                                        "<p>Nomor Pengujian   : <b>{4}</b>,</p> " +
                                                                        "<p>Tujuan Pengujian   : <b>{3}</b>.</p> " +
                                                                        "" +
                                                                        "<p>Silahkan cek aplikasi Q100+ untuk melihat data tersebut. Terima kasih.</p>";
        public const string WHATSAPP_SELF_NOTIFICATION_APPROVED_TO_QA = "*[Q100-Data Pengujian EM-Info]*, Hallo *{0}*, telah *disetujui* pada Data Pengujian EM berikut: Nomor Batch : *{2}*, Nomor Permohonan : *{1}* , Nomor Pengujian : *{4}* , Tujuan Pengujian : *{3}*. Silahkan cek aplikasi Q100+ untuk melihat data tersebut.";

        public const string EMAIL_SUBJECT_SELF_NOTIFICATION_REJECT_TO_QA = "[Q100+] Data {0} – {1}/{2} has been rejected";
        public const string EMAIL_BODY_SELF_NOTIFICATION_REJECTED_TO_QA = "<h4>[Q100-Data Pengujian EM-Info]</h4> " +
                                                                        "<p>Hallo <b>{0}</b>,</p> " +
                                                                        "<p>telah <b>ditolak</b></p> " +
                                                                        "<p>pada data Data Pengujian EM berikut :</p> " +
                                                                        "<p>Nomor Batch        : <b>{1}</b>,</p> " +
                                                                        "<p>Nomor Permohonan   : <b>{2}</b>,</p> " +
                                                                        "<p>Nomor Pengujian    : <b>{4}</b>,</p> " +
                                                                        "<p>Tujuan Pengujian   : <b>{3}</b>.</p> " +
                                                                        "" +
                                                                        "<p>Silahkan cek aplikasi Q100+ untuk melihat data tersebut. Terima kasih.</p>";
        public const string WHATSAPP_SELF_NOTIFICATION_REJECTED_TO_QA = "*[Q100-Data Pengujian EM-Info]*, Hallo *{0}*, telah *ditolak* pada data SAMPLING berikut : Nomor Batch : *{2}*, Nomor Permohonan : *{1}* Nomor Pengujian : *{4}* Tujuan Pengujian : *{3}*. Silahkan cek aplikasi Q100+ untuk melihat data tersebut.";

        //spesial case parameter
        public const string ASC_ORDER = "asc";
        public const string DESC_ORDER = "desc";
        public const string ASC_IN_REVIEW_AHLI_MUDA_UJI = "asc_in_riview_ahli_muda_uji";
        public const string ASC_IN_REVIEW_KABAG_UJI = "asc_in_riview_kabag_uji";
        public const string DESC_IN_REVIEW_AHLI_MUDA_UJI = "desc_in_riview_ahli_muda_uji";
        public const string DESC_IN_REVIEW_KABAG_UJI = "desc_in_riview_kabag_uji";


        //QcProcess Type Sample Form
        public const int ADD_SAMPLE_TEST = 1;
        public const int ADD_SAMPLE_BY_BATCH_TEST = 2;

        //Threshold sending late shipment
        public const double THRESHOLD_SHIPMENT_2X24 = 48;


        public const int MSG_TYPE_NEXT_PIC = 1;
        public const int MSG_TYPE_BEFORE_PIC = 2;

        //item group label
        public const string MEDIA = "MEDIA";
        public const string PRODUCT = "PRODUCT";
        public const string RAW_MATERIAL = "RAW_MATERIAL";
        public const string M_SAMP = "M-SAMP";
        public const string M_CA = "M-CA";
        public const string M_TSA = "M-TSA";

        //tool code group
        public const string TOOL_GROUP_CODE_AHU = "TF-010";

        public const string GEDUNG_16_CODE = "GD16";
        public const string GILANG_NADIA_NEWNIK = "09851538";
        public const string DEVELOPMENT_ENVIRONMENT_NAME = "development";
        public const string TESTING_ENVIRONMENT_NAME = "testing";
        public const string STAGING_ENVIRONMENT_NAME = "staging";
        public const string SOFTLIVE_ENVIRONMENT_NAME = "softlive";

        //product method
        public const int PRODUCT_METHOD_ID_ASEPTIC = 1;
        public const int PRODUCT_METHOD_ID_NON_ASEPTIC = 0;
        public const string PRODUCT_METHOD_NAME_ASEPTIC = "aseptik";
        public const string PRODUCT_METHOD_NAME_NON_ASEPTIC = "non-aseptik";

        //master data object status
        public const int OBJECT_STATUS_ACTIVE = 3;
        public const string OBJECT_STATUS_ACTIVE_NAME = "ACTIVE";
        //urutan sample AS > CA > SP > FD > Gloves
        public const string CODE_PARAMTER_UJI_AS = "AS";
        public const string CODE_PARAMTER_UJI_CA = "CA";
        public const string CODE_PARAMTER_UJI_SP = "SP";
        public const string CODE_PARAMTER_UJI_FD = "FD";
        public const string CODE_PARAMTER_UJI_GLOVE = "GL";

        public const int PURPOSE_BR_ID = 3;

        //history action
        public const string HISTORY_ADD_ACTION = "ADD";
        public const string HISTORY_EDIT_ACTION = "EDIT";
        public const string HISTORY_DELETE_ACTION = "DELETE";
    }
}

