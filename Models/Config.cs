public class Config
{
    public bool ScheduleMonthlyDownload { get; set; } = false;
    public string DatabasePath { get; set; } = @"\\10.15.1.130\ProgramasCGC\Banco_Receita\";
    public string FileName { get ; set; } = "Simples.zip";
    public bool OnlyParse { get; set; } = false;
}