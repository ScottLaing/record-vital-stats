using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordMyStats.DataAccess.Data.Vitals
{
    public class PostgresSqlStrings
    {
        public static string AddMemberQueryString = @"INSERT INTO public.Member
                                       (FirstName
                                       ,LastName
                                       ,MiddleName
                                       ,IsActive
                                       ,Email
                                       ,DateOfBirth
                                       ,Sex
                                       ,Password
                                       ,Zip
                                       ,Country)
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

        public static string GetMemberByEmailQueryString = "SELECT id, FirstName, LastName from public.Member where Email = @Email";

        public static string GetMemberByEmailAndPasswordQueryString = 
                    "SELECT id, FirstName, LastName from public.Member where Email = @Email and Password = @Password";

        public static string GetAllMembersQueryString = "SELECT id, FirstName, LastName, Email, Sex, Zip, IsActive, MiddleName from public.Member ";

        public static string GetMemberDetailsById = @"select FirstName, MiddleName, LastName, DateOfBirth, Sex, Zip, Email, Country, CreateDate, IsActive
                                            FROM public.Member
		                                    WHERE Id = @MemberId";

        public static string AddStatisticEntryQueryString = @"INSERT INTO public.StatisticEntry
                                       (MemberId
                                       ,CreateDate
                                       ,BloodSugar
                                       ,Weight
                                       ,BPSystolic
                                       ,BPDiastolic
                                       ,HeartRate
                                        ,BSUnits
                                        ,WeightUnits)
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

        public static string AddNoteEntryQueryString = @"INSERT INTO public.Note
                           (Description
                           ,FullText
                           ,Created
                           ,ModBy
                           ,IsActive
                           ,Key1
                           ,Key2
                           ,MemberId
                            ,IsEncrypted
                            ,Salt)
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

        public static string AddQuestionEntryQueryString = @"INSERT INTO public.Question
                           (QuestionText
                           ,ExplanationText
                           ,MultipleAnswers
                           ,IsActive
                           ,Area
                           ,Area2)
                     VALUES
                           (@QuestionText
                           ,@ExplanationText
                           ,@MultipleAnswers
                           ,TRUE
                           ,''
                           ,'' );
                        SELECT scope_identity();";

        public static string UpdateQuestionQueryString = @"UPDATE public.Question
                        SET QuestionText = @QuestionText, ExplanationText = @ExplanationText
                        WHERE Id = @Id;";

        public static string AddAnswerQueryString = @"INSERT INTO public.Answer
                           (AnswerText
                           ,IsCorrect
                           ,IsActive
                           ,QuestionId)
                     VALUES
                           (@AnswerText
                           ,@IsCorrect
                           ,TRUE
                           ,@QuestionId);";

        public static string AddBloodSugarEntryQueryString = @"INSERT INTO public.BloodSugar
                               (MemberId
                               ,Value
                               ,Units
                               ,WhenTaken
                               ,IsActive
                               ,Mood
                               ,Comments
                               ,CreateDate
                               ,RecordingDate)
                         VALUES
                               (@MemberId
                               ,@Value
                               ,@Units
                               ,@WhenTaken
                               ,TRUE
                               ,@Mood
                               ,@Comments
                               ,@CreateDate
                               ,@RecordingDate)";

        public static string AddBloodPressureEntryQueryString = @"INSERT INTO public.BloodPressure
                               (MemberId
                               ,RecordingDate
                               ,Systolic
                               ,Diastolic
                               ,Units
                               ,WhenTaken
                               ,IsActive
                               ,CreateDate)
                         VALUES
                               (@MemberId
                               ,@RecordingDate
                               ,@Systolic
                               ,@Diastolic
                               ,@Units
                               ,@WhenTaken
                               ,TRUE
                               ,@CreateDate)";


        public static string UpdateSessionsDeactivate = @"UPDATE public.Session
                                       SET IsActive = FALSE 
                                        where MemberId = @MemberId AND IsActive = TRUE";


        public static string GetBloodSugarsByMemberIdQueryString = @"SELECT Id
                                  ,MemberId
                                  ,Value
                                  ,Units
                                  ,WhenTaken
                                  ,Comments
                                  ,Mood
                                  ,CustomTime
                                  ,IsActive
                                  ,CreateDate
                                  ,RecordingDate
                              FROM public.BloodSugar
                              WHERE MemberId = @MemberId 
                                    AND IsActive = TRUE
                              ORDER BY RecordingDate";

        public static string GetQuestionsIdQueryString =
                    @"SELECT  * FROM public.Question ";

        public static string GetQuestionLookupResultsByMemberIdQueryString =
                            @"SELECT  q.Id QuestionId
                                            ,q.QuestionText
                                              ,q.ExplanationText
                                              ,q.IsActive QuestionIsActive
                                              ,q.MultipleAnswers
                                              ,a.Id as AnswerId
                                                ,a.AnswerText
                                                ,a.IsCorrect AnswerIsCorrect
                                                ,a.IsActive AnswerIsActive
												 ,q.CreatedDate
                                              ,q.CreatedBy
                                              ,q.ModifiedDate
                                              ,q.ModifiedBy
                                          FROM public.Question q, public.Answer a
                                              WHERE q.Id = a.QuestionId
                                                    AND q.IsActive = TRUE
                                              ORDER BY 1, a.Id";

        public static string GetBloodSugarsByMemberIdDateRangeQueryString = @"SELECT Id
                                    ,MemberId
                                    ,Value
                                    ,Units
                                    ,WhenTaken
                                    ,Comments
                                    ,Mood
                                    ,CustomTime
                                    ,IsActive
                                    ,CreateDate
                                    ,RecordingDate
                                FROM public.BloodSugar
                                WHERE MemberId = @MemberId
                                        AND RecordingDate >= @DateFrom AND RecordingDate <= @DateTo
                                        AND IsActive = TRUE
                                ORDER BY RecordingDate";

        public static string GetNotesByMemberIdDateRangeQueryString = @"SELECT Id
                                      ,Description
                                      ,FullText
                                      ,Created
                                      ,ModBy
                                      ,IsActive
                                      ,Key1
                                      ,Key2
                                      ,Salt
                                      ,IsEncrypted
                                  FROM public.Note
                                WHERE MemberId = @MemberId
                                        AND Created >= @DateFrom AND Created <= @DateTo
                                        AND IsActive = TRUE
                                ORDER BY Created";

        public static string GetStatsByMemberIdQueryString = @"SELECT Id
                                              ,MemberId
                                              ,CreateDate
                                              ,BloodSugar
                                              ,Weight
                                              ,BPSystolic
                                              ,BPDiastolic
                                              ,HeartRate
                                              ,WeightUnits
                                              ,BSUnits
                                                FROM public.StatisticEntry 
                                                WHERE MemberId = @MemberId
                                                ORDER BY CreateDate";

        public static string GetStatsByMemberIdDateRangeQueryString = @"SELECT Id
                                              ,MemberId
                                              ,CreateDate
                                              ,BloodSugar
                                              ,Weight
                                              ,BPSystolic
                                              ,BPDiastolic
                                              ,HeartRate
                                              ,WeightUnits
                                              ,BSUnits
                                                FROM public.StatisticEntry 
                                                WHERE MemberId = @MemberId
                                                    AND CreateDate >= @DateFrom AND CreateDate <= @DateTo
                                                ORDER BY CreateDate";

        public static string GetMemberIdBySessionKeyQueryString = "SELECT MemberId, ExpiresDate, IsActive from public.Session where SessionKey = @SessionKey and IsActive = TRUE";


        public static string CreateSessionInsertSql = @"INSERT INTO public.Session
                       (MemberId
                       ,SessionKey
                       ,CreateDate
                       ,ExpiresDate
                       ,Platform
                       ,IsActive)
                 VALUES
                       (@MemberId
                       ,@SessionKey
                       ,@CreateDate
                       ,@ExpiresDate
                       ,@Platform
                       ,@IsActive)  ";

    }
}
