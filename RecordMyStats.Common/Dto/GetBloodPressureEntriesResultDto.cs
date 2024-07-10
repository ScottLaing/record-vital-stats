using RecordMyStats.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordMyStats.Common.Dto
{
    public class GetBloodPressureEntriesResultDto
    {
        public List<BloodPressure>? Entries { get; set; }
        public string? Errors { get; set; }
        public bool Result { get; set; }
    }
}
