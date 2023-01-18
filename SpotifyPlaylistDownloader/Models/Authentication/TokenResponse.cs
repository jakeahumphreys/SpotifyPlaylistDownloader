using System.Text.Json.Serialization;

namespace SpotifyPlaylistDownloader.Models.Authentication;

public class TokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
}