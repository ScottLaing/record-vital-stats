using RecordMyStats.Common.Entities;

namespace RecordMyStats.Common.Dto
{
    public class GetBloodPressureEntriesResultDto
    {
        public List<BloodPressure>? Entries { get; set; }
        public string? Errors { get; set; }
        public bool Result { get; set; }
    }
}
