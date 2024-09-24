using Newtonsoft.Json.Linq;
using System.Windows.Input;
using static RecordMyStats.Common.Constants;


namespace RecordMyStats.Windows;

/// <summary>
/// code behind for <see cref="BloodPressureWindow"/>
/// </summary>
public partial class O2LevelWindow : Window
{
    private string _sessionKey;
    private string _fullName;
    private string _token;
    private IVitalsBLL vitalsBLL = VitalsFactory.GetVitalsBLL();
    private const string OxygenValueNotValid = "Oxygen value not valid.";

    public O2LevelWindow(string sessionKey, string fullName, string token)
    {
        InitializeComponent();
        GlobalUISettings.AddToWindowsList(this);

        var info = vitalsBLL.GetMemberInfoBySessionKey(sessionKey, token, out string memberInfoErrors);

        this.Title = Constants.AppGlobal.ApplicationName + " - Oxygen Level";
        UpdateTime();
        txtFullName.Content = fullName + LoggedIn;
        _sessionKey = sessionKey;
        _fullName = fullName;
        _token = token;

        cmbWhenMeasured.Items.Add("Morning");
        cmbWhenMeasured.Items.Add("During the day");
        cmbWhenMeasured.Items.Add("Before sleep");

        Constants.MoodMapDictionary.Values.ToList().ForEach(s => cmbMood.Items.Add(s));

        rbEntryTimeNow_Checked(null, null);
    }

    private void UpdateTime()
    {
        var now = DateTime.Now;
        this.dpDate.SelectedDate = now;
        this.txtTime.Text = $"{now.Hour:D2}:{now.Minute:D2}.{now.Second:D2}";
    }

    private void btnRefreshTime_Click(object sender, RoutedEventArgs e)
    {
        UpdateTime();
    }

    private void btnRecord_Click(object sender, RoutedEventArgs e)
    {
        string whenMeasured = "";
        if (this.cmbWhenMeasured.SelectedIndex == -1)
        {
            MessageBox.Show(ChoiceWhenMeasured, Constants.AppGlobal.ApplicationName);
            return;
        }
        whenMeasured = this.cmbWhenMeasured?.SelectedItem?.ToString() ?? "";

        DateTime newDateTime;

        if (rbEntryTimeNow.IsChecked ?? false)
        {
            newDateTime = DateTime.Now;
        }
        else
        {
            var dateStr = (this.dpDate.SelectedDate?.ToString(ShortDateFormat) ?? "") + " " + this.txtTime.Text;

            if (!DateTime.TryParseExact(dateStr, DateFormat1,
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None,
                       out newDateTime))
            {
                MessageBox.Show(DateTimeWrongFormat, Constants.AppGlobal.ApplicationName);
                return;
            }

            if (newDateTime > DateTime.Now)
            {
                MessageBox.Show(DateTimeCannotBeFuture, Constants.AppGlobal.ApplicationName);
                return;
            }
        }


        var o2level = txtO2Level.Text;
        if (! int.TryParse(o2level, out int iOxygenLevel))
        {
            MessageBox.Show(OxygenValueNotValid, Constants.AppGlobal.ApplicationName);
            return;
        }

        if (iOxygenLevel <= 0)
        {
            MessageBox.Show(OxygenValueNotValid, Constants.AppGlobal.ApplicationName);
            return;
        }

        if (!int.TryParse(this.txtHeartRate.Text, out int heartRate))
        {
            MessageBox.Show("Heart rate value should be a number", Constants.AppGlobal.ApplicationName);
            return;
        }

        if (heartRate > 200 || heartRate < 40)
        {
            MessageBox.Show("Heart rate value should be between 40 and 200", Constants.AppGlobal.ApplicationName);
            return;
        }

        OxygenLevel entry = new OxygenLevel()
        {
            OxygenValue = iOxygenLevel,
            RecordingDate = newDateTime,
            CreateDate = DateTime.Now,
            WhenTaken = whenMeasured,
            Mood = cmbMood.SelectedIndex,
            HeartRate = heartRate,
            Comments = txtComments.Text,
            IsActive = true
        };

        // TBD work in progress
        bool success = vitalsBLL.AddOxygenEntry(entry, _sessionKey, _token, out string addEntryErrors);
        //bool success = true;
        if (!success)
        {
            MessageBox.Show(string.Format(TroubleSavingEntry, addEntryErrors), Constants.AppGlobal.ApplicationName);
        }
        this.Close();
    }
    private void NumericTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        // Regular expression to check if the input is numeric
        Regex regex = new Regex("[^0-9]+");
        e.Handled = regex.IsMatch(e.Text);
    }

    private void NumericTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        // Allow navigation keys, backspace, delete, etc.
        if (e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Tab ||
            e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Enter)
        {
            e.Handled = false;
        }
        else
        {
            // Disallow any non-numeric key
            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)))
            {
                e.Handled = true;
            }
        }
    }

    private void rbEntryTimeNow_Checked(object sender, RoutedEventArgs e)
    {
        try
        {
            if (this.dpDate != null)
            {
                this.dpDate.IsEnabled = false;
                this.txtTime.IsEnabled = false;
            }
        }
        catch (Exception ex)
        {
            //MessageBox.Show(ex.Message, Constants.AppGlobal.ApplicationName);
        }
    }

    private void rbEntryTimeCustom_Checked(object sender, RoutedEventArgs e)
    {
        this.dpDate.IsEnabled = true;
        this.txtTime.IsEnabled = true;
    }
}
