using Newtonsoft.Json;
using RecordMyStats.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordMyStats.BLL
{
    public class LoginMemberHandler
    {
        public static bool LoginMember(string email, string password, out string sessionKey, out string fullName, out string token, out string errors)
        {
            bool success = true;
            LoginInfoDto loginDto = new LoginInfoDto();
            loginDto.Email = email;
            loginDto.Password = password;

            sessionKey = "";
            fullName = "";
            errors = "";
            token = "";
            var loginResultDto = new LoginMemberResultDto();

            using (var client = new HttpClient())
            {
                try
                {
                    string returnResult = HttpUtils.SetupAndCallApi(client, false, loginDto, "Member/LoginMember", "");

                    loginResultDto = JsonConvert.DeserializeObject<LoginMemberResultDto>(returnResult);
                    success = loginResultDto.Result;
                    errors = loginResultDto.Errors ?? "";
                    sessionKey = loginResultDto.SessionKey ?? "";
                    fullName = loginResultDto.FullName ?? "";
                    token = loginResultDto.Token ?? "";
                }
                catch (Exception ex)
                {
                    success = false; // on error show email in use to block further continued logic with this email until issue is addressed (unclear on result)
                    errors = $"trouble logging in, error: {ex.Message}";
                }
            }

            return success;
        }
    }
}
