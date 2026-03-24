using RFBAutomaticDataDownloader.Helpers;
using RFBAutomaticDataDownloader.Services;

public class Program
{
    public static Config AppConfig { get; private set; } = new();
    public static void Main()
    {
        var setup = ConfigFileSetup.SetupConfig(out Config appConfig);
        AppConfig = appConfig;

        switch (setup)
        {
            case SetupResult.SETUP:
                Logger.CreateLog(LogType.Aviso, "Criado arquivo para configuração. '/config/config.json'.");
                Console.ReadKey();
            break;
            case SetupResult.ERROR:
                Logger.CreateLog(LogType.Erro, "Ocorreu um erro ao configurar. Gentileza verificar com supervisor.");
                Console.ReadKey();
            break;
            case SetupResult.OK:
                Logger.CreateLog(LogType.Aviso, "Começando o processamento.");
                StartProcess().GetAwaiter().GetResult();
            break;
        }


    }
    private static async Task StartProcess()
    {
        if(AppConfig.ScheduleMonthlyDownload)
        {
            Scheduler.SetupSchedule();
            Logger.CreateLog(LogType.Sucesso, "Criou uma tarefa mensal sno windows");
        } 
        Logger.CreateLog(LogType.Sucesso, "Download iniciado...");
        string downloadedFile = await WebDownloader.DownloadFile(await WebDownloader.GetUrl(), AppConfig.FileName);
        if(!File.Exists(downloadedFile))
        {
            Logger.CreateLog(LogType.Erro, "Nenhum arquivo foi baixado.");
            return;
        }
        Logger.CreateLog(LogType.Sucesso, "Download Completo.");
        
        Logger.CreateLog(LogType.Sucesso, "Extraindo arquivo...");
        using var extractor = new Decompresser();
        string decompressed = extractor.Decompress(downloadedFile);
        Logger.CreateLog(LogType.Sucesso, "Extração completa.");

        Logger.CreateLog(LogType.Sucesso, "Criando Banco de Dados.");
        DbManager.ParseDatabase(decompressed);
        Logger.CreateLog(LogType.Sucesso, "Banco de Dados criado com sucesso.");
    }
    
}