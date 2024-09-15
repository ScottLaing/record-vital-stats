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

        public override string ToString()
        {
            return $"Id: {Id}, MemberId: {MemberId}, Description: {Description}, " +
                $"FullText: {FullText}, Created: {Created}, ModBy: {ModBy}, IsActive: {IsActive}, Key1: {Key1}, Key2: {Key2}, " +
                $"Salt: {Salt}, IsEncrypted: {IsEncrypted}";
        }
    }
}
