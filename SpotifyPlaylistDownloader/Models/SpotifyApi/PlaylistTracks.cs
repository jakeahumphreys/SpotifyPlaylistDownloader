using System.Text.Json.Serialization;

namespace SpotifyPlaylistDownloader.Models.SpotifyApi;

public class PlaylistTracks
{
    [JsonPropertyName("items")]
    public PlaylistItem[] Items { get; set; }
}