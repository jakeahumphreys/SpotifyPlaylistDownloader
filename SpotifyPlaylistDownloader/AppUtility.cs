using System.Text.Json;
using SpotifyPlaylistDownloader.Models.Authentication;

namespace SpotifyPlaylistDownloader;

public static class AppUtility
{
    public static AppSettings LoadSettings()
    {
        var appDirectory = GetAppDirectory();
        var settingsFile = Path.Combine(appDirectory, "appsettings.json");

        if (!Directory.Exists(appDirectory))
            Directory.CreateDirectory(appDirectory);

        if (!File.Exists(settingsFile))
        {
            var configFileJson = JsonSerializer.Serialize(CreateNewConfigFile());
            File.WriteAllText(settingsFile, configFileJson);
        }
        
        var settingsFileJsonString = File.ReadAllText(settingsFile);
        var loadedAppSettings = JsonSerializer.Deserialize<AppSettings>(settingsFileJsonString);

        return loadedAppSettings;
    }

    private static AppSettings CreateNewConfigFile()
    {
        return new AppSettings
        {
            SpotifyApiCredentials = new SpotifyApiCredentials
            {
                ClientId = "",
                ClientSecret = "",
                SpotifyUserId = ""
            }
        };
    }

    public static string GetAppDirectory()
    {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SpotifyPlaylistExporter");
    }
}


