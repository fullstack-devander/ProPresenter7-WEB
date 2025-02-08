using System.Text.Json.Serialization;

namespace ProPresenter7WEB.Service.Contracts
{
    public class Id
    {
        [JsonPropertyName("uuid")]
        public required string Uuid { get; set; }

        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("index")]
        public required int Index { get; set; }
    }
}
