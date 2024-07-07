using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RecordMyStats.Common.Entities
{
    public class QuestionLookupResult
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; } = "";
        public string ExplanationText { get; set; } = "";
        public bool QuestionIsActive { get; set; }
        public int AnswerId { get; set; }
        public string? AnswerText { get; set; }
        public bool MultipleAnswers { get; set; }

        public bool AnswerIsActive { get; set; }
        public bool AnswerIsCorrect { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; } = "";
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; } = "";
        private static Random rnd = new Random();

        public static List<Question> GetQuestionsFromQuestionResults(List<QuestionLookupResult> questionResults)
        {
            List<Question> result = new List<Question>();

            var linqGroups = questionResults.GroupBy(q => q.QuestionId);
            foreach (var group in linqGroups)
            {
                var first = group.FirstOrDefault();
                if (first == null) { continue;  }

                var question = first.toQuestion();
                List<Answer> answers = new List<Answer>();
                foreach (var answerDetail in group)
                {
                    Answer answer = answerDetail.toAnswer();
                    answers.Add(answer);
                }
                answers = answers.OrderBy(q => rnd.Next()).ToList();
                question.AnswersList = answers;
                result.Add(question);
            }

            return result;
        }

        private Answer toAnswer()
        {
            Answer answer = new Answer();
            answer.Id = this.AnswerId;
            answer.IsActive = this.AnswerIsActive;
            answer.AnswerText = this.AnswerText ?? "";
            answer.AnswerText = answer.AnswerText.Trim();
            answer.IsCorrect = this.AnswerIsCorrect;
            return answer;
        }

        public Question toQuestion()
        {
            var question = new Question();
            question.Id = this.QuestionId;
            question.QuestionText = this.QuestionText;
            question.ExplanationText = this.ExplanationText;
            question.IsActive = this.QuestionIsActive;
            question.MultipleAnswers = this.MultipleAnswers;
            question.CreatedDate = this.CreatedDate;
            question.CreatedDate = this.CreatedDate;
            question.ModifiedBy = this.ModifiedBy;
            question.ModifiedDate = this.ModifiedDate;
            return question;
        }
    }
}

 
