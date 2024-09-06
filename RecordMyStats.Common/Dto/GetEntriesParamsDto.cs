namespace RecordMyStats.Common.Dto
{
    public class GetEntriesParamsDto
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string? SessionKey { get; set; }      
    }
}
