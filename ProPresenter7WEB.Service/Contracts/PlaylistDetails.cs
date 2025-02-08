using System.Text.Json.Serialization;

namespace ProPresenter7WEB.Service.Contracts
{
    public class PlaylistDetails
    {
        [JsonPropertyName("id")]
        public required Id Id { get; set; }

        [JsonPropertyName("items")]
        public required PlaylistDetailsItem[] Items { get; set; }
    }
}
