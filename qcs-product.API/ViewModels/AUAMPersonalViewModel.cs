using System.Text.Json.Serialization;

namespace qcs_product.API.ViewModels
{
    
    public class AUAMResponse<T>
    {
        [JsonPropertyName("data")]
        public T Data { get; set; }
        
        [JsonPropertyName("statusCode")]
        public int StatusCode { get; set; } 
        
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
    
    public class AUAMPersonalViewModel
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("newUserId")]
        public string Nik { get; set; }
        [JsonPropertyName("oldNIK")]
        public string OldNik { get; set; }
        [JsonPropertyName("newNIK")]
        public string NewNik { get; set; }

        [JsonPropertyName("positionName")]
        public string PositionName { get; set; }
        
        [JsonPropertyName("email")]
        public string Email { get; set; }

    }
    public class AUAMPersonalExtViewModel
    {

        [JsonPropertyName("nik")]
        public string Nik { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("noTelp")]
        public string NoTelp { get; set; }

        [JsonPropertyName("initial")]
        public string Initial { get; set; }

        [JsonPropertyName("organizationId")]
        public int OrganizationId { get; set; }

        [JsonPropertyName("bioHROrganizationId")]
        public int BioHROrganizationId { get; set; }

        [JsonPropertyName("positionId")]
        public string PositionId { get; set; }
    }


}