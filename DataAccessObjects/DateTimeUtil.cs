using System;

namespace BusinessObjects.Utils
{
    public class DateTimeUtil
    {
        public static string getTimestampNow()
        {
            return DateTime.Now.ToString("dd/MM/yyyy - HH:mm:ss");
        }
    }
}
