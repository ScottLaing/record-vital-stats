using RecordMyStats.Common.Entities;

namespace RecordMyStats.Common.Dto
{
    public class AddBloodSugarEntryDto
    {
        public BloodSugar? BloodSugarEntry { get; set; }
        public string? SessionKey { get; set; }      
    }
}
