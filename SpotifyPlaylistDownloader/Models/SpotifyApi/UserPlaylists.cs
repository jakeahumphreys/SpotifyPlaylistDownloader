using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SpotifyPlaylistDownloader.Models.SpotifyApi;

public sealed class UserPlaylists
{
    [JsonPropertyName("items")]
    public List<Playlist> Items { get; set; }
}