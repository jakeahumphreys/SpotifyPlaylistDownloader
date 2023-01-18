using Newtonsoft.Json;

namespace SpotifyPlaylistDownloader.Models.Authentication;

public class SpotifyApiCredentials
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string SpotifyUserId { get; set; }
}