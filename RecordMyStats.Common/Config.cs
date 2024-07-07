using System.Configuration;
using System;

namespace RecordMyStats.Common
{
    public class Config
    {
        public class AppSettings
        {
            public static bool UsingPostgres = true;

            public static bool UsingLocalDatabase
            {
                get
                {
                    bool usingLocalDb = false;
                    if (ConfigurationManager.AppSettings != null)
                    {
                        try
                        {
                            string value = ConfigurationManager.AppSettings?["UsingLocalDatabase"] ?? "";
                            if (string.IsNullOrWhiteSpace(value))
                            {
                                usingLocalDb = true;
                            }
                            else
                            {
                                usingLocalDb = bool.Parse(value);
                            }
                        }
                        catch
                        {
                            usingLocalDb = true;
                        }
                    }
                    return usingLocalDb;
                }
            }

            public static bool UsingRestApi
            {
                get
                {
                    bool usingRestApi = true;
                    if (ConfigurationManager.AppSettings != null)
                    {
                        try
                        {
                            string value = ConfigurationManager.AppSettings?["UsingRestApi"] ?? "";
                            if (value != null)
                            {
                                usingRestApi = bool.Parse(value);
                            }
                        }
                        catch
                        {
                            usingRestApi = true;
                        }
                    }
                    return usingRestApi;
                }
            }

            public static bool UsingDevLocalApi
            {
                get
                {
                    bool usingDevLocalApi = true;
                    if (ConfigurationManager.AppSettings != null)
                    {
                        try
                        {
                            string value = ConfigurationManager.AppSettings?["UsingDevLocalApi"] ?? "";
                            if (value != null)
                            {
                                usingDevLocalApi = bool.Parse(value);
                            }
                        }
                        catch
                        {
                            usingDevLocalApi = true;
                        }
                    }
                    return usingDevLocalApi;
                }
            }
        }

        public class DbConnectionStrings
        {
            // local database for local dev and testing
            public static readonly string LocalDBConnString = AppSettings.UsingPostgres ? (Environment.GetEnvironmentVariable("RMSLocalPostgresConnString") ?? "") :
                                                                (Environment.GetEnvironmentVariable("RMSLocalDbConnString") ?? "");

            // azure db for live - note in use at this time.
            public static readonly string AzureDBConnString = Environment.GetEnvironmentVariable("RMSAzureDbConnString") ?? "";

            public static string VitalsDbConnString => AppSettings.UsingLocalDatabase ? LocalDBConnString : AzureDBConnString;
        }

        public class RestApiAddresses
        {
            public static readonly string HttpLocalDevApiAddress = "https://localhost:7290/api/";

            public static readonly string HttpAzureDeployedApiAddress = Environment.GetEnvironmentVariable("RMSAzureWebApiAddress") ?? "";

            public static string HttpApiAddress => AppSettings.UsingDevLocalApi ? HttpLocalDevApiAddress : HttpAzureDeployedApiAddress;
        }

        public class Secrets
        {
            public static readonly string JwtTokenPhrase = Environment.GetEnvironmentVariable("RMS_JWT_TokenPhrase") ?? "";
        }
    }
}
