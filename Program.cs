using RFBAutomaticDataDownloader.Helpers;

public class Program
{
    private static readonly string BasePath = AppDomain.CurrentDomain.BaseDirectory;
    public static Config AppConfig { get; private set; } = new();
    static async Task Main()
    {
        var setup = ConfigFileSetup.SetupConfig(BasePath, out Config AppConfig);

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
                Scheduler.SetupSchedule();
                await DownloadFile(AppConfig.OutputPath);
            break;
        }


    }
    private static async Task DownloadFile(string outputPath)
    {
        string downloadUrl = @"https://arquivos.receitafederal.gov.br/public.php/dav/files/YggdBLfdninEJX9/2026-03/Simples.zip";
        
        using var client = new HttpClient();
        
        using var response = await client.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead);
        Console.WriteLine(response.StatusCode);

        using var stream = await response.Content.ReadAsStreamAsync();
        using var file = new FileStream(Path.Combine(outputPath, "Simples.zip"), FileMode.Create, FileAccess.Write);
        
        var totalBytes =  response.Content.Headers.ContentLength ?? -1L;
        var canShowProgress = totalBytes != -1;
        var buffer = new byte[8192];
        long readTotal = 0;
        int readBytes;
        while((readBytes = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {
            await file.WriteAsync(buffer, 0, readBytes);
            readTotal += readBytes;
            if(canShowProgress)
            {
                double progress = (double)readTotal / totalBytes * 100;
                Console.Write($"\rBaixando: {progress:F2}%");
            }
            else
            {
                Console.Write($"\rBaixando: {readTotal / 1024 / 1024} MB");
            }
        }
        Console.WriteLine("Download Completed.");
    }
}