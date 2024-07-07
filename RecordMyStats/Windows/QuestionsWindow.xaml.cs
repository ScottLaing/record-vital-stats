using Newtonsoft.Json.Linq;
using System.Drawing;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace RecordMyStats.Windows;

/// <summary>
/// code behind for <see cref="BloodSugarWindow"/>
/// </summary>
public partial class QuestionsWindow : Window
{
    private string _sessionKey;
    private string _fullName;
    private string _token;
    private IVitalsBLL vitalsBLL = VitalsFactory.GetVitalsBLL();
    List<Question>? _questions;
    Question _currentQuestion = new Question();
    int _currentQuestionOffset;

    private Random rnd = new Random();


    public SolidColorBrush Red
    {
        get
        {
            return new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 128, 128));
        }
    }

    public SolidColorBrush Green
    {
        get
        {
            return new SolidColorBrush(System.Windows.Media.Color.FromRgb(128, 255, 128));
        }
    }

    public QuestionsWindow(string sessionKey, string fullName, string token)
    {
        InitializeComponent();
        GlobalUISettings.AddToWindowsList(this);

        ucMainMenu.SessionKey = sessionKey;
        ucMainMenu.FullName = fullName;
        ucMainMenu.ParentWindow = this as Window;
        ucMainMenu.Token = token;

        this.Title = Constants.AppGlobal.ApplicationName + " - Cert Questions";
        _sessionKey = sessionKey;
        _fullName = fullName;
        _token = token;

        bool errorFound = RefreshQuestionsAndPointToFirstQuestion();
    }

    private bool RefreshQuestionsAndPointToFirstQuestion()
    {
        bool errorFnd = false;
        bool random = this.chkRandomOrder.IsChecked ?? false;
        var questions = vitalsBLL.GetQuestionsBySessionKey(_sessionKey, _token, random, out string questionErrors);

        _questions = questions;

        if (_questions == null)
        {
            MessageBox.Show("logic error - questions not defined");
            errorFnd = true;
        }
        else if (!_questions.Any())
        {
            MessageBox.Show("no questions in list");
            errorFnd = true;
        }
        else
        {
            _currentQuestionOffset = 0;
            _currentQuestion = _questions[_currentQuestionOffset];

            UpdateChoices();
        }

        return errorFnd;
    }

    private object myRandom(Question q)
    {
        int num = rnd.Next();
        return num;
    }

    private void btnSubmit_Click(object sender, RoutedEventArgs e)
    {
        var singleAnswerNumber = _currentQuestion.AnswerNumber;

        _currentQuestion.Submitted = true;

        var multipleAnswers = _currentQuestion.MultipleAnswers;
        int myChoice = -1;
        var myChoices = new List<int>();

        if (multipleAnswers)
        {
            if (this.cbAnswer1.IsChecked ?? false) 
            {
                myChoices.Add(1);
            }
            if (this.cbAnswer2.IsChecked ?? false)
            {
                myChoices.Add(2);
            }
            if (this.cbAnswer3.IsChecked ?? false)
            {
                myChoices.Add(3);
            }
            if (this.cbAnswer4.IsChecked ?? false)
            {
                myChoices.Add(4);
            }
            _currentQuestion.ChoicesMade = myChoices;
            _currentQuestion.Correct = AreListsEqual(myChoices, _currentQuestion.MultipleAnswerNumbers);
            if (_currentQuestion.Correct ?? false)
            {
                tbCorrect.Visibility = Visibility.Visible;
            }
            else
            {
                tbInCorrect.Visibility = Visibility.Visible;
            }
        }
        else
        {
            if (this.rbAnswer1.IsChecked ?? false)
            {
                myChoice = 1;
            }
            else if (this.rbAnswer2.IsChecked ?? false)
            {
                myChoice = 2;
            }
            else if (this.rbAnswer3.IsChecked ?? false)
            {
                myChoice = 3;
            }
            else if (this.rbAnswer4.IsChecked ?? false)
            {
                myChoice = 4;
            }
            _currentQuestion.ChoiceMade = myChoice;
            _currentQuestion.Correct = (myChoice == singleAnswerNumber);
            if (_currentQuestion.Correct ?? false)
            {
                tbCorrect.Visibility = Visibility.Visible;
            }
            else
            {
                tbInCorrect.Visibility = Visibility.Visible;
            }
        }

        //

        if (multipleAnswers)
        {
            var multAnswers = _currentQuestion.MultipleAnswerNumbers;
            if (multAnswers.Contains(1))
            {
                this.txtcbAnswer1.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(128, 255, 128));
            }
            if (multAnswers.Contains(2))
            {
                this.txtcbAnswer2.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(128, 255, 128));
            }
            if (multAnswers.Contains(3))
            {
                this.txtcbAnswer3.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(128, 255, 128));
            }
            if (multAnswers.Contains(4))
            {
                this.txtcbAnswer4.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(128, 255, 128));
            }
        }
        else 
        {
            if (singleAnswerNumber == 1)
            {
                this.txtrbAnswer1.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(128, 255, 128));
            }
            else if (singleAnswerNumber == 2)
            {
                this.txtrbAnswer2.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(128, 255, 128));
            }
            else if (singleAnswerNumber == 3)
            {
                this.txtrbAnswer3.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(128, 255, 128));
            }
            else if (singleAnswerNumber == 4)
            {
                this.txtrbAnswer4.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(128, 255, 128));
            }
            else
            {
                MessageBox.Show("trouble getting answer from table");
            }

            if (_currentQuestion.Correct ?? false)
            {

            }
            else
            {
                if (myChoice == 1)
                {
                    this.txtrbAnswer1.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 128, 128));
                }
                else if (myChoice == 2)
                {
                    this.txtrbAnswer2.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 128, 128));
                }
                else if (myChoice == 3)
                {
                    this.txtrbAnswer3.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 128, 128));
                }
                else if (myChoice == 4)
                {
                    this.txtrbAnswer4.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 128, 128));
                }
            }
        }

        this.txtExplanation.Text = _currentQuestion.ExplanationText;
    }

    private bool AreListsEqual(List<int> myChoices, List<int> multipleAnswerNumbers)
    {
        if (myChoices == null || multipleAnswerNumbers == null)
        {
            return false;
        }

        var sortedA = myChoices.OrderBy(c => c).ToList();
        var sortedB = multipleAnswerNumbers.OrderBy(c => c).ToList();

        bool result = Enumerable.SequenceEqual(sortedA, sortedB);
        return result;
    }

    private void btnNext_Click(object sender, RoutedEventArgs e)
    {

        if (_questions == null)
        {
            MessageBox.Show("questions not defined");
            return;
        }

        tbCorrect.Visibility = Visibility.Hidden;
        tbInCorrect.Visibility = Visibility.Hidden;


        if (_currentQuestionOffset + 2 > _questions.Count)
        {
            MessageBox.Show("end of questions");
            return;
        }
        _currentQuestionOffset++;

        _currentQuestion = _questions[_currentQuestionOffset];

        UpdateChoices();
    }

    private void UpdateChoices()
    {
        tbCorrect.Visibility = Visibility.Hidden;
        tbInCorrect.Visibility = Visibility.Hidden;

        ClearBackgrounds();
        ClearAnswers();

        bool multipleAnswers = _currentQuestion.MultipleAnswers;
        ToggleRadionAndCBVisibility(multipleAnswers);

        UnCheckAllAnswers();

        if (_questions == null || !_questions.Any())
        {
            MessageBox.Show("questions null or no questions defined.");
            return;
        }

        if (_currentQuestion == null)
        {
            MessageBox.Show("no current question defined, possible logic error.");
            return;
        }

        this.lblIdLabel.Content = $"Id: {_currentQuestion.Id}";

        // update basic question information
        this.txtQuestion.Text = _currentQuestion.QuestionText;

        if (this.chkShowExplanations.IsChecked ?? false)
        {
            this.txtExplanation.Text = _currentQuestion.ExplanationText;
        }
        else
        {
            this.txtExplanation.Text = "";
        }

        if (_currentQuestion.AnswersList == null)
        {
            MessageBox.Show("no answers list for this question, data or retrieval error");
            return;
        }

        var al = _currentQuestion.AnswersList;
        if (al == null || al.Count < 4)
        {
            MessageBox.Show("answers null or less than 4 answers, possible data or logic error with this question.");
            return;
        }

        AddAnswersToUIControls(multipleAnswers, al);

        if (_currentQuestion.Submitted)
        {
            ResetSubmittedChoices();
        }
    }

    private void AddAnswersToUIControls(bool multipleAnswers, List<Answer> al)
    {
        if (multipleAnswers)
        {
            this.txtcbAnswer1.Text = al[0].AnswerText ?? "";
            this.txtcbAnswer2.Text = al[1].AnswerText ?? "";
            this.txtcbAnswer3.Text = al[2].AnswerText ?? "";
            this.txtcbAnswer4.Text = al[3].AnswerText ?? "";
        }
        else
        {
            this.txtrbAnswer1.Text = al[0].AnswerText ?? "";
            this.txtrbAnswer2.Text = al[1].AnswerText ?? "";
            this.txtrbAnswer3.Text = al[2].AnswerText ?? "";
            this.txtrbAnswer4.Text = al[3].AnswerText ?? "";
        }
    }

    private void UnCheckAllAnswers()
    {
        this.rbAnswer1.IsChecked = false;
        this.rbAnswer2.IsChecked = false;
        this.rbAnswer3.IsChecked = false;
        this.rbAnswer4.IsChecked = false;


        this.cbAnswer1.IsChecked = false;
        this.cbAnswer2.IsChecked = false;
        this.cbAnswer3.IsChecked = false;
        this.cbAnswer4.IsChecked = false;
    }

    private void ToggleRadionAndCBVisibility(bool showMult)
    {
        this.rbAnswer1.Visibility = showMult ? Visibility.Hidden : Visibility.Visible;
        this.rbAnswer2.Visibility = showMult ? Visibility.Hidden : Visibility.Visible;
        this.rbAnswer3.Visibility = showMult ? Visibility.Hidden : Visibility.Visible;
        this.rbAnswer4.Visibility = showMult ? Visibility.Hidden : Visibility.Visible;

        this.cbAnswer1.Visibility = showMult ? Visibility.Visible : Visibility.Hidden;
        this.cbAnswer2.Visibility = showMult ? Visibility.Visible : Visibility.Hidden;
        this.cbAnswer3.Visibility = showMult ? Visibility.Visible : Visibility.Hidden;
        this.cbAnswer4.Visibility = showMult ? Visibility.Visible : Visibility.Hidden;

        if (showMult)
        {
            this.cbAnswer1.Focus();
        }
        else
        {
            this.rbAnswer1.Focus();
        }
    }

    private void ClearAnswers()
    {
        this.txtcbAnswer1.Text = "";
        this.txtcbAnswer2.Text = "";
        this.txtcbAnswer3.Text = "";
        this.txtcbAnswer4.Text = "";

        this.txtrbAnswer1.Text = "";
        this.txtrbAnswer2.Text = "";
        this.txtrbAnswer3.Text = "";
        this.txtrbAnswer4.Text = "";
    }

    private void ClearBackgrounds()
    {
        this.txtrbAnswer1.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));
        this.txtrbAnswer2.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));
        this.txtrbAnswer3.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));
        this.txtrbAnswer4.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));

        this.txtcbAnswer1.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));
        this.txtcbAnswer2.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));
        this.txtcbAnswer3.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));
        this.txtcbAnswer4.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));
    }

    private void ResetSubmittedChoices()
    {
        if (_currentQuestion == null)
        {
            MessageBox.Show("no current question selected, logic error");
            return;
        }

        // show explanation
        this.txtExplanation.Text = _currentQuestion.ExplanationText ?? "";
        if (_currentQuestion.Correct != null)
        {
            if (_currentQuestion.Correct.Value)
            {
                tbCorrect.Visibility = Visibility.Visible;
            }
            else
            {
                tbInCorrect.Visibility = Visibility.Visible;
            }
        }

        bool multAnsw = _currentQuestion.MultipleAnswers;

        ReselectAnswers(multAnsw);

        // mult choice
        if (multAnsw)
        {

            var multAnswerNums = _currentQuestion.MultipleAnswerNumbers;
            if (multAnswerNums == null)
            {
                MessageBox.Show("multiple answers, data does not have a valid answers list");
                return;
            }

            // show correct choice with green background
            if ( multAnswerNums.Contains(1))
            {
                this.txtcbAnswer1.Background = Green;
            }
            if (multAnswerNums.Contains(2))
            {
                this.txtcbAnswer2.Background = Green;
            }
            if (multAnswerNums.Contains(3))
            {
                this.txtcbAnswer3.Background = Green;
            }
            if (multAnswerNums.Contains(4))
            {
                this.txtcbAnswer4.Background = Green;
            }

            if (_currentQuestion.Correct == null)
            {
                return;
            }

            var correct = _currentQuestion.Correct.Value;

            if ((!correct))
            {
                var userChoises = _currentQuestion.ChoicesMade;

                if (userChoises.Contains(1) && ! multAnswerNums.Contains(1))
                {
                    this.txtcbAnswer1.Background = Red;
                }
                if (userChoises.Contains(2) && !multAnswerNums.Contains(2))
                {
                    this.txtrbAnswer2.Background = Red;
                }
                if (userChoises.Contains(3) && !multAnswerNums.Contains(3))
                {
                    this.txtrbAnswer3.Background = Red;
                }
                if (userChoises.Contains(4) && !multAnswerNums.Contains(4))
                {
                    this.txtrbAnswer4.Background = Red;
                }
            }
        }
        else 
        { 
            // show correct choice with green background
            if (_currentQuestion.AnswerNumber == 1)
            {
                this.txtrbAnswer1.Background = Green;
            }
            else if (_currentQuestion.AnswerNumber == 2)
            {
                this.txtrbAnswer2.Background = Green;
            }
            else if (_currentQuestion.AnswerNumber == 3)
            {
                this.txtrbAnswer3.Background = Green;
            }
            else if (_currentQuestion.AnswerNumber == 4)
            {
                this.txtrbAnswer4.Background = Green;
            }

            var correct = _currentQuestion.Correct ?? false;

            if (_currentQuestion.ChoiceMade.HasValue && _currentQuestion.ChoiceMade != -1 && (!correct))
            {
                int choiceMade = _currentQuestion.ChoiceMade ?? -1;

                if (choiceMade == 1)
                {
                    this.txtrbAnswer1.Background = Red;
                }
                else if (choiceMade == 2)
                {
                    this.txtrbAnswer2.Background = Red;
                }
                else if (choiceMade == 3)
                {
                    this.txtrbAnswer3.Background = Red;
                }
                else if (choiceMade == 4)
                {
                    this.txtrbAnswer4.Background = Red;
                }
            }
        }
    }

    private void ReselectAnswers(bool multAnsw)
    {
        // single choice answer, set the is checked to what they chose
        if (!multAnsw && _currentQuestion.ChoiceMade != -1 && _currentQuestion.ChoiceMade != null)
        {
            if (_currentQuestion.ChoiceMade == 1)
            {
                this.rbAnswer1.IsChecked = true;
            }
            else if (_currentQuestion.ChoiceMade == 2)
            {
                this.rbAnswer2.IsChecked = true;
            }
            else if (_currentQuestion.ChoiceMade == 3)
            {
                this.rbAnswer3.IsChecked = true;
            }
            else if (_currentQuestion.ChoiceMade == 4)
            {
                this.rbAnswer4.IsChecked = true;
            }
        }
        else if (multAnsw && _currentQuestion.ChoicesMade != null && _currentQuestion.ChoicesMade.Any())
        {
            var csm = _currentQuestion.ChoicesMade;
            if (csm.Contains(1))
            {
                this.cbAnswer1.IsChecked = true;
            }
            if (csm.Contains(2))
            {
                this.cbAnswer2.IsChecked = true;
            }
            if (csm.Contains(3))
            {
                this.cbAnswer3.IsChecked = true;
            }
            if (csm.Contains(4))
            {
                this.cbAnswer4.IsChecked = true;
            }
        }
    }

    private void btnPrev_Click(object sender, RoutedEventArgs e)
    {

        if (_questions == null)
        {
            MessageBox.Show("questions not defined");
            return;
        }
        if (_currentQuestionOffset == 0)
        {
            MessageBox.Show("already at first question");
            return;
        }

        tbCorrect.Visibility = Visibility.Hidden;
        tbInCorrect.Visibility = Visibility.Hidden;

        _currentQuestionOffset--;

        _currentQuestion = _questions[_currentQuestionOffset];

        UpdateChoices();

    }

    private void btnImportFromFile_Click(object sender, RoutedEventArgs e)
    {
        var utils = new AddQuestionsUtils(vitalsBLL, _sessionKey, _token);
        utils.ImportQuestionsFromFiles();
    }

    private void btnRedoQuestions_Click(object sender, RoutedEventArgs e)
    {
        RefreshQuestionsAndPointToFirstQuestion();

        if (_questions != null)
        {
            _currentQuestionOffset = _questions.Count - 1;
            _currentQuestion = _questions[_currentQuestionOffset];

            UpdateChoices();
        }
    }

    private void btnUpdateQuestion_Click(object sender, RoutedEventArgs e)
    {
        _currentQuestion.QuestionText = this.txtQuestion.Text;
        bool res = vitalsBLL.UpdateQuestion(_currentQuestion, _sessionKey, _token, out string errors);
        if (res)
        {
            MessageBox.Show("Question text updated");
        }
    }

    private void btnUpdateExplanation_Click(object sender, RoutedEventArgs e)
    { 
        _currentQuestion.ExplanationText = this.txtExplanation.Text;
        bool res = vitalsBLL.UpdateQuestion(_currentQuestion, _sessionKey, _token, out string errors);
        if (res)
        {
            MessageBox.Show("Explanation text updated");
        }
    }

    private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == Key.Right && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))) // The Arrow-Down key
        {
            this.btnNext_Click(this.btnNext, new RoutedEventArgs());
        }
        if (e.Key == Key.Left && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))) // The Arrow-Down key
        {
            this.btnPrev_Click(this.btnPrev, new RoutedEventArgs());
        }
    }
}
