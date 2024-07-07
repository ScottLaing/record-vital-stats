using Microsoft.IdentityModel.Tokens;
using RecordMyStats.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RecordMyStats.WebApi2.Services
{
    public class TokenUtility
    {
        private const string OAuthClaimEmailAddressUri = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";

        // this is WIP stuff, I'm seeing if I can get information like email directly off the oath token.
        // todo: genericize this to get any claims info we can off the OAuth token in a flexble way
        // todo: should we put more stuff in the oath token claims? would this improve security in a small way or be helpful?
        public static string GetEmailFromToken(HttpRequest request, IConfiguration configuration)
        {
            try
            {
                string secretKey = Config.Secrets.JwtTokenPhrase;

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(secretKey);
                var authToken = request.Headers["Authorization"];
                var tokenStr = authToken.ToString();
                tokenStr = tokenStr.Replace("Bearer ", "");

                tokenHandler.ValidateToken(tokenStr, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Authentication:Issuer"],
                    ValidAudience = configuration["Authentication:Audience"],
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                var email = jwtToken.Claims.First(x => x.Type == OAuthClaimEmailAddressUri).Value;
                return email;
            }
            catch
            {
                return "error";
            }
        }

        internal static string GetNewOauthToken(string fullName, string email, IConfiguration configuration)
        {
            if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email))
            {
                return "";
            }

            var configSecretKeyByteArray = Encoding.ASCII.GetBytes(Config.Secrets.JwtTokenPhrase);
            var securityKey = new SymmetricSecurityKey(configSecretKeyByteArray);
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claimsInfo = new List<Claim>();
            claimsInfo.Add(new Claim(ClaimTypes.Name, fullName));
            claimsInfo.Add(new Claim(ClaimTypes.Email, email));

            var issuer = configuration["Authentication:Issuer"];
            var audience = configuration["Authentication:Audience"];
            var jwtSecurityToken = new JwtSecurityToken(
                issuer,
                audience,
                claimsInfo,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(Constants.TokenOptions.TokenExpireMinutes).AddHours(Constants.TokenOptions.TokenExpireHours),
                signingCredentials);

            var userToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return userToken;
        }
    }
}
