using RecordMyStats.Common.Entities;
using RecordMyStats.DataAccess.Data.Vitals;

namespace RecordMyStats.BLL
{
    /// <summary>
    /// Forwards business method calls directly to a data repository, useful for local scenarios not using the rest web api.
    /// </summary>
    public class VitalsBLLDirectAccess : IVitalsBLL
    {
        private IVitalsRepository repos = VitalsRepositoryFactory.GetVitalsRepository();

        public bool LoginMember(string email, string password, out string sessionKey, out string fullName, out string token, out string errors)
        {
            string sessionKeyOut;
            string fullNameOut;
            string errorsOut;
            bool result = repos.LoginMember(email, password, out sessionKeyOut, out fullNameOut, out errorsOut);
            sessionKey = sessionKeyOut;
            fullName = fullNameOut;
            errors = errorsOut;
            token = ""; // token not needed for direct database call
            return result;
        }

        public bool AddMember(Member member, out string errors, out string newSessionKey, out string newToken)
        {
            string sessionKeyOut;
            string errorsOut;
            bool result = repos.AddMember(member, out errorsOut, out sessionKeyOut);
            errors = errorsOut;
            newSessionKey = sessionKeyOut;
            newToken = "";
            return result;
        }

        public bool IsEmailInUse(string email, out string errors)
        {
            string errorsOut;
            bool result = repos.IsEmailInUse(email, out errorsOut);
            errors = errorsOut;
            return result;
        }

        public bool AddEntry(StatisticEntry entry, string sessionKey, string token, out string errors)
        {
            string errorsOut;
            bool result = repos.AddEntry(entry, sessionKey, out errorsOut);
            errors = errorsOut;
            return result;
        }

        public bool AddNoteEntry(Note entry, string sessionKey, string token, out string errors)
        {
            string errorsOut;
            bool result = repos.AddNoteEntry(entry, sessionKey, out errorsOut);
            errors = errorsOut;
            return result;
        }

        public bool AddBloodSugarEntry(BloodSugar entry, string sessionKey, string token, out string errors)
        {
            string errorsOut;
            bool result = repos.AddBloodSugarEntry(entry, sessionKey, out errorsOut);
            errors = errorsOut;
            return result;
        }

        public bool AddBloodPressureEntry(BloodPressure bp, string sessionKey, string token, out string errors)
        {
            string errorsOut;
            bool result = repos.AddBloodPressureEntry(bp, sessionKey, out errorsOut);
            errors = errorsOut;
            return result;
        }

        public List<StatisticEntry>? GetEntriesBySessionKey(string sessionKey, string token, out string errors)
        {
            string errorsOut;
            var result = repos.GetEntriesBySessionKey(sessionKey, out errorsOut);
            errors = errorsOut;
            return result;
        }

        public List<StatisticEntry>? GetEntriesBySessionKey(string sessionKey, DateTime fromDate, DateTime toDate, string token, out string errors)
        {
            string errorsOut;
            var result = repos.GetEntriesBySessionKey(sessionKey, fromDate, toDate, out errorsOut);
            errors = errorsOut;
            return result;
        }

        public List<BloodSugar>? GetBloodSugarEntriesBySessionKey(string sessionKey, DateTime fromDate, DateTime toDate, string token, out string errors)
        {
            string errorsOut;
            var result = repos.GetBloodSugarEntriesBySessionKey(sessionKey, fromDate, toDate, out errorsOut);
            errors = errorsOut;
            return result;
        }

        public List<BloodSugar>? GetBloodSugarEntriesBySessionKey(string sessionKey, string token, out string errors)
        {
            string errorsOut;
            var result = repos.GetBloodSugarEntriesBySessionKey(sessionKey, out errorsOut);
            errors = errorsOut;
            return result;
        }

        public Member? GetMemberInfoBySessionKey(string sessionKey, string token, out string errors)
        {
            string errorsOut;
            var result = repos.GetMemberInfoBySessionKey(sessionKey, out errorsOut);
            errors = errorsOut;
            return result;
        }

        public List<Note>? GetNoteEntriesByRange(string sessionKey, DateTime fromDate, DateTime toDate, string token, out string errors)
        {
            throw new NotImplementedException();
        }

        public List<Question>? GetQuestionsBySessionKey(string sessionKey, string token, bool random,out string errors)
        {
            string errorsOut;
            var result = repos.GetQuestionsBySessionKey(sessionKey, random, out errorsOut);
            errors = errorsOut;
            return result;
        }

        public bool AddQuestionBySessionKey(Question question, string sessionKey, string token, out string errors)
        {
            string errorsOut;
            bool result = repos.AddQuestionEntry(question, sessionKey, out errorsOut);
            errors = errorsOut;
            return result;
        }

        public bool UpdateQuestion(Question question, string sessionKey, string token, out string errors)
        {
            string errorsOut;
            bool result = repos.UpdateQuestion(question, sessionKey, out errorsOut);
            errors = errorsOut;
            return result;
        }

        public List<BloodPressure>? GetBloodPressureEntriesBySessionKey(string sessionKey, string token, out string errors)
        {
            throw new NotImplementedException();
        }

        public List<BloodPressure>? GetBloodPressureEntriesBySessionKey(string sessionKey, DateTime fromDate, DateTime toDate, string token, out string errors)
        {
            throw new NotImplementedException();
        }
    }
}

