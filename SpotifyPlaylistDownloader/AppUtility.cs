using System.Text.Json;
using Microsoft.Extensions.Configuration;
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

        var config = new ConfigurationBuilder()
            .AddJsonFile(settingsFile, false, true)
            .Build();

        var settingsFileJsonString = File.ReadAllText(settingsFile);
        

        var loadedAppSettings = JsonSerializer.Deserialize<AppSettings>(settingsFileJsonString);
        //
        // if (!IsSettingsJsonValid(settingsFileJsonString))
        //     loadedAppSettings = UpgradeConfigFile(loadedAppSettings);
            
        return loadedAppSettings;
    }

    // private static bool IsSettingsJsonValid(string currentSettingsJson)
    // {
    //     var schemaGenerator = new JSchemaGenerator();
    //     var settingsSchema = schemaGenerator.Generate(typeof(AppSettings));
    //     
    //     JObject currentSettingsSchema = JObject.Parse(currentSettingsJson);
    //
    //     return currentSettingsSchema.IsValid(settingsSchema);
    // }
    
    private static AppSettings CreateNewConfigFile()
    {
        return new AppSettings
        {
            SpotifyApiCredentials = new SpotifyApiCredentials
            {
                ClientId = "",
                ClientSecret = ""
            }
        };
    }

    public static string GetAppDirectory()
    {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SpotifyPlaylistExporter");
    }

    // private static AppSettings UpgradeConfigFile(AppSettings currentAppSettings)
    // {
    //     Console.WriteLine("Updating your settings file");
    //     return new AppSettings
    //     {
    //         SpotifyApiCredentials = new SpotifyApiCredentials
    //         {
    //             ClientId = currentAppSettings.SpotifyApiCredentials.ClientId,
    //             ClientSecret = currentAppSettings.SpotifyApiCredentials.ClientSecret,
    //             SpotifyUserId = currentAppSettings.SpotifyApiCredentials.SpotifyUserId
    //         }
    //     };
    // }
}


