using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.ccfw.Utility
{
    /// <summary>
    /// 日志管理类
    /// Created by 林江雄 in 2014-05-08
    /// </summary>
    public class LogHelper
    {
        static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static void AddLog(object content)
        {
            logger.Info(content);
        }

        public static void AddLog(string jobName, string exceptionMsg)
        {
            logger.Info(string.Format("{0} execute failed. Message: {1}", jobName, exceptionMsg));
        }
    }
}
