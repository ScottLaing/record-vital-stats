using RecordMyStats.Common.Entities;

namespace RecordMyStats.Common.Dto
{
    public class MemberResultDto
    {
        public Member? Member { get; set; }
        public string? Errors { get; set; }
        public bool Result { get; set; }
    }
}
