namespace RecordMyStats.Common.Entities
{
    public class Note
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string Description { get; set; } = "";
        public string FullText { get; set; } = "";
        public DateTime Created { get; set; } = DateTime.Now;
        public string ModBy { get; set; } = "";
        public bool IsActive { get; set; }
        public string? Key1 { get; set; }
        public string? Key2 { get; set; }
        public string? Salt { get; set; }
        public bool? IsEncrypted { get; set; }
    }
}
