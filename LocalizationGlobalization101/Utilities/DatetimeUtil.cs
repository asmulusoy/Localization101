using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalizationGlobalization101.Utilities
{
    public static class DatetimeUtil
    {
        public static DateTime FromUTCData(DateTime dt, int timezoneOffset)
        {
            DateTime newDate = dt + new TimeSpan(timezoneOffset / 60, timezoneOffset % 60, 0);
            return newDate;
        }
    }
}
