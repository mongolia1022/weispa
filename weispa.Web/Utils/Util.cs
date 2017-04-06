using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.weispa.Web.Utils
{
    public class WeispaUtil
    {
        public static bool CkeckOccurred(int start, int end, int a, int b)
        {
            return (start >= a && start < b) ||
                   (start <= a && end >= b) ||
                   (end > a && end < b);
        }

        public static string GetOccurredSql(int a, int b)
        {
            return string.Format("((StartTime>={0} AND StartTime<{1}) OR (StartTime<={0} AND EndTime>={1}) OR (EndTime>{0} AND EndTime<{1}))",a,b);
        }
    }
}