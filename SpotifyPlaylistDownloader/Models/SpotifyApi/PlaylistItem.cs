using System.Text.Json.Serialization;

namespace SpotifyPlaylistDownloader.Models.SpotifyApi;

public class PlaylistItem
{
    [JsonPropertyName("track")]
    public PlaylistTrack Track { get; set; }
}