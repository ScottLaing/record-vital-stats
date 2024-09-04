namespace RecordMyStats.Common
{
    public class Constants
    {
        public readonly static string Salt = Environment.GetEnvironmentVariable("RMSDefaultSalt") ?? "DC3CF258-6B9E-46CE-8CE1-694CC20A3424"; 
        public readonly static string EncryptionKey = Environment.GetEnvironmentVariable("RMSDefaultEncryptionKey") ?? "F309BAAD-B39D-47AD-AAFF-536517712037";
        public const int GuidLength = 36;

        public static bool EncryptNotes = true;

        public class AppGlobal
        {
            public static string ApplicationName = "Record Vital Stats";
        }

        public class TokenOptions
        {
            public const int TokenExpireHours = 24;
            public const int TokenExpireMinutes = 0;
        }

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
