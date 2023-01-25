using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using SpotifyPlaylistDownloader.Models.Authentication;
using SpotifyPlaylistDownloader.Models.Common;
using SpotifyPlaylistDownloader.Models.SpotifyApi;

namespace SpotifyPlaylistDownloader;

public class SpotifyClient
{
    public async Task<List<Playlist>> GetUserPlaylists(HttpClient httpClient, string userId)
    {
        var response = await httpClient.GetAsync($"https://api.spotify.com/v1/users/{userId}/playlists");
        
        var responseJson = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrWhiteSpace(responseJson))
            return new List<Playlist>();

        var responseData = JsonSerializer.Deserialize<UserPlaylists>(responseJson);

        if (responseData == null)
            return new List<Playlist>();

        return responseData.Items;
    }
    
    public async Task<Playlist> GetPlaylistData(HttpClient httpClient, string playlistId)
    {
        var response = await httpClient.GetAsync($"https://api.spotify.com/v1/playlists/{playlistId}");
        
        var responseJson = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrWhiteSpace(responseJson))
            return new Playlist();
        
        var responseData = JsonSerializer.Deserialize<Playlist>(responseJson);

        if (responseData == null)
            return new Playlist();

        return responseData;
    }
    
    public async Task<List<PlaylistTrack>> GetPlaylistTracksAsync(HttpClient httpClient, string playlistId)
    {
        var response = await httpClient.GetAsync($"https://api.spotify.com/v1/playlists/{playlistId}/tracks");

        var responseJson = await response.Content.ReadAsStringAsync();

        if (!string.IsNullOrWhiteSpace(responseJson))
        {
            var responseData = JsonSerializer.Deserialize<PlaylistTracks>(responseJson);

            if (responseData == null)
                return new List<PlaylistTrack>();

            return responseData.Items.Select(i => i.Track).ToList();
        }

        return new List<PlaylistTrack>();
    }

    public async Task<UrlItem> GetPlaylistImageUrlAsync(HttpClient httpClient, string playlistId)
    {
        var response = await httpClient.GetAsync($"https://api.spotify.com/v1/playlists/{playlistId}/images");

        var responseJson = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrWhiteSpace(responseJson))
            return new UrlItem();

        var responseData = JsonSerializer.Deserialize<List<UrlItem>>(responseJson);

        if (responseData == null)
            return new UrlItem();

        return responseData.First();
    }

    public async Task<Result<string>> GetBearerTokenAsync(HttpClient httpClient, SpotifyApiCredentials spotifyApiCredentials)
    {

        if (string.IsNullOrWhiteSpace(spotifyApiCredentials.ClientId))
            return new Result<string>().WithError("Please add your Spotify ClientId to the settings file.");
        
        if (string.IsNullOrWhiteSpace(spotifyApiCredentials.ClientSecret))
            return new Result<string>().WithError("Please add your Spotify ClientSecret to the settings file.");
        
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
        "Basic",
        Convert.ToBase64String(Encoding.UTF8.GetBytes($"{spotifyApiCredentials.ClientId}:{spotifyApiCredentials.ClientSecret}")));

        // Set the request body
        var requestBody = new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" }
        };

        var response = await httpClient.PostAsync(
            "https://accounts.spotify.com/api/token",
            new FormUrlEncodedContent(requestBody));

        var responseJson = await response.Content.ReadAsStringAsync();
        var responseData = JsonSerializer.Deserialize<TokenResponse>(responseJson);

        if (responseData == null)
            return new Result<string>().WithError("Unable to fetch bearer token from API.");

        return new Result<string>(responseData.AccessToken);
    }
}