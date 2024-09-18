using static RecordMyStats.Common.Constants;

namespace RecordMyStats.Windows;

/// <summary>
/// code behind for <see cref="StatsViewWindow"/>
/// </summary>
public partial class NotesViewWindow : Window
{
    private string _sessionKey;
    private string _fullName;
    private string _token;
    private IVitalsBLL vitalsBLL = VitalsFactory.GetVitalsBLL();
    List<Note>? lastLookupResults = new List<Note>();

    public NotesViewWindow(string sessionKey, string fullName, string token)
    {
        InitializeComponent();
        GlobalUISettings.AddToWindowsList(this);
        this.Title = Constants.AppGlobal.ApplicationName + " - List Notes";
      
        txtFullName.Content = fullName + LoggedIn;
        _sessionKey = sessionKey;
        _fullName = fullName;
        _token = token;

        DateTime from = DateTime.Now.AddDays(-10);
        DateTime to = DateTime.Now;
        var results = vitalsBLL.GetNoteEntriesByRange(sessionKey, from, to, token, out string errors);
        if (!string.IsNullOrEmpty(errors))
        {
            lblStatus.Content = Errors + errors;
        }
        else if (results == null)
        {
            lblStatus.Content = SomeErrorsWithLookup;
        }
        //foreach (var result in results)
        //{
        //    if (result.IsEncrypted ?? false)
        //    {
        //        result.FullText = CryptUtils.DecryptString(result.FullText);
        //    }
        //}
        dgResults.ItemsSource = results;
        lblStatus.Content = $"Notes count: {results?.Count}";
        lastLookupResults = results;

        var now = DateTime.Now;
        this.dpFromDate.DisplayDate = from;
        this.dpFromDate.SelectedDate = from;
        this.dpToDate.DisplayDate = to;
        this.dpToDate.SelectedDate = to;

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

        var results = vitalsBLL.GetNoteEntriesByRange(_sessionKey, from.Value, to.Value, _token, out string errors);

        if (!string.IsNullOrEmpty(errors))
        {
            lblStatus.Content = Errors + errors;
        }
        else if (results == null)
        {
            lblStatus.Content = SomeErrorsWithLookup;
        }
        dgResults.ItemsSource = results;
        lblStatus.Content = $"Notes count: {results?.Count}";
        lastLookupResults = results;

    }

    private void btnRecord_Click(object sender, RoutedEventArgs e)
    {
     
    }

}
