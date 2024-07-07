using RecordMyStats.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordMyStats.Common.Dto
{
    public class AddEntryDto
    {
        public StatisticEntry? StatisticEntry { get; set; }
        public string? SessionKey { get; set; }      
    }
}
