using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordMyStats.Common.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public string QuestionText { get; set; } = "";
        public string ExplanationText { get; set; } = "";
        public bool IsActive { get; set; }

        public bool MultipleAnswers { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; } = "";
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; } = "";

        // not from table but to track user's behaviors during quizzing, these are settable by the quiz process,not static from table
        #region Quiz Properties

        public bool Submitted { get; set; } = false;
        public int? ChoiceMade { get; set; } = null;
        public List<int> ChoicesMade { get; set; } = new List<int>();
        public bool? Correct { get; set; } = null;

        #endregion

        #region Calculated Answers Properties

        public List<Answer>? AnswersList { get; set; }

        // for questions with multiple answers (multiple choice)
        private List<int>? _multipleAnswerNumbers = null;

        public List<int> MultipleAnswerNumbers
        {
            get
            {
                if (_multipleAnswerNumbers != null)
                {
                    return _multipleAnswerNumbers;
                }

                var al = AnswersList;
                if (al == null || !al.Any())
                {
                    return new List<int>();
                }

                var result = new List<int>();
                if (al != null && al.Count >= 4)
                {
                    if (al[0].IsCorrect)
                    {
                        result.Add(1);
                    }
                    if (al[1].IsCorrect)
                    {
                        result.Add(2);
                    }
                    if (al[2].IsCorrect)
                    {
                        result.Add(3);
                    }
                    if (al[3].IsCorrect)
                    {
                        result.Add(4);
                    }
                }
                _multipleAnswerNumbers = result;
                return result;
            }
        }

        // for questions with one answer
        private int? _answerNumber = null;

        public int AnswerNumber
        {
            get
            {
                if (_answerNumber != null)
                {
                    return _answerNumber.Value;
                }

                var al = AnswersList;
                if (al == null || ! al.Any())
                {
                    return -1;
                }

                int result = -1;
                if (al != null && al.Count >= 4)
                {
                    if (al[0].IsCorrect)
                    {
                        result = 1;
                    }
                    else if (al[1].IsCorrect)
                    {
                        result = 2;
                    }
                    else if (al[2].IsCorrect)
                    {
                        result = 3;
                    }
                    else if (al[3].IsCorrect)
                    {
                        result = 4;
                    }
                }
                _answerNumber = result;
                return result;
            }
        }

        #endregion

        public override string ToString()
        {
            return $"QuestionText: {QuestionText}, ExplanationText: {ExplanationText}, IsActive: {IsActive}";
        }
    }
}
