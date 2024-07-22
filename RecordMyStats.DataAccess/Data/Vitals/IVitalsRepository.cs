using RecordMyStats.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordMyStats.DataAccess.Data.Vitals
{
    public interface IVitalsRepository
    {
        bool AddMember(Member member, out string errors, out string newSessionKey);

        bool LoginMember(string email, string password, out string sessionKey, out string fullName, out string errors);

        string CreateSessionByEmail(string email, out string errors);

        bool AddEntry(StatisticEntry entry, string sessionKey, out string errors);

        List<StatisticEntry>? GetEntriesBySessionKey(string sessionKey, out string errors);

        List<StatisticEntry>? GetEntriesBySessionKey(string sessionKey, DateTime from, DateTime to, out string errors);

        List<Note>? GetNoteEntriesBySessionKey(string sessionKey, DateTime from, DateTime to, out string errors);

        List<Note>? GetNoteEntriesBySessionKey(string sessionKey, int nMostRecent, out string errors);

        bool IsEmailInUse(string email, out string errors);

        Member? GetMemberInfoBySessionKey(string sessionKey, out string errors);

        List<Question>? GetQuestionsBySessionKey(string sessionKey, bool random, out string errors);

        bool AddBloodSugarEntry(BloodSugar entry, string sessionKey, out string errors);

        bool AddNoteEntry(Note entry, string sessionKey, out string errors);

        bool AddBloodPressureEntry(BloodPressure entry, string sessionKey, out string errors);

        //bool GetBloodSugarEntriesByMemberId(int memberId, DateTime from, DateTime to, out List<BloodSugar>? bloodSugars, out string errors);

        //bool GetBloodSugarEntriesByMemberId(int memberId, out List<BloodSugar>? stats, out string errors);

        List<BloodSugar>? GetBloodSugarEntriesBySessionKey(string sessionKey, out string errors);

        List<BloodPressure>? GetBloodPressureEntriesBySessionKey(string sessionKey, out string errors);

        bool AddQuestionEntry(Question question, string sessionKey, out string errors);

        List<BloodSugar>? GetBloodSugarEntriesBySessionKey(string sessionKey, DateTime from, DateTime to, out string errors);

        List<BloodPressure>? GetBloodPressureEntriesBySessionKey(string sessionKey, DateTime from, DateTime to, out string errors);

        bool UpdateQuestion(Question question, string sessionKey, out string errorsOut);

        bool AddOxygenLevelEntry(OxygenLevel oxygenLevelEntry, string sessionKey, out string errors);
    }
}
