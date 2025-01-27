using System.Text.Json.Serialization;

namespace ProPresenter7WEB.Service.Contracts
{
    public class VersionInfo
    {
        [JsonPropertyName("api_version")]
        public required string ApiVersion { get; set; }

        [JsonPropertyName("host_description")]
        public required string HostDescription { get; set; }

        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("os_version")]
        public required string OsVersion { get; set; }

        [JsonPropertyName("platform")]
        public required string Platform { get; set; }
    }
}
