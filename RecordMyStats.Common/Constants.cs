namespace RecordMyStats.Common
{
    public class Constants
    {
        public readonly static string Salt = Environment.GetEnvironmentVariable("RMSDefaultSalt") ?? "DC3CF258-6B9E-46CE-8CE1-694CC20A3424"; 
        public readonly static string EncryptionKey = Environment.GetEnvironmentVariable("RMSDefaultEncryptionKey") ?? "F309BAAD-B39D-47AD-AAFF-536517712037";
        public const int GuidLength = 36;

        public static bool EncryptNotes = true;

        public const string SomeErrorsWithLookup = "Errors: some error occurred with lookup";
        public const string FromAndToDates = "Please enter from and to dates";
        public const string EntriesCount = "Entries count: {0}";
        public const string LoggedIn = " logged in.";
        public const string Errors = "errors: ";
        public const string LongDateFormat = "MMMM dd yyyy hh:mm tt";
        public const string MillimetersOfMercury = "mmHg";
        public const string ShortDateFormat = "yyyy-MM-dd";

        public const string SystolicBloodPressureNotValid = "Systolic blood pressure value not valid.";
        public const string DiastolicBloodPressureNotValid = "Diastolic blood pressure value not valid.";
        public const string HeartRateShouldBeNumber = "Heart rate value should be a number";
        public const string HeartRateNotInRange = "Heart rate value should be between 40 and 200";

        public const string DateFormat1 = "yyyy-MM-dd HH:mm.ss";
        public const string DateTimeWrongFormat = "Date and time are not in right format";
        public const string DateTimeCannotBeFuture = "Date time cannot be future";

        public const string ChoiceWhenMeasured = "Please select a choice for when measured";
        public const string TroubleSavingEntry = "Trouble saving entry: {0}";

        public Constants()
        {
            
        }

        public class AppGlobal
        {
            public static string ApplicationName = "Record Vital Stats";
        }

        public class TokenOptions
        {
            public const int TokenExpireHours = 24;
            public const int TokenExpireMinutes = 0;
        }

        public static readonly List<string> BloodSugarUnits = new List<string>
        {
            "mg/dL",
            "mmol/L"
        };

        public static readonly List<string> BloodSugarRecordingTimes = new List<string>
        {
            "Fasting (overnite)",
            "Pre-meal",
            "2 Hours After Meal",
            "1 Hour After Meal",
            "15 Mins After Meal"
        };

        public static readonly List<string> BloodPressureMeasuringTimes = new List<string>
        {
            "Morning",
            "During the day",
            "Before sleep"
        };
        

        public const string NoSelection = "No selection";

        public readonly static Dictionary<int, string> MoodMapDictionary = new Dictionary<int, string>()
        {
            { 0, NoSelection },
            { 1, "Happy" },
            { 2, "Okay" },
            { 3, "Sad" },
            { 4, "Tired" },
        };

        public class DisplayStrings
        {
            public static string NotAvailable = "N/A";
        }

        public class RestStrings
        {
            public static string RestParamsMissing = "missing parameters in rest call";
        }
    }
}
