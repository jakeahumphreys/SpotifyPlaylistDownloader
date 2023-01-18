using System.Text.Json.Serialization;

namespace SpotifyPlaylistDownloader.Models.SpotifyApi;

public class ExternalUrls
{
    [JsonPropertyName("spotify")]
    public string Spotify { get; set; }
}