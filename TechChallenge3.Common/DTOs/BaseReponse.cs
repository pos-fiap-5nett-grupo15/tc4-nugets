using System.Text.Json.Serialization;

namespace TechChallenge3.Common.DTOs
{
    public class BaseReponse
    {
        [JsonIgnore]
        public int ErrorCode { get; set; }
        [JsonIgnore]
        public string ErrorDescription { get; set; }
    }
}
