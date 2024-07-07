namespace RecordMyStats.Windows;

/// <summary>
/// code behind for <see cref="BloodPressureWindow"/>
/// </summary>
public partial class BloodPressureWindow : Window
{
    private string _sessionKey;
    private string _fullName;
    private string _token;
    private IVitalsBLL vitalsBLL = VitalsFactory.GetVitalsBLL();

    public BloodPressureWindow(string sessionKey, string fullName, string token)
    {
        InitializeComponent();
        GlobalUISettings.AddToWindowsList(this);

        var info = vitalsBLL.GetMemberInfoBySessionKey(sessionKey, token, out string memberInfoErrors);

        this.Title = Constants.AppGlobal.ApplicationName + " - Blood Pressure";
        UpdateTime();
        txtFullName.Content = fullName + " logged in.";
        _sessionKey = sessionKey;
        _fullName = fullName;
        _token = token;

        cmbBloodPressureUnits.Items.Add("mmHg");
        cmbBloodPressureUnits.SelectedIndex = 1;

        cmbWhenMeasured.Items.Add("Morning");
        cmbWhenMeasured.Items.Add("During the day");
        cmbWhenMeasured.Items.Add("Before sleep");
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
            MessageBox.Show("Please select a choice for when measured", Constants.AppGlobal.ApplicationName);
            return;
        }
        whenMeasured = this.cmbWhenMeasured?.SelectedItem?.ToString() ?? "";

        if (this.cmbBloodPressureUnits.SelectedIndex == -1)
        {
            MessageBox.Show("Please select a blood pressure unit from blood sugar units drop down", Constants.AppGlobal.ApplicationName);
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
            var dateStr = (this.dpDate.SelectedDate?.ToString("yyyy-MM-dd") ?? "") + " " + this.txtTime.Text;

            if (!DateTime.TryParseExact(dateStr, "yyyy-MM-dd HH:mm.ss",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None,
                       out newDateTime))
            {
                MessageBox.Show("Date and time are not in right format", Constants.AppGlobal.ApplicationName);
                return;
            }

            if (newDateTime > DateTime.Now)
            {
                MessageBox.Show("Date time cannot be future", Constants.AppGlobal.ApplicationName);
                return;
            }
        }

        var systolic = txtBloodPressureSystolic.Text;

        float fSystolic;
        if (! float.TryParse(systolic, out fSystolic))
        {
            MessageBox.Show("Systolic blood pressure value not valid.", Constants.AppGlobal.ApplicationName);
            return;
        }
        if (fSystolic <= 0)
        {
            MessageBox.Show("Systolic blood pressure value not valid", Constants.AppGlobal.ApplicationName);
            return;
        }

        float fDiastolic;

        var diastolic = txtBloodPressureDiastolic.Text;
        if (! float.TryParse(diastolic, out fDiastolic))
        {
            MessageBox.Show("Diastolic blood pressure value not valid.", Constants.AppGlobal.ApplicationName);
            return;
        }

        if (fDiastolic <= 0)
        {
            MessageBox.Show("Diastolic blood pressure value not valid", Constants.AppGlobal.ApplicationName);
            return;
        }

        BloodPressure entry = new BloodPressure()
        {
            Systolic = fSystolic,
            Diastolic = fDiastolic,
            Units = bpUnits ?? "",
            RecordingDate = newDateTime,
            CreateDate = DateTime.Now,
            WhenTaken = whenMeasured
        };

        bool success = vitalsBLL.AddBloodPressureEntry(entry, _sessionKey, _token, out string addEntryErrors);
        if (success)
        {
           // MessageBox.Show("Blood pressure saved successfully.", Constants.AppGlobal.ApplicationName);
        }
        else
        {
            MessageBox.Show($"Trouble saving entry: {addEntryErrors}", Constants.AppGlobal.ApplicationName);
        }
        this.Close();
    }

    private void rbEntryTimeNow_Checked(object sender, RoutedEventArgs e)
    {
        this.dpDate.IsEnabled = false;
        this.txtTime.IsEnabled = false;
    }

    private void rbEntryTimeCustom_Checked(object sender, RoutedEventArgs e)
    {
        this.dpDate.IsEnabled = true;
        this.txtTime.IsEnabled = true;
    }
}
