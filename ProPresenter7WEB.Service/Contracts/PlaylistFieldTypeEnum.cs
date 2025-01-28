using System.Text.Json.Serialization;

namespace ProPresenter7WEB.Service.Contracts
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PlaylistFieldTypeEnum
    {
        [JsonPropertyName("playlist")]
        Playlist,

        [JsonPropertyName("group")]
        Group,
    }
}
