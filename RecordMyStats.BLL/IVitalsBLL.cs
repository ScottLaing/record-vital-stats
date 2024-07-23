using RecordMyStats.Common.Entities;

namespace RecordMyStats.BLL
{
    public interface IVitalsBLL
    {
        bool AddMember(Member member, out string errors, out string newSessionKey, out string newToken);

        bool LoginMember(string email, string password, out string sessionKey, out string fullName, out string token, out string errors);

        bool IsEmailInUse(string email, out string errors);

        bool AddEntry(StatisticEntry entry, string sessionKey, string token, out string errors);

        bool AddBloodSugarEntry(BloodSugar entry, string sessionKey, string token, out string errors);

        List<StatisticEntry>? GetEntriesBySessionKey(string sessionKey, DateTime fromDate, DateTime toDate, string token, out string errors);

        List<StatisticEntry>? GetEntriesBySessionKey(string sessionKey, string token, out string errors);

        List<BloodSugar>? GetBloodSugarEntriesBySessionKey(string sessionKey, DateTime fromDate, DateTime toDate, string token, out string errors);

        List<BloodSugar>? GetBloodSugarEntriesBySessionKey(string sessionKey, string token, out string errors);

        List<BloodPressure>? GetBloodPressureEntriesBySessionKey(string sessionKey, string token, out string errors);

        List<OxygenLevel>? GetOxygenLevelEntriesBySessionKey(string sessionKey, string token, out string errors);

        List<OxygenLevel>? GetOxygenLevelEntriesBySessionKey(string sessionKey, DateTime fromDate, DateTime toDate, string token, out string errors);

        List<BloodPressure>? GetBloodPressureEntriesBySessionKey(string sessionKey, DateTime fromDate, DateTime toDate, string token, out string errors);

        Member? GetMemberInfoBySessionKey(string sessionKey, string token, out string errors);

        List<Question>? GetQuestionsBySessionKey(string sessionKey, string token, bool randomOrder, out string errors);

        bool AddOxygenEntry(OxygenLevel entry, string sessionKey, string token, out string errors);

        bool AddBloodPressureEntry(BloodPressure bp, string sessionKey, string token, out string errors);

        bool AddNoteEntry(Note entry, string sessionKey, string token, out string errors);

        bool AddQuestionBySessionKey(Question question, string sessionKey, string token, out string errors);

        // AddQuestionBySessionKey

        List<Note>? GetNoteEntriesByRange(string sessionKey, DateTime fromDate, DateTime toDate, string token, out string errors);

        bool UpdateQuestion(Question question, string sessionKey, string token,out string errors);
    }
}
