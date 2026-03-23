using System.Text;
using System.Xml.Linq;

namespace RFBAutomaticDataDownloader.Helpers;
public static class WebDownloader
{
    public static async Task<string> DownloadFile(string httpPath)
    {
        string downloadUrl = $@"https://arquivos.receitafederal.gov.br{httpPath}\Simples.zip";
        
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
    public static async Task<string> GetUrl()
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(new HttpMethod("PROPFIND"), "https://arquivos.receitafederal.gov.br/public.php/dav/files/YggdBLfdninEJX9/")
        {
            Content = new StringContent(@"<?xml version='1.0'?> <d:propfind xmlns:d='DAV:' xmlns:nc='http://nextcloud.org/ns' xmlns:oc='http://owncloud.org/ns' xmlns:ocs='http://open-collaboration-services.org/ns'> <d:prop><d:displayname /></d:prop> </d:propfind>",
                Encoding.UTF8, "application/xml")
        };
        var response = await client.SendAsync(request);
        var xml = await response.Content.ReadAsStringAsync();
        XDocument doc = XDocument.Parse(xml);
        XNamespace ns = "DAV:";
        var reg = doc.Descendants(ns + "response").SkipLast(1).LastOrDefault();
        return reg?.Element(ns + "href")?.Value ?? "";
    }
}