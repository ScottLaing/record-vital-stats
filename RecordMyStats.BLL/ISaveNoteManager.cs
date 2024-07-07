using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple
{
    public interface ISaveNoteManager
    {
        public string SaveNote(string noteText, string user = "", string password = "");
    }
}
