using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RecordMyStats.Common.Constants;

namespace RecordMyStats.Common.Entities
{
    public class BloodPressure
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public double Systolic { get; set; }
        public double Diastolic { get; set; }
        public string Units { get; set; } = "";

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

        public string ValueDisplay
        {
            get
            {
                string bloodPressureDisplay = string.Format("{0}/{1} {2}", (int)Systolic, (int)Diastolic, Units);
                return bloodPressureDisplay;
            }
        }

        public string Comments { get; set; } = "";
        public string WhenTaken { get; set; } = "";
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
