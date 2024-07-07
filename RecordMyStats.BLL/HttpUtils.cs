using Newtonsoft.Json;
using RecordMyStats.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordMyStats.BLL
{
    internal class HttpUtils
    {
        internal static string SetupAndCallApi(HttpClient client, bool useOathToken, object dto, string requestPath, string token)
        {
            try
            {
                string entrySerialized = JsonConvert.SerializeObject(dto);
                CancellationToken cancellationToken = new CancellationToken();
                if (useOathToken)
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                }
                string path = $"{Config.RestApiAddresses.HttpApiAddress}{requestPath}";
                var stringContent = new StringContent(entrySerialized, Encoding.UTF8, "application/json");
                var res = client.PostAsync(path, stringContent, cancellationToken);
                var result = res.Result;
                result.EnsureSuccessStatusCode();
                var returnInfo = result.Content.ReadAsStringAsync();
                var returnResult = returnInfo.Result;
                return returnResult;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        internal static string SetupAndCallApiNoBody(HttpClient client, bool useOathToken, string requestPath, string httpParams, string token)
        {
            try
            {
                if (useOathToken)
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                }
                var res = client.PostAsync($"{Config.RestApiAddresses.HttpApiAddress}{requestPath}?{httpParams}", new StringContent(""));
                var result = res.Result;
                result.EnsureSuccessStatusCode();
                var returnInfo = result.Content.ReadAsStringAsync();
                var returnResult = returnInfo.Result;
                return returnResult;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
