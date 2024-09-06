using RecordMyStats.Common.Entities;

namespace RecordMyStats.Common.Dto
{
    public class AddEntryDto
    {
        public StatisticEntry? StatisticEntry { get; set; }
        public string? SessionKey { get; set; }      
    }
}
