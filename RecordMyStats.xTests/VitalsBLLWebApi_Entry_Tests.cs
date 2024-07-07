using RecordMyStats.BLL;
using RecordMyStats.Common;
using RecordMyStats.Common.Entities;
using RecordMyStats.DataAccess.Data.Vitals;
using System;
using System.Linq;
using Xunit;

namespace RecordMyStats.xTests
{
    public class VitalsBLLWebApi_Entry_Tests
    {
        private Random rnd = new Random();
        private IVitalsBLL bll = new VitalsBLLWebApi();

        [Fact]
        public void GetEntriesBySessionKey_Test()
        {
            G.Init();
            var statEntries = bll.GetEntriesBySessionKey("A455FC3A1280429983B8", "", out string errors);

            Assert.True(string.IsNullOrWhiteSpace(errors), "some error encountered: " + errors);
            Assert.True(statEntries?.Any() ?? false, "no stat entries found, some expected");
        }

        [Fact]
        public void AddEntry_Test()
        {
            G.Init();
            StatisticEntry entry = new StatisticEntry()
            {
                Weight = 180 + TestUtils.UniqueValueSmall(),
                BloodSugar = 100 + TestUtils.UniqueValueSmall(),
                BPDiastolic = 120 + TestUtils.UniqueValueSmall(),
                BPSystolic = 80 + TestUtils.UniqueValueSmall(),
                BSUnits = "mg/dL",
                WeightUnits = "lbs",
                HeartRate = 70 + TestUtils.UniqueValueSmall(),
                CreateDate = DateTime.Now

            };

            bool result = bll.AddEntry(entry, "A455FC3A1280429983B8", "", out string errors);

            Assert.True(string.IsNullOrWhiteSpace(errors), "some error encountered: " + errors);
            Assert.True(result);
        }
    }
}