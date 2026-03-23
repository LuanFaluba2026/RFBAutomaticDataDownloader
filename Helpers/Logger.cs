namespace RFBAutomaticDataDownloader.Helpers;
public enum LogType
{
    Sucesso,
    Erro,
    Aviso
}
public class Logger
{
    private static readonly string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
    public static void CreateLog(LogType logType, string log)
    {
        Directory.CreateDirectory(logDirectory);
        string fileName = Path.Combine(logDirectory, $"Log-{DateTime.Now:dd-MM-yyyy}.txt");
        if(!File.Exists(fileName))
            using (File.Create(fileName)) { }
        string logText = $"{DateTime.Now:HH-mm-ss} [{logType}] - {log}\n";
        File.AppendAllText(fileName, logText);
        Console.Write(logText);        
    }
}