using Microsoft.Win32.TaskScheduler;
namespace RFBAutomaticDataDownloader.Helpers;
public class Scheduler()
{
    public static void SetupSchedule()
    {
        using var ts = new TaskService();
        var td = ts.NewTask();
        td.RegistrationInfo.Description = "Downloader Receita - Último dia do mês";
        td.Principal.RunLevel = TaskRunLevel.Highest;
        td.Settings.Hidden = true;
        td.Triggers.Add(new MonthlyDOWTrigger
        {
            DaysOfWeek = DaysOfTheWeek.AllDays,
            WeeksOfMonth = WhichWeek.LastWeek,
            MonthsOfYear = MonthsOfTheYear.AllMonths,
            StartBoundary = DateTime.Today.AddHours(13)
        });
        td.Actions.Add(new ExecAction(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RFBAutomaticDataDownloader.exe"), null, null));
        ts.RootFolder.RegisterTaskDefinition("RFBAutomaticDataDownloader", td, TaskCreation.CreateOrUpdate, null, null, TaskLogonType.InteractiveToken);
    }
}