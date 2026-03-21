using System.Text.Json;

namespace RFBAutomaticDataDownloader.Helpers;

public static class ConfigFileSetup
{
    public static SetupResult SetupConfig(string basePath, out Config config)
    {
        //return base predefinition.
        config = new Config();

        string configFilePath = Path.Combine(basePath, "Config");
        string fileName = Path.Combine(configFilePath, "config.json");
        if(!Directory.Exists(configFilePath))
        {
            Directory.CreateDirectory(configFilePath);
            using var file = File.Create(fileName);
            SetupFile(file);
            return SetupResult.SETUP;
        }
        if(!File.Exists(fileName)) return SetupResult.ERROR;

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