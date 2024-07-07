namespace RecordMyStats.Windows;

/// <summary>
/// code behind for <see cref="MainWindow"/>
/// </summary>
public partial class MainWindow : Window
{
    private string _sessionKey;
    private string _fullName;
    private string _token;
    private IVitalsBLL vitalsBLL = VitalsFactory.GetVitalsBLL();

    public MainWindow(string sessionKey, string fullName, string token)
    {
        InitializeComponent();
        GlobalUISettings.AddToWindowsList(this);

        ucMainMenu.SessionKey = sessionKey;
        ucMainMenu.FullName = fullName;
        ucMainMenu.ParentWindow = this as Window;
        ucMainMenu.Token = token;

        var info = vitalsBLL.GetMemberInfoBySessionKey(sessionKey, token, out string memberInfoErrors);

        this.Title = Constants.AppGlobal.ApplicationName;
        UpdateTime();
        txtFullName.Content = fullName + " logged in.";
        _sessionKey = sessionKey;
        _fullName = fullName;
        _token = token;


    }

    private void UpdateTime()
    {
        //var now = DateTime.Now;
        //this.dpDate.SelectedDate = now;
        //this.txtTime.Text = $"{now.Hour:D2}:{now.Minute:D2}.{now.Second:D2}";
    }

    private void btnRefreshTime_Click(object sender, RoutedEventArgs e)
    {
        UpdateTime();
    }

    private void btnRecord_Click(object sender, RoutedEventArgs e)
    {
    }

    private void rbEntryTimeNow_Checked(object sender, RoutedEventArgs e)
    {
    }

    private void rbEntryTimeCustom_Checked(object sender, RoutedEventArgs e)
    {
    }
}
