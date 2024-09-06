using RecordMyStats.Common.Entities;

namespace RecordMyStats.Common.Dto
{
    public class AddOxygenLevelEntryDto
    {
        public OxygenLevel? OxygenLevelEntry { get; set; }
        public string? SessionKey { get; set; }      
    }
}
