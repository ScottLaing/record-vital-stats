﻿namespace RecordMyStats.Windows;

/// <summary>
/// code behind for <see cref="BloodSugarWindow"/>
/// </summary>
public partial class BloodSugarWindow : Window
{
    private string _sessionKey;
    private string _fullName;
    private string _token;
    private IVitalsBLL vitalsBLL = VitalsFactory.GetVitalsBLL();

    public BloodSugarWindow(string sessionKey, string fullName, string token)
    {
        InitializeComponent();
        GlobalUISettings.AddToWindowsList(this);

        var info = vitalsBLL.GetMemberInfoBySessionKey(sessionKey, token, out string memberInfoErrors);

        this.Title = Constants.AppGlobal.ApplicationName + " - Blood Sugar";
        UpdateTime();
        txtFullName.Content = fullName + " logged in.";
        _sessionKey = sessionKey;
        _fullName = fullName;
        _token = token;

        cmbBloodSugarUnits.Items.Add("mg/dL");
        cmbBloodSugarUnits.Items.Add("mmol/L");
        cmbBloodSugarUnits.SelectedIndex = 0;

        cmbWhenMeasured.Items.Add("Fasting (overnite)");
        cmbWhenMeasured.Items.Add("Pre-meal");
        cmbWhenMeasured.Items.Add("2 Hours After Meal");
        cmbWhenMeasured.Items.Add("1 Hour After Meal");
        cmbWhenMeasured.Items.Add("15 Mins After Meal");

        cmbMood.Items.Add("No Selection");
        cmbMood.Items.Add("Happy");
        cmbMood.Items.Add("Okay");
        cmbMood.Items.Add("Sad");
        cmbMood.Items.Add("Tired");
        cmbMood.SelectedIndex = 1;

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
        if (this.cmbWhenMeasured.SelectedIndex == -1)
        {
            MessageBox.Show("Please select a choice for when measured", Constants.AppGlobal.ApplicationName);
            return;
        }

        if (this.cmbBloodSugarUnits.SelectedIndex == -1)
        {
            MessageBox.Show("Please select a blood sugars unit from blood sugar units drop down", Constants.AppGlobal.ApplicationName);
            return;
        }

        var bsUnits = this.cmbBloodSugarUnits.SelectedValue.ToString();

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

        string whenMeasured = this.cmbWhenMeasured.SelectedItem?.ToString() ?? "";

        string level = this.txtBloodSugar.Text.Trim();
        if (! int.TryParse(level, out int levelInt))
        {
            MessageBox.Show("Blood sugar value should be a number", Constants.AppGlobal.ApplicationName);
            return;
        }

        if (levelInt <= 0)
        {
            MessageBox.Show("Invalid Blood sugar value", Constants.AppGlobal.ApplicationName);
            return;
        }

        BloodSugar entry = new BloodSugar()
        {
            Value = levelInt,
            Units = bsUnits ?? "",
            RecordingDate = newDateTime,
            CreateDate = DateTime.Now,
            WhenTaken = whenMeasured,
            Mood = cmbMood.SelectedIndex,
            Comments = txtComments.Text
        };

        bool success = vitalsBLL.AddBloodSugarEntry(entry, _sessionKey, _token, out string addEntryErrors);
        if (success)
        {
           // MessageBox.Show("Blood sugar saved successfully.", Constants.AppGlobal.ApplicationName);
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
