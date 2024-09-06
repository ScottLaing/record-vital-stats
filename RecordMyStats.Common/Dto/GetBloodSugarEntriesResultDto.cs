using RecordMyStats.Common.Entities;

namespace RecordMyStats.Common.Dto
{
    public class GetBloodSugarEntriesResultDto
    {
        public List<BloodSugar>? Entries { get; set; }
        public string? Errors { get; set; }
        public bool Result { get; set; }
    }
}
