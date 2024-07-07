using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordMyStats.Common.Entities
{
    public class BloodPressure
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public DateTime RecordingDate { get; set; }
        public double Systolic { get; set; }
        public double Diastolic { get; set; }
        public string Units { get; set; } = "";
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public string WhenTaken { get; set; } = "";
    }
}
