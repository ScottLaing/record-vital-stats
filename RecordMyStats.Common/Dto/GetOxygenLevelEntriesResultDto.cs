using RecordMyStats.Common.Entities;

namespace RecordMyStats.Common.Dto
{
    public class GetOxygenLevelEntriesResultDto
    {
        public List<OxygenLevel>? Entries { get; set; }
        public string? Errors { get; set; }
        public bool Result { get; set; }
    }
}
