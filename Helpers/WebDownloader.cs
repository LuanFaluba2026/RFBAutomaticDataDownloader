namespace RFBAutomaticDataDownloader.Helpers;
public static class WebDownloader
{
    public static async Task<string> DownloadFile(string httpPath)
    {
        string downloadUrl = $@"https://arquivos.receitafederal.gov.br/public.php/dav/files/YggdBLfdninEJX9/{httpPath}";
        
        using var client = new HttpClient();
        
        using var response = await client.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead);
        Console.WriteLine(response.StatusCode);

        using var stream = await response.Content.ReadAsStreamAsync();
        string outputPath = DirectoryHelper.CreateBaseDirectory("downloaded");
        string fileName = Path.Combine(outputPath, "Simples.zip");
        using var file = new FileStream(fileName, FileMode.Create, FileAccess.Write);
        
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
        return fileName;
    }
}