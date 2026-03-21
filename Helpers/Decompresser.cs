using System.IO.Compression;

namespace RFBAutomaticDataDownloader.Helpers;
public class Decompresser
{
    public static string Decompress(string file)
    {
        string outputPath = DirectoryHelper.CreateBaseDirectory("decompressed");
        ZipFile.ExtractToDirectory(file, outputPath, overwriteFiles: true);
        return Directory.GetFiles(outputPath)[0];
    }
}