using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RecordMyStats.Common;
using RecordMyStats.Common.Dto;
using RecordMyStats.Common.Entities;
using RecordMyStats.DataAccess.Data.Vitals;
using RecordMyStats.WebApi2.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RecordMyStats.WebApi2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MemberController : ControllerBase
    {
        private IVitalsRepository repos = VitalsRepositoryFactory.GetVitalsRepository();

        private readonly ILogger<MemberController> _logger;

        IConfiguration _configuration;

        public MemberController(ILogger<MemberController> logger, IConfiguration config)
        {
            _logger = logger;
            _configuration = config ?? throw new ArgumentNullException(nameof(config));
        }

        [HttpPost("IsEmailInUse")]
        public ActionResult<SimpleResultDto> IsEmailInUse(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                var simpleResult = new SimpleResultDto()
                {
                    Errors = Constants.RestStrings.RestParamsMissing,
                    Result = false
                };
                return Unauthorized(simpleResult);
            }
            bool result = repos.IsEmailInUse(email, out string errors);

            var lookupResult = new SimpleResultDto()
            {
                Errors = errors,
                Result = result
            };
            return Ok(lookupResult);
        }

        [HttpPost("AddMember")]
        public ActionResult<SessionResultDto> AddMember(Member member)
        {
            if (member == null || string.IsNullOrWhiteSpace(member.Email)
                                || string.IsNullOrWhiteSpace(member.LastName)
                                || string.IsNullOrWhiteSpace(member.FirstName)
                                || string.IsNullOrWhiteSpace(member.Password)
                                || string.IsNullOrWhiteSpace(member.Sex)
                                || string.IsNullOrWhiteSpace(member.Country))
            {
                var missingParams = new SessionResultDto()
                {
                    SessionKey = "",
                    Errors = Constants.RestStrings.RestParamsMissing,
                    Result = false,
                    Token = null
                };
                return Unauthorized(missingParams);
            }

            bool result = repos.AddMember(member, out string errors, out string newSessionKey);

            if (!result)
            {
                var unAuthResult = new SessionResultDto()
                {
                    SessionKey = newSessionKey,
                    Errors = errors,
                    Result = result,
                    Token = null
                };

                return Unauthorized(unAuthResult);
            }

            string userToken = TokenUtility.GetNewOauthToken(member.FullName, member?.Email ?? "", _configuration);
            if (string.IsNullOrWhiteSpace(userToken))
            {
                var unAuthResult = new SessionResultDto()
                {
                    SessionKey = newSessionKey,
                    Errors = "trouble getting Bearer token",
                    Result = false,
                    Token = null
                };

                return Unauthorized(unAuthResult);
            }

            var dtoResult = new SessionResultDto()
            {
                SessionKey = newSessionKey,
                Errors = errors,
                Result = result,
                Token = userToken
            };

            return Ok(dtoResult);
        }

        [HttpPost("LoginMember")]
        public ActionResult<LoginMemberResultDto> LoginMember(LoginInfoDto loginDto)
        {
            if (loginDto == null || string.IsNullOrWhiteSpace(loginDto.Email) || string.IsNullOrWhiteSpace(loginDto.Password))
            {
                var missingParams = new LoginMemberResultDto()
                {
                    SessionKey = null,
                    FullName = null,
                    Errors = Constants.RestStrings.RestParamsMissing,
                    Result = false,
                    Token = null
                };
                return Unauthorized(missingParams);
            }

            bool result = repos.LoginMember(loginDto.Email, loginDto.Password, out string sessionKey, out string fullName, out string errors);

            if (!result)
            {
                var unAuthResult =  new LoginMemberResultDto()
                {
                    SessionKey = null,
                    FullName = null,
                    Errors = Constants.RestStrings.RestParamsMissing,
                    Result = false,
                    Token = null
                };

                return Unauthorized(unAuthResult);
            }

            var userToken = TokenUtility.GetNewOauthToken(fullName, loginDto.Email, _configuration);

            var dtoResult = new LoginMemberResultDto()
            {
                SessionKey = sessionKey,
                FullName = fullName,
                Errors = errors,
                Result = result,
                Token = userToken
            };

            return Ok(dtoResult);
        }

        [Authorize]
        [HttpPost("GetMemberInfoBySessionKey")]
        public ActionResult<MemberResultDto> GetMemberInfoBySessionKey(string sessionKey)
        {
            if (string.IsNullOrWhiteSpace(sessionKey))
            {
                var errResult = new MemberResultDto()
                {
                    Member = null,
                    Errors = Constants.RestStrings.RestParamsMissing,
                    Result = false
                };

                return new ObjectResult("GetMemberInfoBySessionKey missing required parameters") { StatusCode = StatusCodes.Status406NotAcceptable };
            }
            var member = repos.GetMemberInfoBySessionKey(sessionKey, out string errors);

            var successResult = new MemberResultDto()
            {
                Member = member,
                Errors = errors,
                Result = string.IsNullOrWhiteSpace(errors) && member != null
            };

            return Ok(successResult);
        }
    }
}