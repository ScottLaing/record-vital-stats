using RecordMyStats.Common;

namespace RecordMyStats.BLL
{
    public class VitalsFactory
    {
        public static IVitalsBLL GetVitalsBLL()
        {
            if (Config.AppSettings.UsingRestApi)
            {
                return new VitalsBLLWebApi();
            }
            else
            {
                return new VitalsBLLDirectAccess();
            }
        }
    }
}
