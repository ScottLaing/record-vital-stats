using Newtonsoft.Json.Linq;

namespace RecordMyStats.Windows;

/// <summary>
/// code behind for <see cref="NoteWindow"/>
/// </summary>
public partial class NoteWindow : Window
{
    private string _sessionKey;
    private string _fullName;
    private string _token;
    private IVitalsBLL vitalsBLL = VitalsFactory.GetVitalsBLL();

    public NoteWindow(string sessionKey, string fullName, string token)
    {
        InitializeComponent();
        GlobalUISettings.AddToWindowsList(this);

        var info = vitalsBLL.GetMemberInfoBySessionKey(sessionKey, token, out string memberInfoErrors);

        this.Title = Constants.AppGlobal.ApplicationName + " - Notes";
        txtFullName.Content = fullName + " logged in.";
        _sessionKey = sessionKey;
        _fullName = fullName;
        _token = token;

    }


    private void btnRecord_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(this.txtNote.Text))
        {
            MessageBox.Show("Please enter a note", Constants.AppGlobal.ApplicationName);
            return;
        }

        Note entry = new Note()
        {
            Description = "",
            FullText = this.txtNote.Text,
            Created = DateTime.Now,
            ModBy = "slaing",
            IsActive = true,
            Key1 = "WPF Entry",
            Key2 = "",
            Salt = Guid.NewGuid().ToString()
        };

        bool success = vitalsBLL.AddNoteEntry(entry, _sessionKey, _token, out string addEntryErrors);
        if (success)
        {
            //MessageBox.Show("Note saved successfully.", Constants.AppGlobal.ApplicationName);
            this.txtNote.Clear();
        }
        else
        {
            MessageBox.Show($"Trouble saving note entry: {addEntryErrors}", Constants.AppGlobal.ApplicationName);
        }
        this.Close();
    }

    private void btnQuestions_Click(object sender, RoutedEventArgs e)
    {
        var info = vitalsBLL.GetQuestionsBySessionKey(_sessionKey, _token, false, out string memberInfoErrors);
    }
}
