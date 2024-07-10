using RecordMyStats.Utils;
using RecordMyStats.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RecordMyStats.UserControls
{
    /// <summary>
    /// Interaction logic for MainMenuControl.xaml
    /// </summary>
    public partial class MainMenuControl : UserControl
    {
        public string SessionKey { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;

        public Window? ParentWindow { get; set; } = null;

        public MainMenuControl()
        {
            InitializeComponent();
        }

        private void mniLogout_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            GlobalUISettings.CloseWindows();
        }

        private void mniBloodSugar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SessionKey))
            {
                MessageBox.Show("trouble getting session key from parent window");
                return;
            }
            var bloodSugarWindow = new BloodSugarWindow(SessionKey, FullName, Token);
            bloodSugarWindow.Show();
        }

        private void mniRecordStats_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SessionKey))
            {
                MessageBox.Show("trouble getting session key from parent window");
                return;
            }
            var recordStatsWindow = new RecordStatsWindow(SessionKey, FullName, Token);
            recordStatsWindow.Show();
        }

        private void mniViewStats_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SessionKey))
            {
                MessageBox.Show("trouble getting session key from parent window");
                return;
            }
            var statsViewWindow = new StatsViewWindow(SessionKey, FullName, Token);
            statsViewWindow.Show();
        }
        
        private void mniBloodSugarView_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SessionKey))
            {
                MessageBox.Show("trouble getting blood sugar session key from parent window");
                return;
            }
            var bloodSugarViewWindow = new BloodSugarViewWindow(SessionKey, FullName, Token);
            bloodSugarViewWindow.Show();
        }

        private void mniExit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void mniProfile_Click(object sender, RoutedEventArgs e)
        {
            var myProfileWindow = new MyProfileWindow(SessionKey, FullName, Token);
            myProfileWindow.Show();
        }

        private void mniNote_Click(object sender, RoutedEventArgs e)
        {
            var noteWindow = new NoteWindow(SessionKey, FullName, Token);
            noteWindow.Show();
        }

        private void mniViewNotes_Click(object sender, RoutedEventArgs e)
        {
            var notesViewWindow = new NotesViewWindow(SessionKey, FullName, Token);
            notesViewWindow.Show();
            
        }

        private void mniQuestions_Click(object sender, RoutedEventArgs e)
        {
            var questionsWindow = new QuestionsWindow(SessionKey, FullName, Token);
            questionsWindow.Show();

        }

        private void mniBloodPressure_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SessionKey))
            {
                MessageBox.Show("trouble getting session key from parent window");
                return;
            }
            var bloodPressureWindow = new BloodPressureWindow(SessionKey, FullName, Token);
            bloodPressureWindow.Show();
        }

        private void mniBloodPressureView_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SessionKey))
            {
                MessageBox.Show("trouble getting blood sugar session key from parent window");
                return;
            }
            var bloodPressureViewWindow = new BloodPressureViewWindow(SessionKey, FullName, Token);
            bloodPressureViewWindow.Show();
        }
    }
}
