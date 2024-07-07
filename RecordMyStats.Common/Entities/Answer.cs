using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordMyStats.Common.Entities
{
    public class Answer
    {
        public int Id { get; set; }
        public string AnswerText { get; set; } = "";
        public int QuestionId { get; set; } = -1;
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; } = "";
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; } = "";
        public bool IsCorrect { get; set; } = false;

        public override string ToString()
        {
            return $"AnswerText: {AnswerText}, IsCorrect: {IsCorrect}, IsActive: {IsActive}";
        }
    }
}
