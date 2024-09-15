using static RecordMyStats.Common.Constants;

namespace RecordMyStats.Common.Entities
{
    public class OxygenLevel
    {
        public int Id { get; set; }

        public int MemberId { get; set; }

        public int OxygenValue { get; set; }

        public int HeartRate { get; set; } = 70;

        public DateTime RecordingDate { get; set; }

        public int Mood { get; set; }

        public string MoodDisplay
        {
            get
            {
                string mood = MoodMapDictionary.ContainsKey(Mood) ? MoodMapDictionary[Mood] : NoSelection;
                return mood;
            }
        }

        public string OxygenValueDisplay
        {
            get
            {
                string o2Display = string.Format("{0}%", (int)OxygenValue);
                return o2Display;
            }
        }

        public string Comments { get; set; } = "";
        public string WhenTaken { get; set; } = "";
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsActive { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, MemberId: {MemberId}, OxygenValue: {OxygenValue}, HeartRate: {HeartRate}, " +
                $"RecordingDate: {RecordingDate}, Mood: {Mood}, Comments: {Comments}, " +
                $"WhenTaken: {WhenTaken}, CreateDate: {CreateDate}, ModifiedDate: {ModifiedDate}, IsActive: {IsActive}";
        }
    }
}
