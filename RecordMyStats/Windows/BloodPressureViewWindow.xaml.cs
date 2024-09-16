﻿using static RecordMyStats.Common.Constants;

namespace RecordMyStats.Windows;

/// <summary>
/// code behind for <see cref="BloodPressureViewWindow"/>
/// </summary>
public partial class BloodPressureViewWindow : Window
{
    private string _sessionKey;
    private string _fullName;
    private string _token;
    private IVitalsBLL vitalsBLL = VitalsFactory.GetVitalsBLL();
    private List<BloodPressure>? lastLookupResults = new List<BloodPressure>();

    public BloodPressureViewWindow(string sessionKey, string fullName, string token)
    {
        InitializeComponent();
        GlobalUISettings.AddToWindowsList(this);
        Title = Constants.AppGlobal.ApplicationName + " - List Blood Pressure";
      
        txtFullName.Content = fullName + LoggedIn;
        _sessionKey = sessionKey;
        _fullName = fullName;
        _token = token;

        var results = vitalsBLL.GetBloodPressureEntriesBySessionKey(sessionKey, token, out string errors);
        if (!string.IsNullOrEmpty(errors))
        {
            lblStatus.Content = Errors + errors;
        }
        else if (results == null)
        {
            lblStatus.Content = SomeErrorsWithLookup;
        }
        dgResults.ItemsSource = results;
        lblStatus.Content = string.Format(EntriesCount, results?.Count);
        lastLookupResults = results;

        var now = DateTime.Now;
        dpFromDate.DisplayDate = new DateTime(now.Year, 1, 1);
        dpFromDate.SelectedDate = new DateTime(now.Year, 1, 1);
        dpToDate.DisplayDate = now;
        dpToDate.SelectedDate = now;

    }

    private void btnApplyFilters_Click(object sender, RoutedEventArgs e)
    {
        var from = dpFromDate.SelectedDate;
        var to = dpToDate.SelectedDate;
        if (from == null || to == null)
        {
            MessageBox.Show(FromAndToDates, AppGlobal.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        to = new DateTime(to.Value.Year, to.Value.Month, to.Value.Day, 23, 59, 59, 999);

        var results = vitalsBLL.GetBloodPressureEntriesBySessionKey(_sessionKey, from.Value, to.Value, _token, out string errors);

        if (!string.IsNullOrEmpty(errors))
        {
            lblStatus.Content = Errors + errors;
        }
        else if (results == null)
        {
            lblStatus.Content = SomeErrorsWithLookup;
        }
        dgResults.ItemsSource = results;
        lblStatus.Content = string.Format(EntriesCount, results?.Count);
        lastLookupResults = results;
    }
}
