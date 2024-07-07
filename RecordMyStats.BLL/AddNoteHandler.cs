using Newtonsoft.Json;
using RecordMyStats.Common.Dto;
using RecordMyStats.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordMyStats.BLL
{
    public class AddNoteEntryHandler
    {
        public static bool AddNoteEntry(Note entry, string sessionKey, string token, out string errors)
        {
            // AddNoteEntry
            bool success = true;
            errors = "";
            var resultDto = new SimpleResultDto();
            var addNoteEntryDto = new AddNoteEntryDto();
            addNoteEntryDto.SessionKey = sessionKey;
            addNoteEntryDto.NoteEntry = entry;

            using (var client = new HttpClient())
            {
                try
                {
                    string returnResult = HttpUtils.SetupAndCallApi(client, true, addNoteEntryDto, "Entry/AddNoteEntry", token);

                    resultDto = JsonConvert.DeserializeObject<SimpleResultDto>(returnResult);
                    success = resultDto.Result;
                    errors = resultDto.Errors ?? "";
                }
                catch (Exception ex)
                {
                    success = false; // on error show email in use to block further continued logic with this email until issue is addressed (unclear on result)
                    errors = $"trouble adding note entry, error: {ex.Message}";
                }
            }

            return success;
        }
    }

}
