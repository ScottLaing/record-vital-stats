using Simple;

namespace RecordMyStats.Windows;

/// <summary>
/// Interaction logic for LoginWindow.xaml
/// </summary>
public partial class LoginWindow : Window
{
    private IVitalsBLL vitalsBLL = VitalsFactory.GetVitalsBLL();
    private bool _questionsMode = false;

    public LoginWindow()
    {
        InitializeComponent();
        this.Title = Constants.AppGlobal.ApplicationName + " - Login";
        if (RecordMyStats.MySettings.Default.UseLastLogins == "true")
        {
            this.txtEmail.Text = RecordMyStats.MySettings.Default.LastEmail;
            this.txtPassword.Password = RecordMyStats.MySettings.Default.LastPassword;
            this.chkSaveLoginSettings.IsChecked = true;
        }
    }

    private void btnCreateNewUser_Click(object sender, RoutedEventArgs e)
    {
        var createUserWindow = new CreateUserWindow();
        createUserWindow.Show();
        this.Close();
    }

    private void btnLoginUser_Click(object sender, RoutedEventArgs e)
    {
        bool success = vitalsBLL.LoginMember(this.txtEmail.Text, this.txtPassword.Password, out string sessionKey, out string fullName, out string token, out string errors);

        if (this.chkSaveLoginSettings.IsChecked ?? false)
        {
            RecordMyStats.MySettings.Default.LastEmail = this.txtEmail.Text;
            RecordMyStats.MySettings.Default.LastPassword = this.txtPassword.Password;
            RecordMyStats.MySettings.Default.UseLastLogins = "true";
            RecordMyStats.MySettings.Default.Save();
        }
        else
        {
            RecordMyStats.MySettings.Default.UseLastLogins = "false";
        }

        if (success)
        {
            if (_questionsMode)
            {
                var questionsWindow = new QuestionsWindow(sessionKey, fullName, token);
                questionsWindow.Show();
                this.Close();
            }
            else
            {
                var mainWindow = new MainWindow(sessionKey, fullName, token);
                mainWindow.Show();
                this.Close();
            }
        }
        else
        {
            MessageBox.Show($"Trouble logging in. {errors}");
        }
    }

    private void btnTest_Click(object sender, RoutedEventArgs e)
    {
        ISaveNoteManager saveNoteManager = new SaveNoteManager();
        saveNoteManager.SaveNote("the quick brown fox\njumped over\nthe lazy dog", "scottlaing+250@gmail.com", "Scooby123*");
    }
}
