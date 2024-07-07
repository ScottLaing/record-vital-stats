using RecordMyStats.BLL;
using RecordMyStats.Common.Entities;
using RecordMyStats.DataAccess.Data.Vitals;
using System;
using Xunit;

namespace RecordMyStats.xTests
{
    public class VitalsRepository_Tests
    {
        private Random rnd = new Random();

        [Fact]
        public void AddMember_Test()
        {
            var member = new Member()
            {
                FirstName = "Jose" + UniqueValue(),
                LastName = "Raul" + UniqueValue(),
                MiddleName = "Quintero" + UniqueValue(),
                DateOfBirth = new System.DateTime(1950 + rnd.Next(1, 40), rnd.Next(1, 13), rnd.Next(1, 29)),
                Sex = "M",
                Zip = "92373",
                Country = "Spain",
                Email = "JoseRaul88233" + UniqueValue() + ".footballfan@hotmail.com",
                IsActive = true,
                Password = "Gandalf1*" + UniqueValue()
            };
            IVitalsRepository repos = new SqlServerVitalsRepository();
            bool result = repos.AddMember(member, out string errors, out string newSession);

            Assert.True(result, "some error encountered: " + errors);
        }

        [Fact]
        public void GetMemberDetailsByMemberId_Test()
        {
            SqlServerVitalsRepository repos = new SqlServerVitalsRepository();
            var member = repos.GetMemberDetailsByMemberId(3020, out string errors);

            Assert.True(string.IsNullOrWhiteSpace(errors), "some error encountered: " + errors);
            Assert.True(member != null, "member returned was null");
        }

        private string UniqueValue()
        {
            return rnd.Next(1000).ToString();
        }
    }
}