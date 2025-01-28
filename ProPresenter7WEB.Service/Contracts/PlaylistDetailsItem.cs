using System.Text.Json.Serialization;

namespace ProPresenter7WEB.Service.Contracts
{
    public class PlaylistDetailsItem
    {
        [JsonPropertyName("id")]
        public required Id Id { get; set; }

        [JsonPropertyName("type")]
        public PlaylistDetailsItemTypeEnum Type { get; set; }
    }
}
