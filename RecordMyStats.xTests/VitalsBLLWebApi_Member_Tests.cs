using RecordMyStats.BLL;
using RecordMyStats.Common.Entities;
using System;
using Xunit;

namespace RecordMyStats.xTests
{
    [Collection("Setup collection")]
    public class VitalsBLLWebApi_Member_Tests
    {
        private Random rnd = new Random();
        private IVitalsBLL bll = new VitalsBLLWebApi();

        [Fact]
        public void IsEmail_Test()
        {
            G.Init();
            string email = "scott.prime@gmail.com";
            bool result = bll.IsEmailInUse(email, out string errors);

            Assert.True(result, "some error encountered: " + errors);
        }

        [Fact]
        public void AddMember_Test()
        {
            G.Init();
            Member member = new Member()
            {
                LastName = "Smith",
                FirstName = "Michael",
                Country = "Spain",
                Email = $"michael.smith{TestUtils.UniqueValue()}@gmail.com",
                DateOfBirth = new DateTime(1900, 1, 1),
                Sex = "M",
                Zip = "78130",
                Password = "MichaelJohnson1*",
                MiddleName = "Andreeson"
            };

            bool result = bll.AddMember(member, out string errors, out string newSession, out string newToken);

            Assert.True(result, "some error encountered: " + errors);
            Assert.True(!string.IsNullOrWhiteSpace(newSession));
        }

        [Fact]
        public void LoginMember_Test()
        {
            G.Init();
            bool result = bll.LoginMember("michael.smith@gmail.com", "MichaelJohnson1*", out string sessionKey, out string fullName, out string token, out string errors);

            Assert.True(result, "some error encountered: " + errors);
            Assert.True(!string.IsNullOrWhiteSpace(sessionKey));
            Assert.True(!string.IsNullOrWhiteSpace(fullName));
            Assert.False(string.IsNullOrWhiteSpace(token));
        }

        [Fact]
        public void GetMemberInfoBySessionKey_Test()
        {
            G.Init();
            var member = bll.GetMemberInfoBySessionKey("A455FC3A1280429983B8", "", out string errors);

            Assert.True(member != null);
            Assert.True(! string.IsNullOrWhiteSpace(member?.FullName));
            Assert.True(string.IsNullOrWhiteSpace(errors));
        }
    }
}