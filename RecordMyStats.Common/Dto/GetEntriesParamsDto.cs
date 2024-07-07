using RecordMyStats.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordMyStats.Common.Dto
{
    public class GetEntriesParamsDto
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string? SessionKey { get; set; }      
    }
}
