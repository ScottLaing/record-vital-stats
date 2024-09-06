using RecordMyStats.Common.Entities;

namespace RecordMyStats.Common.Dto
{
    public class GetQuestionsResultDto
    {
        public List<Question>? Questions { get; set; }
        public string? Errors { get; set; }
        public bool Result { get; set; }
    }
}
