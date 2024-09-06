using RecordMyStats.Common.Entities;

namespace RecordMyStats.Common.Dto
{
    public class GetEntriesResultDto
    {
        public List<StatisticEntry>? Entries { get; set; }
        public string? Errors { get; set; }
        public bool Result { get; set; }
    }
}
