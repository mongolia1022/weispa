using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.weispa.Web
{
    public static class ModelExt
    {
        public static string ToMinuteStr(this DateTime time)
        {
            return time.ToString("yyyy-MM-dd HH:mm");
        }

        public static string ToHHmm(this DateTime time)
        {
            return time.ToString("HH:mm");
        }

        public static string ToDayStr(this DateTime time)
        {
            return time.ToString("MM/dd");
        }

        public static string ToWeekDayStr(this DateTime time,string type="周")
        {
            string val = "";
            switch (time.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    val= "日";
                    break;
                case DayOfWeek.Monday:
                    val= "一";break;
                case DayOfWeek.Tuesday:
                    val = "二";break;
                case DayOfWeek.Wednesday:
                    val = "三";break;
                case DayOfWeek.Thursday:
                    val = "四";break;
                case DayOfWeek.Friday:
                    val = "五";break;
                case DayOfWeek.Saturday:
                    val = "六";break;
            }

            return type+val;
        }

        
    }
}