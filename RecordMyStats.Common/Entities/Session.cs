namespace RecordMyStats.Common.Entities
{
    public class Session
    {
        public int SessionId { get; set; }
        public int MemberId { get; set; }
        public string? SessionKey { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpiresDate { get; set; }
        public string? Platform { get; set; }
        public bool IsActive { get; set; }

        public override string ToString()
        {
            return $"SessionId: {SessionId}, MemberId: {MemberId}, SessionKey: {SessionKey}, CreateDate: {CreateDate}, ExpiresDate: {ExpiresDate}";
        }
    }
}
