using System.Text.Json.Serialization;

namespace ProPresenter7WEB.Service.Contracts
{
    public class Playlist
    {
        [JsonPropertyName("id")]
        public required Id Id { get; set; }

        [JsonPropertyName("field_type")]
        public PlaylistFieldTypeEnum FieldType { get; set; }

        [JsonPropertyName("children")]
        public required Playlist[] Children { get; set; }
    }
}
