using static RecordMyStats.Common.Constants;

namespace RecordMyStats.Windows;

/// <summary>
/// code behind for <see cref="RecordStatsWindow"/>
/// </summary>
public partial class RecordStatsWindow : Window
{
    private string _sessionKey;
    private string _fullName;
    private string _token;
    private IVitalsBLL vitalsBLL = VitalsFactory.GetVitalsBLL();
    private const string PulseValueInvalid = "Pulse value is not valid.";
    private const string BloodSugarValueInvalid = "Blood sugar value is not valid.";

    public RecordStatsWindow(string sessionKey, string fullName, string token)
    {
        InitializeComponent();
        GlobalUISettings.AddToWindowsList(this);

        var info = vitalsBLL.GetMemberInfoBySessionKey(sessionKey, token, out string memberInfoErrors);

        this.Title = Constants.AppGlobal.ApplicationName + " - Quick Stats";
        UpdateTime();
        txtFullName.Content = fullName + LoggedIn;
        _sessionKey = sessionKey;
        _fullName = fullName;
        _token = token;

        cmbBloodSugarUnits.Items.Add("mg/dL");
        cmbBloodSugarUnits.Items.Add("mmol/L");
        cmbBloodSugarUnits.SelectedIndex = 0;

        cmbWeightUnits.Items.Add("lbs");
        cmbWeightUnits.Items.Add("kgs");
        cmbWeightUnits.SelectedIndex = 0;
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

    private void chkBloodPressureNotRecorded_Checked(object sender, RoutedEventArgs e)
    {
        var thisCb = sender as CheckBox;
        if (thisCb != null && thisCb.IsChecked.HasValue)
        {
            this.txtBloodPressureSys.IsEnabled = !thisCb.IsChecked.Value;
            this.txtBloodPressureDia.IsEnabled = !thisCb.IsChecked.Value;
        }
    }

    private void chkBloodSugarNotRecorded_Checked(object sender, RoutedEventArgs e)
    {
        var thisCb = sender as CheckBox;
        if (thisCb != null && thisCb.IsChecked.HasValue)
        {
            this.txtBloodSugar.IsEnabled = !thisCb.IsChecked.Value;
        }
    }

    private void chkWeightNotRecorded_Checked(object sender, RoutedEventArgs e)
    {
        var thisCb = sender as CheckBox;
        if (thisCb != null && thisCb.IsChecked.HasValue)
        {
            this.txtWeight.IsEnabled = !thisCb.IsChecked.Value;
        }
    }

    private void chkPulseNotRecorded_Checked(object sender, RoutedEventArgs e)
    {
        var thisCb = sender as CheckBox;
        if (thisCb != null && thisCb.IsChecked.HasValue)
        {
            this.txtPulse.IsEnabled = !thisCb.IsChecked.Value;
        }
    }

    private void btnRecord_Click(object sender, RoutedEventArgs e)
    {
        int? pulse = null;
        if (this.chkPulseNotRecorded.IsChecked ?? false)
        {
            pulse = null;
        }
        else
        {
            if (int.TryParse(this.txtPulse.Text, out int res))
            {
                pulse = res;
            }
            else
            {
                MessageBox.Show(PulseValueInvalid, Constants.AppGlobal.ApplicationName);
                return;
            }
        }
        int? bloodSugar = null;
       // int j = 0;
        if (this.chkBloodSugarNotRecorded.IsChecked ?? false)
        {
            bloodSugar = null;
        }
        else
        {
            if (int.TryParse(this.txtBloodSugar.Text, out int res))
            {
                bloodSugar = res;
            }
            else
            {
                MessageBox.Show(BloodSugarValueInvalid, Constants.AppGlobal.ApplicationName);
                return;
            }
        }

        int? dia = null;
        if (int.TryParse(this.txtBloodPressureDia.Text, out int intDia))
        {
            dia = intDia;
        }

        int? sys = null;
        if (int.TryParse(this.txtBloodPressureSys.Text, out int intSys))
        {
            sys = intSys;
        }

        double? wt = null;
        if (double.TryParse(this.txtWeight.Text, out double dblWeight))
        {
            wt = dblWeight;
        }

        if (this.cmbBloodSugarUnits.SelectedIndex == -1)
        {
            MessageBox.Show(SelectBloodSugarUnits, Constants.AppGlobal.ApplicationName);
            return;
        }

        var bsUnits = this.cmbBloodSugarUnits.SelectedValue.ToString();

        if (this.cmbWeightUnits.SelectedIndex == -1)
        {
            MessageBox.Show("Please select a weight unit from weight units drop down", Constants.AppGlobal.ApplicationName);
            return;
        }

        var wtUnits = this.cmbWeightUnits.SelectedValue.ToString();

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

        StatisticEntry entry = new StatisticEntry()
        {
            HeartRate = pulse,
            BloodSugar = bloodSugar,
            BPDiastolic = dia,
            BPSystolic = sys,
            Weight = wt,
            BSUnits = bsUnits,
            WeightUnits = wtUnits,
            CreateDate = newDateTime
        };

        bool success = vitalsBLL.AddEntry(entry, _sessionKey, _token, out string addEntryErrors);
        if (success)
        {
           // MessageBox.Show("Entry saved successfully.", Constants.AppGlobal.ApplicationName);
        }
        else
        {
            MessageBox.Show(string.Format(TroubleSavingEntry, addEntryErrors), Constants.AppGlobal.ApplicationName);
        }
        this.Close();
        //Note testNote = new Note()
        //{
        //    Description = "test note",
        //    FullText = "the quick brown fox jumped over the lazy dog",
        //    Key1 = "key1",
        //    Key2 = "key2",
        //    ModBy = "slaing",
        //    Created = DateTime.Now,
        //    IsActive = true
        //};

        //bool s2 = vitalsBLL.AddNoteEntry(testNote, _sessionKey, _token, out string addNoteErrors);
        //if (s2)
        //{

        //}
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
