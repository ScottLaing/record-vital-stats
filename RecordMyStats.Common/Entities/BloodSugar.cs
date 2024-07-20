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

        public string MoodDisplay
        {
            get
            {
                string mood = "";
                switch (Mood)
                {
                    case 0:
                        mood = "No selection";
                        break;
                    case 1:
                        mood = "No selection";
                        break;
                    case 2:
                        mood = "Happy";
                        break;
                    case 3:
                        mood = "Sad";
                        break;
                    case 4:
                        mood = "Tired";
                        break;
                    default:
                        mood = "No selection";
                        break;
                }
                return mood;
            }
        }
        public string Comments { get; set; } = "";
        public DateTime RecordingDate { get; set; }
        public bool IsActive { get; set; }
    }
}
