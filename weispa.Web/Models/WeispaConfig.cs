using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Configuration;
using com.ccfw.Utility;

namespace com.weispa.Web.Models
{
    public static class WeispaConfig
    {
        static WeispaConfig()
        {
            InvitationCodeKeeTime = ConvertHelper.StrToInt(WebConfigurationManager.AppSettings["InvitationCodeKeeTime"]);
            if(InvitationCodeKeeTime==0)
            InvitationCodeKeeTime = 60;
        }

        public static int ServiceMinutes = 90;
        public static int RestMinutes = 30;
        public static int LimitDays = 7;
        public static int LimitOrderCount = 2;

        public static string WxAppKey = WebConfigurationManager.AppSettings["WxAppKey"];
        public static string WxAppSecret = WebConfigurationManager.AppSettings["WxAppSecret"];
        public static int InvitationCodeKeeTime;
    }
}