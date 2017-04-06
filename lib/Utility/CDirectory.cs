using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace com.ccfw.Utility
{
    public class CDirectory
    {
        public static string ConvertToBaseWebSitePath(string pathShare)
        {
            return ConvertToBaseWebSitePath(pathShare, "/html/xml");
        }

        public static string ConvertToBaseWebSitePath(string pathShare, string webPath)
        {
            string[] strArray = pathShare.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            if (strArray.Length > 0)
            {
                return Path.Combine(HttpContext.Current.Server.MapPath(webPath), strArray[strArray.Length - 1]);
            }
            return HttpContext.Current.Server.MapPath(webPath);
        }

        public static void Create(string path)
        {
            try
            {
                if (!string.IsNullOrEmpty(path) && !Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch
            {
            }
        }

        public static void Delete(string sPath)
        {
            string[] files = Directory.GetFiles(sPath);
            foreach (string str in files)
            {
                try
                {
                    CFile.Delete(str);
                }
                catch
                {
                }
            }
            string[] directories = Directory.GetDirectories(sPath);
            foreach (string str2 in directories)
            {
                try
                {
                    Delete(str2);
                }
                catch
                {
                }
            }
            FileInfo info = new FileInfo(sPath)
            {
                Attributes = FileAttributes.Normal
            };
            Directory.Delete(sPath);
        }

        public static List<string> GetDirectories(string sPath)
        {
            return GetDirectories(sPath, true);
        }

        public static List<string> GetDirectories(string sPath, bool IsFull)
        {
            string[] directories = null;
            try
            {
                directories = Directory.GetDirectories(sPath);
            }
            catch
            {
            }
            List<string> list = new List<string>();
            if (directories != null)
            {
                foreach (string str in directories)
                {
                    if (IsFull)
                    {
                        list.Add(str);
                    }
                    else
                    {
                        list.Add(str.Replace(sPath, "").TrimStart(new char[] { '\\', '/' }));
                    }
                }
            }
            return list;
        }

        public static List<string> GetFiles(string sPath)
        {
            return GetFiles(sPath, true);
        }

        public static List<string> GetFiles(string sPath, bool IsFull)
        {
            string[] files = null;
            try
            {
                files = Directory.GetFiles(sPath);
            }
            catch
            {
            }
            List<string> list = new List<string>();
            if (files != null)
            {
                foreach (string str in files)
                {
                    if (IsFull)
                    {
                        list.Add(str);
                    }
                    else
                    {
                        list.Add(str.Replace(sPath, "").TrimStart(new char[] { '\\', '/' }));
                    }
                }
            }
            return list;
        }

        public static string GetPathByWeb(string sWebPath)
        {
            if (sWebPath == "")
            {
                throw new Exception("<font color='red'>未指定路径</font>");
            }
            int num = sWebPath.LastIndexOf("/");
            if (num < 0)
            {
                throw new Exception("font color='red'>指定路径格式不正确</font>");
            }
            string str = sWebPath.Substring(num + 1);
            if (str.IndexOf('.') <= 0)
            {
                throw new Exception("<font color='red'>文件名格式不对</font>");
            }
            string path = sWebPath.Substring(0, sWebPath.LastIndexOf("/") + 1);
            path = HttpContext.Current.Server.MapPath(path);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (!path.EndsWith(@"\"))
            {
                path = path + @"\";
            }
            return (path + str);
        }
    }
}
