using System.Runtime.CompilerServices;
using RFBAutomaticDataDownloader.Helpers;
using RFBAutomaticDataDownloader.Services;

public class Program
{
    public static Config AppConfig { get; private set; } = new();
    public static void Main()
    {
        var setup = ConfigFileSetup.SetupConfig(out Config AppConfig);

        switch (setup)
        {
            case SetupResult.SETUP:
                Console.WriteLine("O arquivo de configuração foi criado em '/Config/config.json'. Gentileza parametrizar as predefinições do sistema.");
                Console.ReadKey();
            break;
            case SetupResult.ERROR:
                Console.WriteLine("Ocorreu um erro ao configurar. Gentileza verificar com supervisor");
                Console.ReadKey();
            break;
            case SetupResult.OK:
                Console.WriteLine("Starting Process...");
                StartProcess().GetAwaiter().GetResult();
            break;
        }


    }
    private static async Task StartProcess()
    {
        if(AppConfig.ScheduleMonthlyDownload) Scheduler.SetupSchedule();
        
        Console.WriteLine("Iniciando o Download...");
        string downloadedFile = await WebDownloader.DownloadFile(await WebDownloader.GetUrl());
        if(!File.Exists(downloadedFile))
        {
            Console.WriteLine("Nenhum arquivo foi baixado.");
            return;
        }
        Console.WriteLine("Download Completed.");
        
        Console.WriteLine("Extraindo arquivo...");
        Decompresser.Decompress(downloadedFile);
        Console.WriteLine("Extração completa.");
    }
    
}