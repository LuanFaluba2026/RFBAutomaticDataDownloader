namespace RFBAutomaticDataDownloader.Helpers;
public static class DirectoryHelper
{
    public static readonly string BasePath = AppDomain.CurrentDomain.BaseDirectory;

    public static string CreateBaseDirectory(string directoryName)
    {
        string path = Path.Combine(BasePath, directoryName);
        if(!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            return path;
        }
        return path;
    }
}