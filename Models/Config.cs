public class Config
{
    public string OutputPath { get; set;} = @"C:\";
    public string AppDirectory { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RFBAutomaticDataDownloader.exe");
}