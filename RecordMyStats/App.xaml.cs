namespace RecordMyStats;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    void Application_Startup(object sender, StartupEventArgs e)
    {
        string checkConnString = Config.DbConnectionStrings.VitalsDbConnString;
    }
}
