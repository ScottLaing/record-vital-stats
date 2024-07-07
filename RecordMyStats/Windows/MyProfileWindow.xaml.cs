namespace RecordMyStats.Windows;

/// <summary>       
/// Interaction logic for MyProfileWindow.xaml
/// </summary>
public partial class MyProfileWindow : Window
{
    private IVitalsBLL vitalsBLL = VitalsFactory.GetVitalsBLL();
    public string? SessionKey { get; private set; }

    public MyProfileWindow(string sessionKey,  string fullName, string token)
    {
        InitializeComponent();
        this.Title = Constants.AppGlobal.ApplicationName + " - User Profile";
        SessionKey = sessionKey;
        var memberInfo = vitalsBLL.GetMemberInfoBySessionKey(sessionKey, token, out string errors);
        if (memberInfo != null)
        {
            txtEmail.Text = memberInfo.Email;
            txtAccountCreated.Text = memberInfo.CreateDate?.ToString("MM/dd/yyyy") ?? "";
            txtCountry.Text = memberInfo.Country;
            txtFirstName.Text = fullName;
        }

    }
}
