using System.Windows.Input;
using static RecordMyStats.Common.Constants;

namespace RecordMyStats.Windows;

/// <summary>
/// code behind for <see cref="BloodPressureWindow"/>
/// </summary>
public partial class BloodPressureWindow : Window
{
    private const string SelectBloodPressureUnit = "Please select a blood pressure unit from blood sugar units drop down";
    private readonly string WindowTitle = Constants.AppGlobal.ApplicationName + " - Blood Pressure";
    private string _sessionKey;
    private string _fullName;
    private string _token;
    private IVitalsBLL vitalsBLL = VitalsFactory.GetVitalsBLL();
    private const int PulseRateMin = 30;
    private const int PulseRateMax = 300;   

    public BloodPressureWindow(string sessionKey, string fullName, string token)
    {
        InitializeComponent();
        GlobalUISettings.AddToWindowsList(this);

        var info = vitalsBLL.GetMemberInfoBySessionKey(sessionKey, token, out string memberInfoErrors);

        this.Title = WindowTitle;
        UpdateTime();
        txtFullName.Content = fullName + LoggedIn;
        _sessionKey = sessionKey;
        _fullName = fullName;
        _token = token;

        cmbBloodPressureUnits.Items.Add(MillimetersOfMercury);
        cmbBloodPressureUnits.SelectedIndex = 0;

        BloodPressureMeasuringTimes.ForEach(s => cmbWhenMeasured.Items.Add(s));

        Constants.MoodMapDictionary.Values.ToList().ForEach(s => cmbMood.Items.Add(s));

        rbEntryTimeNow_Checked(null, null);
    }

    private void UpdateTime()
    {
        var now = DateTime.Now;
        this.dpDate.SelectedDate = now;
        this.txtTime.Text = string.Format("{0:D2}:{1:D2}:{2:D2}", now.Hour, now.Minute, now.Second);
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

        if (this.cmbBloodPressureUnits.SelectedIndex == -1)
        {
            MessageBox.Show(SelectBloodPressureUnit, Constants.AppGlobal.ApplicationName);
            return;
        }

        var bpUnits = this.cmbBloodPressureUnits.SelectedValue.ToString();

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

        var systolic = txtBloodPressureSystolic.Text;

        float fSystolic;
        if (! float.TryParse(systolic, out fSystolic))
        {
            MessageBox.Show(SystolicBloodPressureNotValid, Constants.AppGlobal.ApplicationName);
            return;
        }
        if (fSystolic <= 0)
        {
            MessageBox.Show(SystolicBloodPressureNotValid, Constants.AppGlobal.ApplicationName);
            return;
        }

        float fDiastolic;

        var diastolic = txtBloodPressureDiastolic.Text;
        if (! float.TryParse(diastolic, out fDiastolic))
        {
            MessageBox.Show(DiastolicBloodPressureNotValid, Constants.AppGlobal.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.None);
            return;
        }

        if (fDiastolic <= 0)
        {
            MessageBox.Show(DiastolicBloodPressureNotValid, Constants.AppGlobal.ApplicationName);
            return;
        }

        if (!int.TryParse(this.txtHeartRate.Text, out int heartRate))
        {
            MessageBox.Show(HeartRateShouldBeNumber, Constants.AppGlobal.ApplicationName);
            return;
        }

        if (heartRate > PulseRateMax || heartRate < PulseRateMin)
        {
            MessageBox.Show(HeartRateNotInRange, Constants.AppGlobal.ApplicationName);
            return;
        }

        BloodPressure entry = new BloodPressure()
        {
            Systolic = fSystolic,
            Diastolic = fDiastolic,
            Units = bpUnits ?? "",
            RecordingDate = newDateTime,
            CreateDate = DateTime.Now,
            WhenTaken = whenMeasured,
            Mood = cmbMood.SelectedIndex,
            HeartRate = heartRate,
            Comments = txtComments.Text,
            IsActive = true
        };

        bool success = vitalsBLL.AddBloodPressureEntry(entry, _sessionKey, _token, out string addEntryErrors);
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
            //TODO - odd error, log with Serilog in future
        }
    }

    private void rbEntryTimeCustom_Checked(object sender, RoutedEventArgs e)
    {
        this.dpDate.IsEnabled = true;
        this.txtTime.IsEnabled = true;
    }
}
