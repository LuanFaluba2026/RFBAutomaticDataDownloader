public class Config
{
    public string AppDirectory { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RFBAutomaticDataDownloader.exe");
    public bool ScheduleMonthlyDownload { get; set; } = false;
}