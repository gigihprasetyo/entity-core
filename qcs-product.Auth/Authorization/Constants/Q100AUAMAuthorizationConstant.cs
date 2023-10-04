namespace qcs_product.Auth.Authorization.Constants
{
    /// <summary>
    /// application constant
    /// </summary>
    public class Q100AUAMAuthorizationConstant
    {
        public const string OK_MESSAGE = "ok";
        public const string USER_POSITION_ID_ATTRIBUTE = "PositionId";
        public const string USER_NAME_ATTRIBUTE = "Name";
        public const string USER_ORGANIZATION_ATTRIBUTE = "OrganizationId";
        public const string USER_NIK_ATTRIBUTE = "NIK";
        public const string USER_EMAIL_ATTRIBUTE = "Email";
        public const string UNVALID_TOKEN = "expired or wrong token";
        public const string GET_DB_CURRENT_TIMESTAMP_QUERY = "SELECT CURRENT_TIMESTAMP as current_timestamp";
        public const string INSERT_OPERATION = "insert";
        public const string UPDATE_OPERATION = "update";
        public const string DELETE_OPERATION = "delete";
        public const string EMPTY_STRING = "";
        public const string BEARER = "Bearer ";
        public const string AUTHORIZATION_HEADER = "Authorization";
        public const string AUTH_STATUS_HEADER = "AuthStatus";
        public const string AUTHORIZED = "Authorized";
        public const string NOT_AUTHORIZED = "Not Authorized";
        public const string PLEASE_PROVIDE_AUTHORIZATION = "Please Provide Authorization";
        public const string UNAUTHORIZED_ENDPOINT_MESSAGE = "Unauthorized Endpoint";
        public const string INVALID_TOKEN_MESSAGE = "Invalid Token";
    }
}
