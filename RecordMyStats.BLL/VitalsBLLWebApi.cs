using Newtonsoft.Json;
using RecordMyStats.Common;
using RecordMyStats.Common.Dto;
using RecordMyStats.Common.Entities;
using RecordMyStats.Common.Utility;
using RecordMyStats.DataAccess.Data.Vitals;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace RecordMyStats.BLL
{
    /// <summary>
    /// forwards business calls related to data to the web api calls, packages the parameters for the web api methods and then gets the results
    /// and unpacks them for the caller.
    /// </summary>
    public class VitalsBLLWebApi : IVitalsBLL
    {
        private IVitalsRepository repos = VitalsRepositoryFactory.GetVitalsRepository();
        private bool _encrypt = true;

        public bool LoginMember(string email, string password, out string sessionKey, out string fullName, out string token, out string errors)
        {
            return LoginMemberHandler.LoginMember(email, password, out sessionKey, out fullName, out token, out errors);
        }

        public bool AddMember(Member member, out string errors, out string newSessionKey, out string newToken)
        {
            bool success = true;
            newSessionKey = "";
            errors = "";
            newToken = "";
            var resultDto = new SessionResultDto();

            using (var client = new HttpClient())
            {
                try
                {
                    string returnResult = HttpUtils.SetupAndCallApi(client, false, member, "Member/AddMember", "");

                    resultDto = JsonConvert.DeserializeObject<SessionResultDto>(returnResult);
                    success = resultDto.Result;
                    errors = resultDto.Errors ?? "";
                    newToken = resultDto.Token ?? "";
                    newSessionKey = resultDto.SessionKey ?? "";

                }
                catch (Exception ex)
                {
                    success = false; // on error show email in use to block further continued logic with this email until issue is addressed (unclear on result)
                    errors = $"trouble adding member is in use result, error: {ex.Message}";
                }
            }

            return success;
        }

        public bool IsEmailInUse(string email, out string errors)
        {
            bool emailInUse = true;
            var resultDto = new SimpleResultDto();

            errors = "";
            using (var client = new HttpClient())
            {
                var encodedEmail = HttpUtility.HtmlEncode(email);
                try
                {
                    string returnResult = HttpUtils.SetupAndCallApiNoBody(client, false, "Member/IsEmailInUse", $"email={encodedEmail}", "");

                    resultDto = JsonConvert.DeserializeObject<SimpleResultDto>(returnResult);
                    emailInUse = resultDto.Result;
                }
                catch (Exception ex)
                {
                    emailInUse = true; // on error show email in use to block further continued logic with this email until issue is addressed (unclear on result)
                    errors = $"trouble retrieving email is in use result, error: {ex.Message}";
                }
            }
               
            return emailInUse;
        }
        public bool AddNoteEntry(Note entry, string sessionKey, string token, out string errors)
        {
            return AddNoteEntryHandler.AddNoteEntry(entry, sessionKey, token, out errors);
        }

        public bool AddEntry(StatisticEntry entry, string sessionKey, string token, out string errors)
        {
            bool success = true;
            errors = "";
            var resultDto = new SimpleResultDto();
            var addEntryDto = new AddEntryDto();
            addEntryDto.SessionKey = sessionKey;
            addEntryDto.StatisticEntry = entry;

            using (var client = new HttpClient())
            {
                try
                {
                    string returnResult = HttpUtils.SetupAndCallApi(client, true, addEntryDto, "Entry/AddEntry", token);

                    resultDto = JsonConvert.DeserializeObject<SimpleResultDto>(returnResult);
                    success = resultDto.Result;
                    errors = resultDto.Errors ?? "";
                }
                catch (Exception ex)
                {
                    success = false; // on error show email in use to block further continued logic with this email until issue is addressed (unclear on result)
                    errors = $"trouble adding statistics entry, error: {ex.Message}";
                }
            }

            return success;
        }

        public bool AddBloodSugarEntry(BloodSugar entry, string sessionKey, string token, out string errors)
        {
            bool success = true;
            errors = "";
            var resultDto = new SimpleResultDto();
            var addEntryDto = new AddBloodSugarEntryDto();
            addEntryDto.SessionKey = sessionKey;
            addEntryDto.BloodSugarEntry = entry;

            using (var client = new HttpClient())
            {
                try
                {
                    string returnResult = HttpUtils.SetupAndCallApi(client, true, addEntryDto, "Entry/AddBloodSugarEntry", token);

                    resultDto = JsonConvert.DeserializeObject<SimpleResultDto>(returnResult);
                    success = resultDto.Result;
                    errors = resultDto.Errors ?? "";
                }
                catch (Exception ex)
                {
                    success = false; // on error show email in use to block further continued logic with this email until issue is addressed (unclear on result)
                    errors = $"trouble adding blood sugar entry, error: {ex.Message}";
                }
            }

            return success;
        }

        public bool AddBloodPressureEntry(BloodPressure bp, string sessionKey, string token, out string errors)
        {
            bool success = true;
            errors = "";
            var resultDto = new SimpleResultDto();
            var addEntryDto = new AddBloodPressureEntryDto();
            addEntryDto.SessionKey = sessionKey;
            addEntryDto.BloodPressureEntry = bp;

            using (var client = new HttpClient())
            {
                try
                {
                    string returnResult = HttpUtils.SetupAndCallApi(client, true, addEntryDto, "Entry/AddBloodPressureEntry", token);

                    resultDto = JsonConvert.DeserializeObject<SimpleResultDto>(returnResult);
                    success = resultDto.Result;
                    errors = resultDto.Errors ?? "";
                }
                catch (Exception ex)
                {
                    success = false; // on error show email in use to block further continued logic with this email until issue is addressed (unclear on result)
                    errors = $"trouble adding blood pressure entry, error: {ex.Message}";
                }
            }

            return success;
        }

        public List<BloodSugar>? GetBloodSugarEntriesBySessionKey(string sessionKey, string token, out string errors)
        {
            bool wereEntriesFound = false;
            var resultDto = new GetBloodSugarEntriesResultDto();

            List<BloodSugar>? entriesFound = new List<BloodSugar>();

            errors = "";
            using (var client = new HttpClient())
            {
                var encodedSession = HttpUtility.HtmlEncode(sessionKey);
                try
                {
                    string returnResult = HttpUtils.SetupAndCallApiNoBody(client, true, "Entry/GetBloodSugarEntriesBySessionKey", $"sessionKey={encodedSession}", token);

                    resultDto = JsonConvert.DeserializeObject<GetBloodSugarEntriesResultDto>(returnResult);
                    wereEntriesFound = resultDto.Result;
                    entriesFound = resultDto.Entries;
                    errors = resultDto.Errors ?? "";

                }
                catch (Exception ex)
                {
                    wereEntriesFound = false;
                    errors = $"trouble getting statistic entries, error: {ex.Message}";
                }
            }

            return entriesFound;
        }
        

        public List<Note>? GetNoteEntriesByRange(string sessionKey, DateTime fromDate, DateTime toDate, string token, out string errors)
        {
            bool wereEntriesFound = false;
            var resultDto = new GetNoteEntriesResultDto();

            List<Note>? entriesFound = new List<Note>();
            var entryParams = new GetNoteEntriesParamsDto();
            entryParams.SessionKey = sessionKey;
            entryParams.DateFrom = fromDate;
            entryParams.DateTo = toDate;

            errors = "";
            using (var client = new HttpClient())
            {
                var encodedSession = HttpUtility.HtmlEncode(sessionKey);
                try
                {
                    string returnResult = HttpUtils.SetupAndCallApi(client, true, entryParams, "Entry/GetNoteEntriesByRange", token);

                    resultDto = JsonConvert.DeserializeObject<GetNoteEntriesResultDto>(returnResult);
                    wereEntriesFound = resultDto.Result;
                    entriesFound = resultDto.Notes;
                    errors = resultDto.Errors ?? "";
                }
                catch (Exception ex)
                {
                    wereEntriesFound = false;
                    errors = $"trouble retrieving note entries, error: {ex.Message}";
                }
            }

            return entriesFound;
        }

        public List<BloodSugar>? GetBloodSugarEntriesBySessionKey(string sessionKey, DateTime fromDate, DateTime toDate, string token, out string errors)
        {
            bool wereEntriesFound = false;
            var resultDto = new GetBloodSugarEntriesResultDto();

            List<BloodSugar>? entriesFound = new List<BloodSugar>();
            var entryParams = new GetEntriesParamsDto();
            entryParams.SessionKey = sessionKey;
            entryParams.DateFrom = fromDate;
            entryParams.DateTo = toDate;

            errors = "";
            using (var client = new HttpClient())
            {
                var encodedSession = HttpUtility.HtmlEncode(sessionKey);
                try
                {
                    string returnResult = HttpUtils.SetupAndCallApi(client, true, entryParams, "Entry/GetBloodSugarEntriesByRange", token);

                    resultDto = JsonConvert.DeserializeObject<GetBloodSugarEntriesResultDto>(returnResult);
                    wereEntriesFound = resultDto.Result;
                    entriesFound = resultDto.Entries;
                    errors = resultDto.Errors ?? "";
                }
                catch (Exception ex)
                {
                    wereEntriesFound = false;
                    errors = $"trouble retrieving statistic entries, error: {ex.Message}";
                }
            }

            return entriesFound;
        }

        public List<StatisticEntry>? GetEntriesBySessionKey(string sessionKey, string token, out string errors)
        {
            bool wereEntriesFound = false;
            var resultDto = new GetEntriesResultDto();

            List<StatisticEntry>? entriesFound = new List<StatisticEntry>();

            errors = "";
            using (var client = new HttpClient())
            {
                var encodedSession = HttpUtility.HtmlEncode(sessionKey);
                try
                {
                    string returnResult = HttpUtils.SetupAndCallApiNoBody(client, true, "Entry/GetEntriesBySessionKey", $"sessionKey={encodedSession}", token);

                    resultDto = JsonConvert.DeserializeObject<GetEntriesResultDto>(returnResult);
                    wereEntriesFound = resultDto.Result;
                    entriesFound = resultDto.Entries;
                    errors = resultDto.Errors ?? "";

                }
                catch (Exception ex)
                {
                    wereEntriesFound = false; 
                    errors = $"trouble getting statistic entries, error: {ex.Message}";
                }
            }

            return entriesFound;
        }

        public List<StatisticEntry>? GetEntriesBySessionKey(string sessionKey, DateTime fromDate, DateTime toDate, string token, out string errors)
        {
            bool wereEntriesFound = false;
            var resultDto = new GetEntriesResultDto();

            List<StatisticEntry>? entriesFound = new List<StatisticEntry>();
            var entryParams = new GetEntriesParamsDto();
            entryParams.SessionKey = sessionKey;
            entryParams.DateFrom = fromDate;
            entryParams.DateTo = toDate;

            errors = "";
            using (var client = new HttpClient())
            {
                var encodedSession = HttpUtility.HtmlEncode(sessionKey);
                try
                {
                    string returnResult = HttpUtils.SetupAndCallApi(client, true, entryParams, "Entry/GetEntriesByRange", token);

                    resultDto = JsonConvert.DeserializeObject<GetEntriesResultDto>(returnResult);
                    wereEntriesFound = resultDto.Result;
                    entriesFound = resultDto.Entries;
                    errors = resultDto.Errors ?? "";
                }
                catch (Exception ex)
                {
                    wereEntriesFound = false;
                    errors = $"trouble retrieving statistic entries, error: {ex.Message}";
                }
            }

            return entriesFound;
        }

        public Member? GetMemberInfoBySessionKey(string sessionKey, string token, out string errors)
        {
            var resultDto = new MemberResultDto();
            Member? member = null;

            errors = "";
            using (var client = new HttpClient())
            {
                var encodedSession = HttpUtility.HtmlEncode(sessionKey);
                try
                {
                    string returnResult = HttpUtils.SetupAndCallApiNoBody(client, true, "Member/GetMemberInfoBySessionKey", $"sessionKey={encodedSession}", token);

                    resultDto = JsonConvert.DeserializeObject<MemberResultDto>(returnResult);
                    member = resultDto.Member;
                    errors = resultDto.Errors ?? "";
                }
                catch (Exception ex)
                {
                    member = null;
                    errors = $"trouble getting profile info for member, error: {ex.Message}";
                }
            }

            return member;
        }

        public List<Question>? GetQuestionsBySessionKey(string sessionKey, string token, bool random, out string errors)
        {
            bool wereEntriesFound = false;
            var resultDto = new GetQuestionsResultDto();

            List<Question>? entriesFound = new List<Question>();

            errors = "";
            using (var client = new HttpClient())
            {
                var encodedSession = HttpUtility.HtmlEncode(sessionKey);
                try
                {
                    string returnResult = HttpUtils.SetupAndCallApiNoBody(client, true, "Entry/GetQuestionsBySessionKey", $"sessionKey={encodedSession}", token);

                    resultDto = JsonConvert.DeserializeObject<GetQuestionsResultDto>(returnResult);
                    wereEntriesFound = resultDto.Result;
                    entriesFound = resultDto.Questions;
                    errors = resultDto.Errors ?? "";
                }
                catch (Exception ex)
                {
                    wereEntriesFound = false;
                    errors = $"trouble getting statistic entries, error: {ex.Message}";
                }
            }

            return entriesFound;
        }

        public bool AddQuestionBySessionKey(Question question, string sessionKey, string token, out string errors)
        {
            throw new NotImplementedException();
        }

        public bool UpdateQuestion(Question questions, string sessionKey, string token, out string errors)
        {
            throw new NotImplementedException();
        }
    }
}
