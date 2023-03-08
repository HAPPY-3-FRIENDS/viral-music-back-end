using System;

namespace BusinessObjects.Utils
{
    public class DateTimeUtil
    {
        public static string getTimestampNow()
        {
            return DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
        }
    }
}
