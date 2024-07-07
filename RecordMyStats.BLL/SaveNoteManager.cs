using RecordMyStats.BLL;
using RecordMyStats.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple
{
    public class SaveNoteManager : ISaveNoteManager
    {
        private static string sessionKey = "";
        private static string token = "";

        public string SaveNote(string noteText, string user = "", string password = "")
        {
            if (!string.IsNullOrEmpty(user))
            {
                bool success1 = LoginMemberHandler.LoginMember(user, password, out string sessionKey1, out string fullName, out string token1, out string errors);
                if (!success1)
                {
                    return "error logging in: " + errors;
                }
                sessionKey = sessionKey1;
                token = token1;
            }

            if (string.IsNullOrWhiteSpace(sessionKey) || string.IsNullOrWhiteSpace(token))
            {
                return "no session key or token, login is needed";
            }

            Note entry = new Note()
            {
                Description = "",
                FullText = noteText,
                Created = DateTime.Now,
                ModBy = "slaing",
                IsActive = true,
                Key1 = "SMS",
                Key2 = "",
                Salt = Guid.NewGuid().ToString()
            };

            bool success2 = AddNoteEntryHandler.AddNoteEntry(entry, sessionKey, token, out string addEntryErrors);
            if (success2)
            {
                //
            }
            else
            {
                return "error adding the note: " + addEntryErrors;
            }
            return "";
        }
    } 
}
