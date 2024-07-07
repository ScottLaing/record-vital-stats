using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordMyStats.Common.Entities
{
    public class BloodSugar
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public double Value { get; set; }
        public string Units { get; set; } = "";
        public string WhenTaken { get; set; } = "";
        public string? Notes { get; set; }
        public int? MoodLevel { get; set; }
        public string? CustomTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime RecordingDate { get; set; }
    }
}
