using System.Text.Json.Serialization;

namespace SpotifyPlaylistDownloader.Models.SpotifyApi;

public sealed class Playlist
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
}