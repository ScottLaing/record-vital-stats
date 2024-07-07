//using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace RecordMyStats.WebApi2.Controllers
//{
//    [Route("api/authentication")]
//    [ApiController]
//    public class AuthenticationController : ControllerBase
//    {
//        IConfiguration _configuration;
//        public AuthenticationController(IConfiguration config)
//        {
//            _configuration = config ??
//                throw new ArgumentNullException(nameof(config));
//        }

//        // POST api/<AuthenticationController>
//        //[HttpPost("authenticate")]
//        //public ActionResult<string> Authenticate(AuthenticationRequestBody request)
//        //{
//        //    var user = ValidateUserCredentials(request.UserName, request.Password);

//        //    if (user == null)
//        //    {
//        //        return Unauthorized();
//        //    }

//        //    var securityKey = new SymmetricSecurityKey(
//        //        Encoding.ASCII.GetBytes(_configuration["Authentication:SecretKey"]));

//        //    var signingCredentials = new SigningCredentials(
//        //        securityKey, SecurityAlgorithms.HmacSha256);

//        //    var claimsInfo = new List<Claim>();
//        //    claimsInfo.Add(new Claim(ClaimTypes.GivenName, user?.FirstName ?? ""));
//        //    claimsInfo.Add(new Claim(ClaimTypes.Surname, user?.LastName ?? ""));
//        //    claimsInfo.Add(new Claim(ClaimTypes.Email, user?.Email ?? ""));

//        //    var jwtSecurityToken = new JwtSecurityToken(
//        //        _configuration["Authentication:Issuer"],
//        //        _configuration["Authentication:Audience"],
//        //        claimsInfo,
//        //        DateTime.UtcNow,
//        //        DateTime.UtcNow.AddMinutes(20),
//        //        signingCredentials);

//        //    var userToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

//        //    return Ok(userToken);

//        //}
        
//        private SiteUser ValidateUserCredentials(string ? userName, string ? password)
//        {
//            return new SiteUser(100, "LoginName", "firstname", "lastname", "username@domain.com");
//        }

//    }

//    public class AuthenticationRequestBody
//    {
//        public string? UserName { get; set; }
//        public string? Password { get; set; }
//    }

//    public class SiteUser
//    {
//        public int Id { get; set; }
//        public string? LoginName { get; set; }
//        public string? FirstName { get; set; }
//        public string? LastName { get; set; }
//        public string? Email { get; set; }

//        public SiteUser(int id, string loginName,
//            string firstName, string lastName, string email)
//        {
//            this.Id = id;
//            this.LoginName = loginName;
//            this.FirstName = firstName;
//            this.LastName = lastName;
//            this.Email = email;
//        }
//    }
//}
