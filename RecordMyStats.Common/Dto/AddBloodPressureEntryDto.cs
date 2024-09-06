using RecordMyStats.Common.Entities;

namespace RecordMyStats.Common.Dto
{
    public class AddBloodPressureEntryDto
    {
        public BloodPressure? BloodPressureEntry { get; set; }
        public string? SessionKey { get; set; }      
    }
}
