using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using SpotifyPlaylistDownloader.Models.SpotifyApi;

namespace SpotifyPlaylistDownloader
{
    class Program
    {
        private readonly AppSettings _settings;
        private readonly SpotifyClient _spotifyClient;
        
        public Program()
        {
            _settings = AppUtility.LoadSettings();
            _spotifyClient = new SpotifyClient();
        }

        static void Main(string[] args) => new Program().RunAsync().GetAwaiter().GetResult();

       
        
        private async Task RunAsync()
        {
            using var httpClient = new HttpClient();
            
            var accessTokenResult =  _spotifyClient.GetBearerTokenAsync(httpClient, _settings.SpotifyApiCredentials).Result;
            
            if(accessTokenResult.IsFailure)
                Console.WriteLine(accessTokenResult.Errors.First().Message);
            else
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessTokenResult.Content);
                
                await AllPlaylists(_spotifyClient, httpClient);
            }
        }

        private async Task AllPlaylists(SpotifyClient spotifyClient, HttpClient httpClient)
        {
            Console.WriteLine("Fetching all playlists");
            var spotifyUserId = _settings.SpotifyApiCredentials.SpotifyUserId;
            var userPlaylists = await spotifyClient.GetUserPlaylists(httpClient, spotifyUserId);
            Console.WriteLine($"Found {userPlaylists.Count} playlists to download");
            var userPlaylistIds = userPlaylists.Select(x => x.Id).ToList();

            var exportDateTime = DateTime.Now.ToString("yyyy-dd-M-HH-mm-ss");
            foreach (var playlistId in userPlaylistIds)
            {
                Console.WriteLine($"Fetching data for {playlistId}...");
                var playlistData = await spotifyClient.GetPlaylistData(httpClient, playlistId);
                Console.WriteLine($"Fetching tracks for {playlistData.Name}...");
                var playlistTracks = await spotifyClient.GetPlaylistTracksAsync(httpClient, playlistId);
                Console.WriteLine($"Found {playlistTracks.Count}, fetching cover art...");
                var coverUrl = await spotifyClient.GetPlaylistImageUrlAsync(httpClient, playlistId);
                Console.WriteLine($"Saving data...");

                var appExportsDirectory = Path.Combine(AppUtility.GetAppDirectory(), "Exports");
                var playlistExportDirectory = Path.Combine(appExportsDirectory, $"SpotifyBackup_{spotifyUserId}_{exportDateTime}");
                var playlistDirectory = Path.Combine(playlistExportDirectory, playlistData.Name);
                
                SavePlaylistData(playlistDirectory, playlistData, playlistTracks, coverUrl);
            }
        }

        private void SavePlaylistData(string playlistDirectory, Playlist playlistData, List<PlaylistTrack> playlistTracks, UrlItem coverUrl)
        {
            var sanitisedDirectory = playlistDirectory.Replace(" ", "");
            
            if (!Directory.Exists(sanitisedDirectory))
            {
                Directory.CreateDirectory(sanitisedDirectory);
            }

            var filePath = Path.Combine(sanitisedDirectory, "tracks.txt");
            using (var fileStream = File.CreateText(filePath))
            {
                foreach (var track in playlistTracks)
                {
                    fileStream.WriteLine($"{track.ExternalUrls.Spotify}");
                }
            }

            var imageFilePath = Path.Combine(sanitisedDirectory, "coverimage.png");
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(new Uri(coverUrl.url), imageFilePath);
            }
        }
    }
}