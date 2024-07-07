using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordMyStats.xTests
{
    public class SetupFixture : IDisposable
    {
        public SetupFixture()
        {
            // Db = new SqlConnection("MyConnectionString");
            //int x = 1;
            // ... initialize data in the test database ...
        }

        public void Dispose()
        {
            //int y = 1;
            // ... clean up test data from the database ...
        }

        //public SqlConnection Db { get; private set; }
    }
}
