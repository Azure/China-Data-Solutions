using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Iotc.Web.Backend.Controllers
{
    public static class DateTimeExtension
    {
        public static DateTime FromJSDatetime(long jsdate)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).
                Add(TimeSpan.FromMilliseconds(jsdate)).ToUniversalTime();
        }

        public static long ToJSDatetime(this DateTime dt)
        {
            return (long)dt
                .ToUniversalTime()
                .Subtract(
                    new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                .TotalMilliseconds;
        }

        public static DateTime FromCubeDatetimeString(string cubeDatetimeString)
        {
            // 2012-06-01T00:00:00
            var ret = DateTime.SpecifyKind(
                DateTime.Parse(cubeDatetimeString),
                DateTimeKind.Utc);
            return ret;
        }

        public static string ToDBDatetimeString(this DateTime dt)
        {
            return dt.ToUniversalTime().ToString("MM/dd/yyyy");
        }


        public static DateTime FromDBDatetimeString(string cubeDatetimeString)
        {
            //// 6/3/2012 12:00:00 AM
            var ret = DateTime.SpecifyKind(
                DateTime.Parse(cubeDatetimeString),
                DateTimeKind.Utc);
            return ret;
        }

        public static string ToCubeDatetimeString(this DateTime dt)
        {
            var utc = dt.ToUniversalTime();
            if (utc.DayOfWeek != 0)
            {
                utc = utc.AddDays((double)(7 - utc.DayOfWeek));
            }

            return utc.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss");
        }
    }
}