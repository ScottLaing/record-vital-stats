using static RecordMyStats.Common.Constants;

namespace RecordMyStats.Windows;

/// <summary>
/// code behind for <see cref="BloodSugarViewWindow"/>
/// </summary>
public partial class BloodSugarViewWindow : Window
{
    private string _sessionKey;
    private string _fullName;
    private string _token;
    private IVitalsBLL vitalsBLL = VitalsFactory.GetVitalsBLL();
    List<BloodSugar>? lastLookupResults = new List<BloodSugar>();

    public BloodSugarViewWindow(string sessionKey, string fullName, string token)
    {
        InitializeComponent();
        GlobalUISettings.AddToWindowsList(this);
        this.Title = Constants.AppGlobal.ApplicationName + " - List Blood Sugar";
      
        txtFullName.Content = fullName + LoggedIn;
        _sessionKey = sessionKey;
        _fullName = fullName;
        _token = token;

        var results = vitalsBLL.GetBloodSugarEntriesBySessionKey(sessionKey, token, out string errors);
        if (!string.IsNullOrEmpty(errors))
        {
            lblStatus.Content = "errors: " + errors;
        }
        else if (results == null)
        {
            lblStatus.Content = SomeErrorsWithLookup;
        }
        dgResults.ItemsSource = results;
        lblStatus.Content = string.Format(EntriesCount, results?.Count);
        lastLookupResults = results;

        var now = DateTime.Now;
        this.dpFromDate.DisplayDate = new DateTime(now.Year, 1, 1);
        this.dpFromDate.SelectedDate = new DateTime(now.Year, 1, 1);
        this.dpToDate.DisplayDate = now;
        this.dpToDate.SelectedDate = now;

    }

    private void btnApplyFilters_Click(object sender, RoutedEventArgs e)
    {
        var from = dpFromDate.SelectedDate;
        var to = dpToDate.SelectedDate;
        if (from == null || to == null)
        {
            MessageBox.Show(FromAndToDates);
            return;
        }

        to = new DateTime(to.Value.Year, to.Value.Month, to.Value.Day, 23, 59, 59, 999);

        var results = vitalsBLL.GetBloodSugarEntriesBySessionKey(_sessionKey, from.Value, to.Value, _token, out string errors);

        if (!string.IsNullOrEmpty(errors))
        {
            lblStatus.Content = "errors: " + errors;
        }
        else if (results == null)
        {
            lblStatus.Content = SomeErrorsWithLookup;
        }
        dgResults.ItemsSource = results;
        lblStatus.Content = string.Format(EntriesCount,results?.Count);
        lastLookupResults = results;

    }

    private void btnRecord_Click(object sender, RoutedEventArgs e)
    {
     
    }

}
