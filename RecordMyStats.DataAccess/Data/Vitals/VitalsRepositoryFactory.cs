using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordMyStats.DataAccess.Data.Vitals
{
    public class VitalsRepositoryFactory
    {
        public static IVitalsRepository GetVitalsRepository()
        {
            if (Common.Config.AppSettings.UsingPostgres)
            {
                return new PostgresVitalsRepository();
            }
            else
            {
                return new SqlServerVitalsRepository();
            }
        }
    }
}
