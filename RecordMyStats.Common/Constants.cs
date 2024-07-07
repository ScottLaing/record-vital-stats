﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
