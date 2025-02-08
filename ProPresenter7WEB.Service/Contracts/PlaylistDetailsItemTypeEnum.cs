using System.Text.Json.Serialization;

namespace ProPresenter7WEB.Service.Contracts
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PlaylistDetailsItemTypeEnum
    {
        [JsonPropertyName("presentation")]
        Presentation,

        [JsonPropertyName("header")]
        Header,
    }
}
