using System.Text.Json;
using RFBAutomaticDataDownloader.Helpers;

namespace RFBAutomaticDataDownloader.Services;

public static class ConfigFileSetup
{
    public static SetupResult SetupConfig(out Config config)
    {
        //return base predefinition.
        config = new Config();

        string configFilePath = DirectoryHelper.CreateBaseDirectory("config");
        string fileName = Path.Combine(configFilePath, "config.json");
        if(!File.Exists(fileName))
        {
            using var file = File.Create(fileName);
            SetupFile(file);
            return SetupResult.SETUP;
        }
        var json = File.ReadAllText(fileName);
        if(json.Length == 0) return SetupResult.ERROR;
        config = JsonSerializer.Deserialize<Config>(json)!;
        return SetupResult.OK;
    }
    private static void SetupFile(FileStream file)
    {
        var config = new Config();
        var json = JsonSerializer.Serialize(config, new JsonSerializerOptions{ WriteIndented = true });
        using var writer = new StreamWriter(file);
        writer.Write(json);
    }
}