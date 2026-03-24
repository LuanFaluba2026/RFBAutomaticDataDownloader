using System.IO.Compression;

namespace RFBAutomaticDataDownloader.Helpers;
public class Decompresser : IDisposable
{
    private static readonly string outputPath = DirectoryHelper.CreateBaseDirectory("decompressed");
    public string Decompress(string file)
    {
        ZipFile.ExtractToDirectory(file, outputPath, overwriteFiles: true);
        return Directory.GetFiles(outputPath)[0];
    }
    public void Dispose()
    {
        string[] files = Directory.GetFiles(outputPath);
        foreach(var f in files)
            File.Delete(f);
    }
}