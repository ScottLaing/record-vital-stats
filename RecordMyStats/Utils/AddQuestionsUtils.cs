using System.IO;

namespace RecordMyStats.Utils
{
    public class AddQuestionsUtils
    {
        private const string ImportFolder = "C:\\Users\\titan\\OneDrive\\Documents\\import-files";
        private IVitalsBLL _vitalsBLL;
        private string _sessionKey;
        private string _token;

        public AddQuestionsUtils(IVitalsBLL vitalsBLL, string sessionKey, string token)
        {
            _vitalsBLL = vitalsBLL;
            _sessionKey = sessionKey;
            _token = token;
        }

        private void SaveQuestion(StringBuilder sbQuestion, StringBuilder answerQuestion, StringBuilder explanation)
        {
            if (sbQuestion.Length == 0)
            {
                return;
            }

            var parts = answerQuestion.ToString().Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var answers = new List<Answer>();
            int cnt = 0;
            foreach (var part in parts)
            {
                var answer = new Answer();
                var part_tr = part.Trim();

                var textPart = part_tr.Replace("FALSE", "");
                textPart = textPart.Replace("TRUE", "");
                textPart = textPart.Trim();
                answer.AnswerText = textPart;

                if (part_tr.EndsWith("TRUE"))
                {
                    answer.IsCorrect = true;
                }
                else
                {
                    answer.IsCorrect = false;
                    cnt++;
                }
                answers.Add(answer);
            }
            var question = new Question();
            question.QuestionText = sbQuestion.ToString().Trim();
            question.ExplanationText = explanation.ToString().Trim();
            question.AnswersList = answers;
            question.MultipleAnswers = (cnt >= 2);

            var questions = _vitalsBLL.AddQuestionBySessionKey(question, _sessionKey, _token, out string questionAddError);
        }

        public void ImportQuestionsFromFiles()
        {
            var files = System.IO.Directory.GetFiles(ImportFolder, "*.txt");
            foreach (var file in files)
            {
                string[] lines = File.ReadAllLines(file);
                var sbQuestion = new StringBuilder();
                var answerQuestion = new StringBuilder();
                var explanation = new StringBuilder();
                bool inQuestion = false;
                bool inAnswer = false;
                bool inExplanation = false;
                int saveCnt = 0;
                foreach (var line1 in lines)
                {
                    string line = line1.Trim();
                    if (line.Length == 0 && !inExplanation)
                    {
                        continue;
                    }

                    if (inQuestion)
                    {
                        if (line.Contains("ANSWER"))
                        {
                            inQuestion = false;
                            inAnswer = true;
                            continue;
                        }
                        else
                        {
                            sbQuestion.AppendLine(line);
                        }
                    }
                    if (inAnswer)
                    {
                        if (line.Contains("EXPLANATION") || line.Contains("MOREE"))
                        {
                            inQuestion = false;
                            inAnswer = false;
                            inExplanation = true;
                            continue;
                        }
                        else
                        {
                            answerQuestion.AppendLine(line);
                        }

                    }
                    if (inExplanation)
                    {
                        if (line.Contains("QUESTION"))
                        {
                            if (sbQuestion.Length > 0 && answerQuestion.Length > 0 && explanation.Length > 0)
                            {
                                SaveQuestion(sbQuestion, answerQuestion, explanation);
                                saveCnt++;
                            }
                            inQuestion = true;
                            inExplanation = false;
                            sbQuestion.Clear();
                            answerQuestion.Clear();
                            explanation.Clear();
                            continue;
                        }
                        else
                        {
                            explanation.AppendLine(line);
                        }
                    }

                    if (line.Contains("QUESTION"))
                    {
                        if (sbQuestion.Length > 0 && answerQuestion.Length > 0 && explanation.Length > 0)
                        {
                            SaveQuestion(sbQuestion, answerQuestion, explanation);
                            saveCnt++;
                        }
                        inQuestion = true;
                        inExplanation = false;
                    }
                }
                if (inExplanation)
                {
                    if (sbQuestion.Length > 0 && answerQuestion.Length > 0 && explanation.Length > 0)
                    {
                        SaveQuestion(sbQuestion, answerQuestion, explanation);
                        saveCnt++;
                    }
                }

                MessageBox.Show($"{saveCnt} new question(s) added to repository");
            }
        }
    }
}
