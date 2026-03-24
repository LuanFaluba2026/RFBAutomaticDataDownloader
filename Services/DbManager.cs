using System.Data.SQLite;
using RFBAutomaticDataDownloader.Helpers;

namespace RFBAutomaticDataDownloader.Services;
public class DbManager
{
    private static readonly string dbPath = Path.Combine(Program.AppConfig.DatabasePath, "RFBDatabase.db");
    private static SQLiteConnection Connection()
    {
        var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;");
        conn.Open();
        return conn;
    }
    public static void ParseDatabase(string decompressed)
    {
        try
        {
            if(!File.Exists(dbPath)) SQLiteConnection.CreateFile(dbPath);

            using var conn = Connection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "CREATE TABLE IF NOT EXISTS TabelaSimplesNacional (CnpjBasico TEXT, OpcaoSimples TEXT, OpcaoMei TEXT)";
            cmd.ExecuteNonQuery();

            var transaction = conn.BeginTransaction();
            cmd.CommandText = "INSERT INTO TabelaSimplesNacional (CnpjBasico, OpcaoSimples, OpcaoMei) VALUES (@c1, @c2, @c3)";
            foreach(var line in File.ReadLines(decompressed))
            {
                string[] cols = line.Split(';');
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@c1", cols[0].Replace("\"", ""));
                cmd.Parameters.AddWithValue("@c2", cols[1].Replace("\"", ""));
                cmd.Parameters.AddWithValue("@c3", cols[4].Replace("\"", ""));
                cmd.ExecuteNonQuery();
            }
            transaction.Commit();
            conn.Close();
        }
        catch(Exception ex)
        {
            Logger.CreateLog(LogType.Erro, ex.Message + "em" + ex.StackTrace);
        }
        
    }
}