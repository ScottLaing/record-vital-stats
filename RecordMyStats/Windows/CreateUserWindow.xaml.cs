namespace RecordMyStats.Windows;

/// <summary>       
/// Interaction logic for CreateUserWindow.xaml
/// </summary>
public partial class CreateUserWindow : Window
{
    private IVitalsBLL vitalsBLL = VitalsFactory.GetVitalsBLL();
    public string? SessionKey { get; private set; }
    private string Token { get; set; } = "";

    public CreateUserWindow()
    {
        InitializeComponent();
        this.Title = Constants.AppGlobal.ApplicationName + " - Create New User";

        this.cmbSex.Items.Add("Male");
        this.cmbSex.Items.Add("Female");
        this.cmbSex.Items.Add("Non-Binary");
        this.cmbSex.Items.Add("Don't wish to say");

        string[] countries = new string[] {
            "United States",
            "Mexico",
            "Canada",
            "France",
            "Germany",
            "Italy",
            "China",
            "India",
            "Hungary",
            "South Africa",
            "Egypt",
            "Japan"
        };

        var countryList = countries.OrderBy(c => c).ToList();
        countryList.ForEach(c => this.cmbCountry.Items.Add(c));

        this.txtFirstName.Focus();
    }

    private void btnRecord_Click(object sender, RoutedEventArgs e)
    {
        string pword = this.txtPassword.Password;
        if (pword.Length < 8)
        {
            MessageBox.Show("Password should be 8 chars or more - please retry.");
            return;
        }
        var chars = new char[] { ' ', '\n', '\t', '\r' };
        if (chars.Any( c => pword.Contains(c)))
        {
            MessageBox.Show("Password cannot contain spaces, new lines or tabs - please retry.");
            return;
        }

        if (pword != this.txtPassword2.Password)
        {
            MessageBox.Show("Passwords do not match - please retry.");
            return;
        }

        if (pword.Any(char.IsUpper) && pword.Any(char.IsLower))
        {

        }
        else
        {
            MessageBox.Show("Password must contain one upper and one lower character - please retry.");
            return;
        }

        Regex rgx = new Regex("[^A-Za-z0-9]");
        bool hasSpecialChars = rgx.IsMatch(pword);
        if (! hasSpecialChars)
        {
            MessageBox.Show("Password must contain a special character like one of: (*!@#$.&( - please retry.");
            return;
        }

        if (this.cmbSex.SelectedIndex == -1)
        {
            MessageBox.Show("No option chosen for Sex.");
            return;
        }
        if (this.cmbCountry.SelectedIndex == -1)
        {
            MessageBox.Show("No option chosen for Country.");
            return;
        }

        try
        {
            DateTime now = DateTime.Now;
            DateTime x = DateTime.Parse(this.txtDOB.Text);
            if ( now.Year < x.Year)
            {
                MessageBox.Show("Date of birth is invalid (year is future).");
                return;
            }
            if (now.Year - x.Year > 130)
            {
                MessageBox.Show("Date of birth is invalid (year too old).");
                return;
            }
        }
        catch
        {
            MessageBox.Show("Date of birth is invalid.");
            return;
        }

        if (!EmailUtility.IsValidEmail(this.txtEmail.Text))
        {
            MessageBox.Show("Email format does not appear to be valid.");
            return;
        }

        bool emailInUse = vitalsBLL.IsEmailInUse(this.txtEmail.Text, out string errors4);

        if (emailInUse)
        {
            MessageBox.Show($"A user with this email already exists in the system. {errors4}");
            return;
        }

        if (!string.IsNullOrWhiteSpace(errors4))
        {
            MessageBox.Show($"Error verifying unique email, {errors4}.");
            return;
        }

        var member = new Member()
        {
            FirstName = this.txtFirstName.Text,
            MiddleName = "",
            LastName = this.txtLastName.Text,
            Country = this.cmbCountry.SelectedValue.ToString(),
            Sex = GetSexAbbrev(this.cmbSex.SelectedValue.ToString()),
            Email = this.txtEmail.Text,
            Zip = "",
            Password = this.txtPassword.Password,
            DateOfBirth = DateTime.Parse(this.txtDOB.Text)
        };

        bool result = vitalsBLL.AddMember(member, out string errors, out string newSessionKey, out string newToken);

        if (result)
        {
            RecordMyStats.MySettings.Default.LastEmail = this.txtEmail.Text;
            RecordMyStats.MySettings.Default.LastPassword = this.txtPassword.Password;
            RecordMyStats.MySettings.Default.UseLastLogins = "true";
            RecordMyStats.MySettings.Default.Save();
            //MessageBox.Show($"Successfully created {member.FullName}");
        }
        else
        {
            MessageBox.Show($"Error in adding member: {errors}");
            return;
        }

        var mainWindow = new MainWindow(newSessionKey, member.FullName, newToken);
        mainWindow.Show();
        this.Close();
    }

    private string GetSexAbbrev(string? v)
    {
        string result = "";
        switch (v) {
            case "Male":
                result = "M";
                break;
            case "Female":
                result = "F";
                break;
            case "Non-Binary":
                result = "X";
                break;
            case "Don't wish to say":
                result = "N";
                break;
        }
        return result;
    }

    private void btnLogin_Click(object sender, RoutedEventArgs e)
    {
        var loginWindow = new LoginWindow();
        loginWindow.Show();
        this.Close();
    }
}
