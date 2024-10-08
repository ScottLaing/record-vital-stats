﻿using Dapper;
using RecordMyStats.Common;
using RecordMyStats.Common.Entities;
using RecordMyStats.Common.Utility;
using System.Data.SqlClient;
using static RecordMyStats.Common.Constants;

namespace RecordMyStats.DataAccess.Data.Vitals
{
    public class SqlServerVitalsRepository : IVitalsRepository
    {
        private string ConnectionString = Config.DbConnectionStrings.VitalsDbConnString;

        #region query strings

        private string AddMemberQueryString = @"INSERT INTO [dbo].[Member]
                                       ([FirstName]
                                       ,[LastName]
                                       ,[MiddleName]
                                       ,[IsActive]
                                       ,[Email]
                                       ,[DateOfBirth]
                                       ,[Sex]
                                       ,[Password]
                                       ,[Zip]
                                       ,[Country])
                                 VALUES
                                       (@FirstName
                                       ,@LastName
                                       ,@MiddleName
                                       ,@IsActive
                                       ,@Email
                                       ,@DateOfBirth
                                       ,@Sex
                                       ,@Password
                                       ,@Zip
                                       ,@Country)";

        private string AddStatisticEntryQueryString = @"INSERT INTO [dbo].[StatisticEntry]
                                       ([MemberId]
                                       ,[CreateDate]
                                       ,[BloodSugar]
                                       ,[Weight]
                                       ,[BPSystolic]
                                       ,[BPDiastolic]
                                       ,[HeartRate]
                                        ,[BSUnits]
                                        ,[WeightUnits])
                                 VALUES
                                       (@MemberId
                                       ,@CreateDate
                                       ,@BloodSugar
                                       ,@Weight
                                       ,@BPSystolic
                                       ,@BPDiastolic
                                       ,@HeartRate
                                        ,@BSUnits
                                        ,@WeightUnits)";

        private string AddNoteEntryQueryString = @"INSERT INTO [dbo].[Note]
                           ([Description]
                           ,[FullText]
                           ,[Created]
                           ,[ModBy]
                           ,[IsActive]
                           ,[Key1]
                           ,[Key2]
                           ,[MemberId]
                            ,[IsEncrypted]
                            ,[Salt])
                     VALUES
                           (@Description
                           ,@FullText
                           ,@Created
                           ,@ModBy
                           ,@IsActive
                           ,@Key1
                           ,@Key2
                           ,@MemberId
                            ,@IsEncrypted
                            ,@Salt)";

        private string AddQuestionEntryQueryString = @"INSERT INTO [dbo].[Question]
                           ([QuestionText]
                           ,[ExplanationText]
                           ,[MultipleAnswers]
                           ,[IsActive]
                           ,[Area]
                           ,[Area2])
                     VALUES
                           (@QuestionText
                           ,@ExplanationText
                           ,@MultipleAnswers
                           ,1
                           ,''
                           ,'' );
                        SELECT scope_identity();";

        private string UpdateQuestionQueryString = @"UPDATE [dbo].[Question]
                        SET QuestionText = @QuestionText, ExplanationText = @ExplanationText
                        WHERE Id = @Id;";

        private string AddAnswerQueryString = @"INSERT INTO [dbo].[Answer]
                           ([AnswerText]
                           ,[IsCorrect]
                           ,[IsActive]
                           ,[QuestionId])
                     VALUES
                           (@AnswerText
                           ,@IsCorrect
                           ,1
                           ,@QuestionId);";

        private string AddBloodSugarEntryQueryString = @"INSERT INTO [dbo].[BloodSugar]
                               ([MemberId]
                               ,[Value]
                               ,[Units]
                               ,[WhenTaken]
                               ,[IsActive]
                               ,[CreateDate]
                               ,[RecordingDate])
                         VALUES
                               (@MemberId
                               ,@Value
                               ,@Units
                               ,@WhenTaken
                               ,1
                               ,@CreateDate
                               ,@RecordingDate)";

        private string AddBloodPressureEntryQueryString = @"INSERT INTO [dbo].[BloodPressure]
                               ([MemberId]
                               ,[RecordingDate]
                               ,[Systolic]
                               ,[Diastolic]
                               ,[Units]
                               ,[WhenTaken]
                               ,[IsActive]
                               ,[CreateDate])
                         VALUES
                               (@MemberId
                               ,@RecordingDate
                               ,@Systolic
                               ,@Diastolic
                               ,@Units
                               ,@WhenTaken
                               ,1
                               ,@CreateDate";


        private string UpdateSessionsDeactivate = @"UPDATE [dbo].[Session]
                                       SET IsActive = 0 
                                        where MemberId = @MemberId AND IsActive = 1";

        private string GetMemberByEmailQueryString = "SELECT id, FirstName, LastName from dbo.Member where Email = @Email";

        public string GetBloodSugarsByMemberIdQueryString = @"SELECT [Id]
                                  ,[MemberId]
                                  ,[Value]
                                  ,[Units]
                                  ,[WhenTaken]
                                  ,[Notes]
                                  ,[MoodLevel]
                                  ,[CustomTime]
                                  ,[IsActive]
                                  ,[CreateDate]
                                  ,[RecordingDate]
                              FROM [dbo].[BloodSugar]
                              WHERE MemberId = @MemberId 
                                    AND IsActive = 1
                              ORDER BY RecordingDate";

        public string GetQuestionsIdQueryString =
                    @"SELECT  * FROM [dbo].[Question] ";

        public string GetQuestionLookupResultsByMemberIdQueryString =
                            @"SELECT  q.Id QuestionId
                                            ,q.[QuestionText]
                                              ,q.[ExplanationText]
                                              ,q.[IsActive] QuestionIsActive
                                              ,q.MultipleAnswers
                                              ,a.Id as AnswerId
                                                ,a.AnswerText
                                                ,a.IsCorrect AnswerIsCorrect
                                                ,a.IsActive AnswerIsActive
												 ,q.[CreatedDate]
                                              ,q.[CreatedBy]
                                              ,q.[ModifiedDate]
                                              ,q.[ModifiedBy]
                                          FROM [dbo].[Question] q, dbo.Answer a
                                              WHERE q.Id = a.QuestionId
                                                    AND q.IsActive = 1
                                              ORDER BY 1, a.Id";

        public string GetBloodSugarsByMemberIdDateRangeQueryString = @"SELECT [Id]
                                    ,[MemberId]
                                    ,[Value]
                                    ,[Units]
                                    ,[WhenTaken]
                                    ,[Notes]
                                    ,[MoodLevel]
                                    ,[CustomTime]
                                    ,[IsActive]
                                    ,[CreateDate]
                                    ,[RecordingDate]
                                FROM [dbo].[BloodSugar]
                                WHERE MemberId = @MemberId
                                        AND RecordingDate >= @DateFrom AND RecordingDate <= @DateTo
                                        AND IsActive = 1
                                ORDER BY RecordingDate";

        public string GetNotesByMemberIdDateRangeQueryString = @"SELECT [Id]
                                      ,[Description]
                                      ,[FullText]
                                      ,[Created]
                                      ,[ModBy]
                                      ,[IsActive]
                                      ,[Key1]
                                      ,[Key2]
                                      ,[Salt]
                                      ,[IsEncrypted]
                                  FROM [dbo].[Note]
                                WHERE MemberId = @MemberId
                                        AND Created >= @DateFrom AND Created <= @DateTo
                                        AND IsActive = 1
                                ORDER BY Created";

        public string GetStatsByMemberIdQueryString = @"SELECT [Id]
                                              ,[MemberId]
                                              ,[CreateDate]
                                              ,[BloodSugar]
                                              ,[Weight]
                                              ,[BPSystolic]
                                              ,[BPDiastolic]
                                              ,[HeartRate]
                                              ,[WeightUnits]
                                              ,[BSUnits]
                                                FROM [dbo].[StatisticEntry] 
                                                WHERE MemberId = @MemberId
                                                ORDER BY CreateDate";

        public string GetStatsByMemberIdDateRangeQueryString = @"SELECT [Id]
                                              ,[MemberId]
                                              ,[CreateDate]
                                              ,[BloodSugar]
                                              ,[Weight]
                                              ,[BPSystolic]
                                              ,[BPDiastolic]
                                              ,[HeartRate]
                                              ,[WeightUnits]
                                              ,[BSUnits]
                                                FROM [dbo].[StatisticEntry] 
                                                WHERE MemberId = @MemberId
                                                    AND CreateDate >= @DateFrom AND CreateDate <= @DateTo
                                                ORDER BY CreateDate";

        private string GetMemberIdBySessionKeyQueryString = "SELECT MemberId, ExpiresDate, IsActive from dbo.Session where SessionKey = @SessionKey and IsActive = 1";

        private string GetMemberByEmailAndPasswordQueryString = "SELECT id, FirstName, LastName from dbo.Member where Email = @Email and Password = @Password";

        private string GetAllMembersQueryString = "SELECT id, FirstName, LastName, Email, Sex, Zip, IsActive, MiddleName from dbo.Member ";

        private string CreateSessionInsertSql = @"INSERT INTO [dbo].[Session]
                       ([MemberId]
                       ,[SessionKey]
                       ,[CreateDate]
                       ,[ExpiresDate]
                       ,[Platform]
                       ,[IsActive])
                 VALUES
                       (@MemberId
                       ,@SessionKey
                       ,@CreateDate
                       ,@ExpiresDate
                       ,@Platform
                       ,@IsActive)  ";

        private string GetMemberDetailsById = @"select FirstName, LastName, Sex, Zip, Email, Country, CreateDate, IsActive
                                            FROM [dbo].[Member]
		                                    WHERE Id = @MemberId";
        private bool _randomizeQuestions = false;
        private static Random rnd = new Random();

        #endregion

        public bool LoginMember(string email, string password, out string sessionKey, out string fullName, out string errors)
        {
            int memberId = 0;
            sessionKey = "";
            fullName = "";
            errors = "";
            List<Member> members = new List<Member>();

            string passHash = HashUtility.HashString(password);
            string memberFullname = "";

            using (var connection = new SqlConnection(ConnectionString))
            {
                var dictionary = new Dictionary<string, object>
                {
                    { "@Email", email },
                    { "@Password", passHash }
                };
                var parameters = new DynamicParameters(dictionary);
                try
                {
                    var member = connection.QuerySingle<Member>(GetMemberByEmailAndPasswordQueryString, parameters);
                    memberId = member.Id;
                    memberFullname = member.FullName;
                }
                catch (InvalidOperationException ex)
                {
                    memberId = 0;
                    errors = "no match found for user and password - " + ex.Message;
                }
                catch (Exception ex)
                {
                    errors = "no match found for user and password - " + ex.Message;
                    memberId = 0;
                }
            }
            if (memberId == 0)
            {
                errors = "no match found for user and password";
                return false;
            }

            bool successRemoveOtherSessions = KillOpenSessions(memberId, out string errors2);
            if (!successRemoveOtherSessions)
            {
                errors = errors2 + " - errors with clean prev sessions call";
                return false;
            }

            bool createSessionOK = CreateSession(memberId, out string errors3, out string newSessionKey);
            if (!createSessionOK)
            {
                errors = errors3 + " - error creating session"; ;
                return false;
            }
            else
            {
                sessionKey = newSessionKey;
                fullName = memberFullname;
                return true;
            }
        }

        public bool AddMember(Member member, out string errors, out string newSessionKey)
        {
            bool success = true;
            errors = "";
            newSessionKey = "";

            string passHash = "";
            if (string.IsNullOrWhiteSpace(member.Password))
            {
                errors = "Password not provided - cannot create user";
                return false;
            }
            else
            {
                passHash = HashUtility.HashString(member.Password);
            }

            var members = new List<Member>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(AddMemberQueryString, connection);
                command.Parameters.AddWithValue("@FirstName", member.FirstName);
                command.Parameters.AddWithValue("@LastName", member.LastName);
                command.Parameters.AddWithValue("@MiddleName", member.MiddleName);
                command.Parameters.AddWithValue("@IsActive", true);
                command.Parameters.AddWithValue("@Email", member.Email);
                command.Parameters.AddWithValue("@DateOfBirth", member.DateOfBirth);
                command.Parameters.AddWithValue("@Sex", member.Sex);
                command.Parameters.AddWithValue("@Password", passHash);
                command.Parameters.AddWithValue("@Zip", member.Zip);
                command.Parameters.AddWithValue("@Country", member.Country);

                try
                {
                    connection.Open();
                    int callResult = command.ExecuteNonQuery();
                    if (callResult != 1)
                    {
                        success = false;
                        errors = "Unspecified error with adding user, no records inserted.";
                    }
                }
                catch (Exception ex)
                {
                    success = false;
                    errors = "Error adding user: " + ex.Message;
                }
            }

            if (success)
            {
                int newMemberId = GetMemberIdByEmail(member?.Email ?? "", out string memEmailErr);

                if (newMemberId == 0)
                {
                   errors = "Trouble getting session key for new user - " + memEmailErr;
                   return false;
                }

                CreateSession(newMemberId, out string createSessErr, out string newSessionKey2);
                if (!string.IsNullOrWhiteSpace(createSessErr))
                {
                    errors = TroubleGettingSessionKey + createSessErr;
                    return false;
                }
                newSessionKey = newSessionKey2;
            }
            return success;
        }

        public List<Member> GetAllMembers()
        {
            List<Member>  members = new List<Member>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                members = connection.Query<Member>(GetAllMembersQueryString).ToList();
            }
            return members;
        }

        public bool IsEmailInUse(string email, out string errors)
        {
            errors = string.Empty;

            int memberId = GetMemberIdByEmail(email, out string errors2);
            if (! string.IsNullOrEmpty(errors2))
            {
                errors = errors2;
                return true;
            }
            if (memberId != 0)
            {
                return true;
            }
            return false;
        }

        public string CreateSessionByEmail(string email, out string errors)
        {
            string result = "";
            errors = "";
            int memberId = GetMemberIdByEmail(email, out string memberIdByEmailErrors) ;
            if (memberId == 0)
            {
                errors = $"Trouble getting member id for this email [{email}] {memberIdByEmailErrors}";
                return "";
            }

            bool success = CreateSession(memberId, out string createSessionErrors, out string newSessionKey);
            if (success)
            {
                result = newSessionKey;
            }
            else
            {
                errors = createSessionErrors;
            }
            return result;
        }

        public int GetMemberIdBySessionKey(string sessionKey, out string errors)
        {
            int result = 0;
            errors = "";
            Session? session = null;

            using (var connection = new SqlConnection(ConnectionString))
            {
                var dictionary = new Dictionary<string, object>
                {
                    { "@SessionKey", sessionKey }
                };
                var parameters = new DynamicParameters(dictionary);
                try
                {
                    session = connection.QuerySingle<Session>(GetMemberIdBySessionKeyQueryString, parameters);
                    result = session.MemberId;
                }
                catch (InvalidOperationException)
                {
                    errors = "no matching active session";
                    result = 0;
                }
                catch (Exception ex)
                {
                    errors = ex.Message;
                    result = 0;
                }
            }
            return result;
        }

        public bool AddEntry(StatisticEntry entry, string sessionKey, out string errors)
        {
            errors = "";

            int memberId = GetMemberIdBySessionKey(sessionKey, out string errors2);
            if (memberId == 0)
            {
                errors = "trouble saving entry - " + errors2;
                return false;
            }

            bool success = AddEntry(entry, memberId, out string errors3);
            if (! success)
            {
                errors = "trouble saving entry - " + errors3;
            }
            return success;
        }


        public bool AddBloodSugarEntry(BloodSugar entry, string sessionKey, out string errors)
        {
            errors = "";

            int memberId = GetMemberIdBySessionKey(sessionKey, out string errors2);
            if (memberId == 0)
            {
                errors = "trouble saving blood sugar entry - " + errors2;
                return false;
            }

            bool success = AddBloodSugarEntry(entry, memberId, out string errors3);
            if (!success)
            {
                errors = "trouble saving blood sugar entry - " + errors3;
            }
            return success;
        }

        public bool AddNoteEntry(Note entry, string sessionKey, out string errors)
        {
            errors = "";

            int memberId = GetMemberIdBySessionKey(sessionKey, out string errors2);
            if (memberId == 0)
            {
                errors = "trouble saving note entry - " + errors2;
                return false;
            }

            if (Constants.EncryptNotes)
            {
                if (string.IsNullOrWhiteSpace(entry.Salt))
                {
                    entry.Salt = Constants.Salt;
                }
                entry.FullText = CryptUtils.EncryptString(entry.FullText, entry.Salt ?? Constants.Salt);
                entry.IsEncrypted = true;
            }
            else
            {
                entry.IsEncrypted = false;
            }

            bool success = AddNoteEntry(entry, memberId, out string errors3);
            if (!success)
            {
                errors = "trouble saving blood sugar entry - " + errors3;
            }
            return success;
        }

        public bool AddBloodPressureEntry(BloodPressure entry, string sessionKey, out string errors)
        {
            errors = "";

            int memberId = GetMemberIdBySessionKey(sessionKey, out string errors2);
            if (memberId == 0)
            {
                errors = "trouble saving blood sugar entry - " + errors2;
                return false;
            }

            bool success = AddBloodPressureEntry(entry, memberId, out string errors3);
            if (!success)
            {
                errors = "trouble saving blood sugar entry - " + errors3;
            }
            return success;
        }

        private bool AddBloodSugarEntry(BloodSugar entry, int memberId, out string errors)
        {
            bool result = true;
            errors = "";

            using (var connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(AddBloodSugarEntryQueryString, connection);
                command.Parameters.AddWithValue("@MemberId", memberId);
                command.Parameters.AddWithValue("@Value", entry.Value);
                command.Parameters.AddWithValue("@Units", ((object?)entry.Units) ?? DBNull.Value);
                command.Parameters.AddWithValue("@WhenTaken", ((object?)entry.WhenTaken) ?? DBNull.Value);
                command.Parameters.AddWithValue("@CreateDate", ((object?)entry.CreateDate) ?? DBNull.Value);
                command.Parameters.AddWithValue("@RecordingDate", ((object?)entry.RecordingDate) ?? DBNull.Value);

                try
                {
                    connection.Open();
                    int callResult = command.ExecuteNonQuery();
                    if (callResult != 1)
                    {
                        result = false;
                        errors = "Unspecified error with saving blood sugar entry, no records inserted.";
                    }
                }
                catch (Exception ex)
                {
                    result = false;
                    errors = "Error saving blood sugar entry: " + ex.Message;
                }
            }
            return result;
        }

        public bool AddQuestionEntry(Question question, string sessionKey, out string errors)
        {
            bool result = true;
            errors = "";

            if (question == null)
            {
                errors = "bad parameters";
                return false;
            }
            if (question.QuestionText == null || string.IsNullOrWhiteSpace(question.QuestionText))
            {
                errors = "question text null or missing, cannot add";
                return false;
            }

            if (GetAllQuestions(out List<Question>? questions, out string getAllQuestionsErrors)) 
            {
                if (questions != null && questions.Any())
                {
                    var existingQuestionsWithSameText = questions.Where(q => q.QuestionText == question.QuestionText).Count();
                    if (existingQuestionsWithSameText > 0)
                    {
                        errors = "question already in table, not adding";
                        return false;
                    }
                }
            }

            using (var connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(AddQuestionEntryQueryString, connection);
                command.Parameters.AddWithValue("@QuestionText", ((object?)question.QuestionText) ?? DBNull.Value);
                command.Parameters.AddWithValue("@ExplanationText", ((object?)question.ExplanationText) ?? DBNull.Value);
                command.Parameters.AddWithValue("@MultipleAnswers", ((object?)question.MultipleAnswers) ?? DBNull.Value);

                try
                {
                    connection.Open();
                    int newQuestionId = Convert.ToInt32(command.ExecuteScalar());
                    if (newQuestionId <= 0)
                    {
                        result = false;
                        errors = "Unspecified error with saving question, no records inserted.";
                    }
                    if (question.AnswersList != null)
                    {
                        foreach (var answer in question.AnswersList)
                        {
                            answer.QuestionId = newQuestionId;
                            bool answerAddRes = AddAnswer(answer, "", out string answerAddError);
                            if (!answerAddRes)
                            {
                                errors += answerAddError;
                                result = false;
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    result = false;
                    errors = "Error saving question: " + ex.Message;
                }
            }
            return result;
        }

        public bool AddAnswer(Answer answer, string sessionKey, out string errors)
        {
            bool result = true;
            errors = "";

            using (var connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(AddAnswerQueryString, connection);
                command.Parameters.AddWithValue("@AnswerText", ((object?)answer.AnswerText) ?? DBNull.Value);
                command.Parameters.AddWithValue("@IsCorrect", ((object?)answer.IsCorrect) ?? DBNull.Value);
                command.Parameters.AddWithValue("@QuestionId", ((object?)answer.QuestionId) ?? DBNull.Value);

                try
                {
                    connection.Open();
                    int callResult = command.ExecuteNonQuery();
                    if (callResult != 1)
                    {
                        result = false;
                        errors = "Unspecified error with saving answer, no records inserted.";
                    }
                }
                catch (Exception ex)
                {
                    result = false;
                    errors = "Error saving answer: " + ex.Message;
                }
            }
            return result;
        }

        private bool AddNoteEntry(Note entry, int memberId, out string errors)
        {
            bool result = true;
            errors = "";

            using (var connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(AddNoteEntryQueryString, connection);
                command.Parameters.AddWithValue("@MemberId", memberId);
                command.Parameters.AddWithValue("@Description", ((object?)entry.Description) ?? DBNull.Value);
                command.Parameters.AddWithValue("@FullText", ((object?)entry.FullText) ?? DBNull.Value);
                command.Parameters.AddWithValue("@Created", ((object?)entry.Created) ?? DBNull.Value);
                command.Parameters.AddWithValue("@ModBy", ((object?)entry.ModBy) ?? DBNull.Value);
                command.Parameters.AddWithValue("@IsActive", ((object?)entry.IsActive) ?? DBNull.Value);
                command.Parameters.AddWithValue("@Key1", ((object?)entry.Key1) ?? DBNull.Value);
                command.Parameters.AddWithValue("@Key2", ((object?)entry.Key2) ?? DBNull.Value);
                command.Parameters.AddWithValue("@IsEncrypted", ((object?)entry.IsEncrypted) ?? DBNull.Value);
                command.Parameters.AddWithValue("@Salt", ((object?)entry.Salt) ?? DBNull.Value);

                try
                {
                    connection.Open();
                    int callResult = command.ExecuteNonQuery();
                    if (callResult != 1)
                    {
                        result = false;
                        errors = "Unspecified error with saving note entry, no records inserted.";
                    }
                }
                catch (Exception ex)
                {
                    result = false;
                    errors = "Error saving note entry: " + ex.Message;
                }
            }
            return result;
        }

        private bool AddBloodPressureEntry(BloodPressure entry, int memberId, out string errors)
        {
            bool result = true;
            errors = "";

            using (var connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(AddBloodPressureEntryQueryString, connection);
                command.Parameters.AddWithValue("@MemberId", memberId);
                command.Parameters.AddWithValue("@Systolic", ((object?)entry.Systolic) ?? DBNull.Value);
                command.Parameters.AddWithValue("@Diastolic", ((object?)entry.Diastolic) ?? DBNull.Value);
                command.Parameters.AddWithValue("@Units", ((object?)entry.Units) ?? DBNull.Value);
                command.Parameters.AddWithValue("@WhenTaken", ((object?)entry.WhenTaken) ?? DBNull.Value);
                command.Parameters.AddWithValue("@CreateDate", ((object?)entry.CreateDate) ?? DBNull.Value);
                command.Parameters.AddWithValue("@RecordingDate", ((object?)entry.RecordingDate) ?? DBNull.Value);

                try
                {
                    connection.Open();
                    int callResult = command.ExecuteNonQuery();
                    if (callResult != 1)
                    {
                        result = false;
                        errors = "Unspecified error with saving blood pressure entry, no records inserted.";
                    }
                }
                catch (Exception ex)
                {
                    result = false;
                    errors = "Error saving blood pressure entry: " + ex.Message;
                }
            }
            return result;
        }

        public List<StatisticEntry>? GetEntriesBySessionKey(string sessionKey, out string errors)
        {
            errors = "";

            int memberId = GetMemberIdBySessionKey(sessionKey, out string errors2);
            if (memberId == 0)
            {
                errors = "trouble saving entry - " + errors2;
                return null;
            }

            bool success = GetEntriesByMemberId(memberId, out List<StatisticEntry>? stats, out string errors3);
            if (!success)
            {
                errors = "trouble getting entries - " + errors3;
                return null;
            }
            return stats;
        }

        public List<BloodSugar>? GetBloodSugarEntriesBySessionKey(string sessionKey, out string errors)
        {
            errors = "";

            int memberId = GetMemberIdBySessionKey(sessionKey, out string memberLookupErrors);
            if (memberId == 0)
            {
                errors = "trouble saving entry - " + memberLookupErrors;
                return null;
            }

            bool success = GetBloodSugarEntriesByMemberId(memberId, out List<BloodSugar>? stats, out string bloodSugarLookupErrors);
            if (!success)
            {
                errors = "trouble getting entries - " + bloodSugarLookupErrors;
                return null;
            }
            return stats;
        }

        public List<Question>? GetQuestionsBySessionKey(string sessionKey,bool random, out string errors)
        {
            errors = "";

            int memberId = GetMemberIdBySessionKey(sessionKey, out string memberLookupErrors);
            if (memberId == 0)
            {
                errors = "trouble saving entry - " + memberLookupErrors;
                return null;
            }

            bool success = GetQuestionsByMemberId(memberId, out List<Question>? questions, random, out string questionLookupErrors);
            if (!success)
            {
                errors = "trouble getting entries - " + questionLookupErrors;
                return null;
            }
            return questions;
        }

        public List<Note>? GetNoteEntriesBySessionKey(string sessionKey, DateTime from, DateTime to, out string errors)
        {
            errors = "";

            int memberId = GetMemberIdBySessionKey(sessionKey, out string memberLookupErrors);
            if (memberId == 0)
            {
                errors = "trouble getting notes session key failure - " + memberLookupErrors;
                return null;
            }

            bool success = GetNoteEntriesByMemberId(memberId, from, to, out List<Note>? notes, out string noteLookupErrors);
            if (!success)
            {
                errors = "trouble getting notes - " + noteLookupErrors;
                return null;
            }
            return notes;
        }

        public List<Note>? GetNoteEntriesBySessionKey(string sessionKey, int nMostRecent, out string errors)
        {
            throw new Exception();
        }

        public List<BloodSugar>? GetBloodSugarEntriesBySessionKey(string sessionKey, DateTime from, DateTime to, out string errors)
        {
            errors = "";

            int memberId = GetMemberIdBySessionKey(sessionKey, out string memberLookupErrors);
            if (memberId == 0)
            {
                errors = "trouble saving entry - " + memberLookupErrors;
                return null;
            }

            bool success = GetBloodSugarEntriesByMemberId(memberId, from, to, out List<BloodSugar>? stats, out string bloodSugarLookupErrors);
            if (!success)
            {
                errors = "trouble getting entries - " + bloodSugarLookupErrors;
                return null;
            }
            return stats;
        }

        public List<StatisticEntry>? GetEntriesBySessionKey(string sessionKey, DateTime from, DateTime to, out string errors)
        {
            errors = "";

            int memberId = GetMemberIdBySessionKey(sessionKey, out string errors2);
            if (memberId == 0)
            {
                errors = "trouble saving entry - " + errors2;
                return null;
            }

            bool success = GetEntriesByMemberId(memberId, from, to, out List<StatisticEntry>? stats, out string errors3);
            if (!success)
            {
                errors = "trouble getting entries - " + errors3;
                return null;
            }
            return stats;
        }

        private int GetMemberIdByEmail(string email, out string errors)
        {
            int result = 0;
            errors = "";
            if (string.IsNullOrEmpty(email))
            {
                errors = "empty email passed to get member id by email";
                return 0;
            }

            List<Member> members = new List<Member>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                var dictionary = new Dictionary<string, object>
                {
                    { "@Email", email }
                };
                var parameters = new DynamicParameters(dictionary);
                try
                {
                    var member = connection.QuerySingle<Member>(GetMemberByEmailQueryString, parameters);
                    result = member.Id;
                }
                catch (InvalidOperationException)
                {
                    result = 0;
                }
                catch (Exception ex)
                {
                    errors = ex.Message;
                    result = 0;
                }
            }
            return result;
        }

        public Member? GetMemberDetailsByMemberId(int memberId, out string errors)
        {
            var result = new Member();

            errors = "";
            if (memberId <= 0)
            {
                errors = "Invalid member id passed to GetMemberDetailsByMemberId";
                return null;
            }

            List<Member> members = new List<Member>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                var dictionary = new Dictionary<string, object>
                {
                    { "@MemberId", memberId }
                };
                var parameters = new DynamicParameters(dictionary);
                try
                {
                    var member = connection.QuerySingle<Member>(GetMemberDetailsById, parameters);
                    result = member;
                }
                catch (InvalidOperationException ioEx)
                {
                    result = null;
                    errors = ioEx.Message + " (invalid operation exception)";
                }
                catch (Exception ex)
                {
                    errors = ex.Message;
                    result = null;
                }
            }
            return result;
        }

        public Member? GetMemberInfoBySessionKey(string sessionKey, out string errors)
        {
            errors = "";
            Member? result = null;
            int memberId = GetMemberIdBySessionKey(sessionKey, out string errorsGetMemberId);
            if (memberId == 0)
            {
                errors = "trouble getting member id - " + errorsGetMemberId;
                return null;
            }

            result = GetMemberDetailsByMemberId(memberId, out string errorsGetMemberDetails);
            if (!string.IsNullOrWhiteSpace(errorsGetMemberDetails))
            {
                errors = "trouble getting member details - " + errorsGetMemberDetails;
                return null;
            }
            return result;
        }

        private bool CreateSession(int memberId, out string errors, out string newSessionKey)
        {
            bool result = true;
            errors = "";
            newSessionKey = "";

            string sessionKey = HashUtility.GetUniqueSessionKey();

            var create = DateTime.Now;
            var tomorrow = create.AddDays(1);

            using (var connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(CreateSessionInsertSql, connection);
                command.Parameters.AddWithValue("@MemberId", memberId);
                command.Parameters.AddWithValue("@SessionKey", sessionKey);
                command.Parameters.AddWithValue("@CreateDate", create);
                command.Parameters.AddWithValue("@ExpiresDate", tomorrow);
                command.Parameters.AddWithValue("@Platform", "win-native");
                command.Parameters.AddWithValue("@IsActive", true);


                try
                {
                    connection.Open();
                    int callResult = command.ExecuteNonQuery();
                    if (callResult != 1)
                    {
                        result = false;
                        errors = "Unspecified error with adding user, no records inserted.";
                    }
                    else
                    {
                        newSessionKey = sessionKey;
                    }
                }
                catch (Exception ex)
                {
                    result = false;
                    errors = "Error adding user: " + ex.Message;
                }
            }
            return result;
        }

        private bool GetBloodSugarEntriesByMemberId(int memberId, out List<BloodSugar>? stats, out string errors)
        {
            errors = "";
            List<BloodSugar> bloodSugarEntries = new List<BloodSugar>();
            stats = new List<BloodSugar>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                var dictionary = new Dictionary<string, object>
                {
                    { "@MemberId", memberId }
                };
                var parameters = new DynamicParameters(dictionary);
                try
                {
                    bloodSugarEntries = connection.Query<BloodSugar>(GetBloodSugarsByMemberIdQueryString, parameters).ToList();
                    if (!bloodSugarEntries.Any())
                    {
                        errors = "no blood sugar entries found";
                        return true;
                    }
                    stats = bloodSugarEntries;
                }
                catch (InvalidOperationException ex)
                {
                    errors = "trouble retrieving blood sugar entries - " + ex.Message;
                    return false;
                }
                catch (Exception ex)
                {
                    errors = "trouble retrieving blood sugar entries - " + ex.Message;
                    return false;
                }
            }
            return true;
        }

        // GetQuestionsIdQueryString

        private bool GetAllQuestions(out List<Question>? questions, out string errors)
        {
            errors = "";
            questions = new List<Question>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                var dictionary = new Dictionary<string, object>
                {
                };
                var parameters = new DynamicParameters(dictionary);
                try
                {
                    questions = connection.Query<Question>(GetQuestionsIdQueryString, parameters).ToList();
                    if (!questions.Any())
                    {
                        errors = "no Question entries found";
                        return true;
                    }

                }
                catch (InvalidOperationException ex)
                {
                    errors = "trouble retrieving question entries - " + ex.Message;
                    return false;
                }
                catch (Exception ex)
                {
                    errors = "trouble retrieving question entries - " + ex.Message;
                    return false;
                }
            }
            return true;
        }

        private bool GetQuestionsByMemberId(int memberId, out List<Question>? questions, bool random, out string errors)
        {
            errors = "";
            List<QuestionLookupResult> questionsResult = new List<QuestionLookupResult>();
            questions = new List<Question>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                var dictionary = new Dictionary<string, object>
                {
                };
                var parameters = new DynamicParameters(dictionary);
                try
                {
                    questionsResult = connection.Query<QuestionLookupResult>(GetQuestionLookupResultsByMemberIdQueryString, parameters).ToList();
                    if (!questionsResult.Any())
                    {
                        errors = "no Question entries found";
                        return true;
                    }

                    questions = QuestionLookupResult.GetQuestionsFromQuestionResults(questionsResult);
                    if (random && questions != null && questions.Any())
                    {
                        questions = questions.OrderBy(q => myRand(q)).ToList();
                    }
                }
                catch (InvalidOperationException ex)
                {
                    errors = "trouble retrieving question entries - " + ex.Message;
                    return false;
                }
                catch (Exception ex)
                {
                    errors = "trouble retrieving question entries - " + ex.Message;
                    return false;
                }
            }
            return true;
        }

        private int myRand(Question q)
        {
            int num = rnd.Next();
            return num;
        }

        private bool GetEntriesByMemberId(int memberId, out List<StatisticEntry>? stats, out string errors)
        {
            errors = "";
            List<StatisticEntry> entries = new List<StatisticEntry>();
            stats = new List<StatisticEntry>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                var dictionary = new Dictionary<string, object>
                {
                    { "@MemberId", memberId }
                };
                var parameters = new DynamicParameters(dictionary);
                try
                {
                    entries = connection.Query<StatisticEntry>(GetStatsByMemberIdQueryString, parameters).ToList();
                    if (!entries.Any())
                    {
                        errors = "no entries found";
                        return true;
                    }
                    stats = entries;
                }
                catch (InvalidOperationException ex)
                {
                    errors = "trouble retrieving entries - " + ex.Message;
                    return false;
                }
                catch (Exception ex)
                {
                    errors = "trouble retrieving entries - " + ex.Message;
                    return false;
                }
            }
            return true;
        }

        private bool GetNoteEntriesByMemberId(int memberId, DateTime from, DateTime to, out List<Note>? notes, out string errors)
        {
            errors = "";
            notes = new List<Note>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                var dictionary = new Dictionary<string, object>
                {
                    { "@MemberId", memberId },
                    { "@DateFrom", from },
                    { "@DateTo", to }
                };

                var parameters = new DynamicParameters(dictionary);
                try
                {
                    notes = connection.Query<Note>(GetNotesByMemberIdDateRangeQueryString, parameters).ToList();
                    if (!notes.Any())
                    {
                        errors = "no note entries found";
                        return true;
                    }
                    else
                    {
                        foreach (var note in notes)
                        {
                            if (note.IsEncrypted ?? false)
                            {
                                if (string.IsNullOrWhiteSpace(note.Salt))
                                {
                                    errors = "note is marked as encrypted but no salt value is found in the record to decrypt";
                                    return false;
                                }
                                note.FullText = CryptUtils.DecryptString(note.FullText, note.Salt);
                            }
                        }
                    }
                }
                catch (InvalidOperationException ex)
                {
                    errors = "trouble retrieving note entries - " + ex.Message;
                    return false;
                }
                catch (Exception ex)
                {
                    errors = "trouble retrieving note entries - " + ex.Message;
                    return false;
                }
            }
            return true;
        }

        private bool GetBloodSugarEntriesByMemberId(int memberId, DateTime from, DateTime to, out List<BloodSugar>? bloodSugars, out string errors)
        {
            errors = "";
            List<BloodSugar> bloodSugarEntries = new List<BloodSugar>();
            bloodSugars = new List<BloodSugar>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                var dictionary = new Dictionary<string, object>
                {
                    { "@MemberId", memberId },
                    { "@DateFrom", from },
                    { "@DateTo", to }
                };

                var parameters = new DynamicParameters(dictionary);
                try
                {
                    bloodSugarEntries = connection.Query<BloodSugar>(GetBloodSugarsByMemberIdDateRangeQueryString, parameters).ToList();
                    if (!bloodSugarEntries.Any())
                    {
                        errors = "no blood sugar entries found";
                        return true;
                    }
                    bloodSugars = bloodSugarEntries;
                }
                catch (InvalidOperationException ex)
                {
                    errors = "trouble retrieving blood sugar entries - " + ex.Message;
                    return false;
                }
                catch (Exception ex)
                {
                    errors = "trouble retrieving blood sugar entries - " + ex.Message;
                    return false;
                }
            }
            return true;
        }

        private bool GetEntriesByMemberId(int memberId, DateTime from, DateTime to, out List<StatisticEntry>? stats, out string errors)
        {
            errors = "";
            List<StatisticEntry> entries = new List<StatisticEntry>();
            stats = new List<StatisticEntry>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                var dictionary = new Dictionary<string, object>
                {
                    { "@MemberId", memberId },
                    { "@DateFrom", from },
                    { "@DateTo", to }
                };

                var parameters = new DynamicParameters(dictionary);
                try
                {
                    entries = connection.Query<StatisticEntry>(GetStatsByMemberIdDateRangeQueryString, parameters).ToList();
                    if (!entries.Any())
                    {
                        errors = "no entries found";
                        return true;
                    }
                    stats = entries;
                }
                catch (InvalidOperationException ex)
                {
                    errors = "trouble retrieving entries - " + ex.Message;
                    return false;
                }
                catch (Exception ex)
                {
                    errors = "trouble retrieving entries - " + ex.Message;
                    return false;
                }
            }
            return true;
        }

        private bool KillOpenSessions(int memberId, out string errors)
        {
            bool result = true;
            errors = "";

            using (var connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(UpdateSessionsDeactivate, connection);
                command.Parameters.AddWithValue("@MemberId", memberId);

                try
                {
                    connection.Open();
                    int callResult = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    result = false;
                    errors = "Error deactivating previous sessions: " + ex.Message;
                }
            }
            return result;
        }

        private bool AddEntry(StatisticEntry statisticEntry, int memberId, out string errors)
        {
            bool result = true;
            errors = "";

            var members = new List<Member>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(AddStatisticEntryQueryString, connection);
                command.Parameters.AddWithValue("@MemberId", memberId);
                command.Parameters.AddWithValue("@CreateDate", statisticEntry.CreateDate);
                command.Parameters.AddWithValue("@BloodSugar", ((object?)statisticEntry.BloodSugar) ?? DBNull.Value);
                command.Parameters.AddWithValue("@Weight", ((object?)statisticEntry.Weight) ?? DBNull.Value);
                command.Parameters.AddWithValue("@BPSystolic", ((object?)statisticEntry.BPSystolic) ?? DBNull.Value);
                command.Parameters.AddWithValue("@BPDiastolic", ((object?)statisticEntry.BPDiastolic) ?? DBNull.Value);
                command.Parameters.AddWithValue("@HeartRate", ((object?)statisticEntry.HeartRate) ?? DBNull.Value);
                command.Parameters.AddWithValue("@BSUnits", ((object?)statisticEntry.BSUnits) ?? DBNull.Value);
                command.Parameters.AddWithValue("@WeightUnits", ((object?)statisticEntry.WeightUnits) ?? DBNull.Value);
                try
                {
                    connection.Open();
                    int callResult = command.ExecuteNonQuery();
                    if (callResult != 1)
                    {
                        result = false;
                        errors = "Unspecified error with saving entry, no records inserted.";
                    }
                }
                catch (Exception ex)
                {
                    result = false;
                    errors = "Error saving entry: " + ex.Message;
                }
            }
            return result;
        }

        public bool UpdateQuestion(Question question, string sessionKey, out string errors)
        {
            bool result = true;
            errors = "";

            if (question == null)
            {
                errors = "bad parameters";
                return false;
            }
            if (question.QuestionText == null || string.IsNullOrWhiteSpace(question.QuestionText))
            {
                errors = "question text null or missing, cannot add";
                return false;
            }

            using (var connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(UpdateQuestionQueryString, connection);
                command.Parameters.AddWithValue("@QuestionText", ((object?)question.QuestionText) ?? DBNull.Value);
                command.Parameters.AddWithValue("@ExplanationText", ((object?)question.ExplanationText) ?? DBNull.Value);
                command.Parameters.AddWithValue("@Id", ((object?)question.Id) ?? DBNull.Value);

                try
                {
                    connection.Open();
                    int callResult = command.ExecuteNonQuery();
                    if (callResult != 1)
                    {
                        result = false;
                        errors = "Unspecified error with updating Question values, no records updated.";
                    }
                }
                catch (Exception ex)
                {
                    result = false;
                    errors = "Error saving question: " + ex.Message;
                }
            }
            return result;
        }

        public List<BloodPressure>? GetBloodPressureEntriesBySessionKey(string sessionKey, out string errors)
        {
            throw new NotImplementedException();
        }

        public List<BloodPressure>? GetBloodPressureEntriesBySessionKey(string sessionKey, DateTime from, DateTime to, out string errors)
        {
            throw new NotImplementedException();
        }

        public bool AddOxygenLevelEntry(OxygenLevel oxygenLevelEntry, string sessionKey, out string errors)
        {
            throw new NotImplementedException();
        }

        public List<OxygenLevel>? GetOxygenLevelEntriesBySessionKey(string sessionKey, out string errors)
        {
            throw new NotImplementedException();
        }
    }
}

