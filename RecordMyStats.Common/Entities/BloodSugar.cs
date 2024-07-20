namespace RecordMyStats.Common.Entities
{
    public class BloodSugar
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public double Value { get; set; }
        public string Units { get; set; } = "";
        public string WhenTaken { get; set; } = "";
        public DateTime CreateDate { get; set; }
        public int Mood { get; set; }
        public string Comments { get; set; } = "";
        public DateTime RecordingDate { get; set; }
        public bool IsActive { get; set; }
    }
}
