using System.Text.Json.Serialization;

namespace SpotifyPlaylistDownloader.Models.SpotifyApi;

public class PlaylistTrack
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("url")]
    public string Url { get; set; }
    [JsonPropertyName("external_urls")]
    public ExternalUrls ExternalUrls { get; set; }
}