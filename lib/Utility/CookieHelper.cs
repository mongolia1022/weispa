using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;

namespace com.ccfw.Utility
{
    public class CookieHelper
    {
        public static string GetCookie(string name)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
            return ((cookie != null) ? cookie.Value : "");
        }

        public static string GetCookieByHead(string sInput)
        {
            List<string> list = CRegex.GetList(CRegex.GetText(sInput, @"Set-Cookie:(?<Cookie>[\s\S]+?)\n", "Cookie"), @"(?<cookie>[\w\d\.]+=[\w\d\._-]+);", "cookie");
            string str2 = "";
            foreach (string str3 in list)
            {
                str2 = str2 + str3 + "; ";
            }
            return str2;
        }

        public static List<CookieObj> GetCookieList(string sInput, List<CookieObj> listInput)
        {
            List<string> list = CRegex.GetList(CRegex.Replace(CRegex.GetText(sInput, @"Set-Cookie:(?<Cookie>[\s\S]+?)\n", "Cookie"), "expires=([^;]+)GMT;", "", 0).Replace("path=/", ""), @"(?<cookie>[\w\d\&\.=]+[\w\d\._-]+);", "cookie");
            CookieObj item = null;
            List<CookieObj> list2 = new List<CookieObj>();
            foreach (string str4 in list)
            {
                if (str4.IndexOf('=') >= 1)
                {
                    string str2 = str4.Substring(0, str4.IndexOf('='));
                    if (str2 != "domain")
                    {
                        string str3;
                        if (str4.Length < (str4.IndexOf('=') + 1))
                        {
                            str3 = "";
                        }
                        else
                        {
                            str3 = str4.Substring(str4.IndexOf('=') + 1);
                        }
                        item = new CookieObj
                        {
                            cookieName = str2,
                            cookieValue = str3
                        };
                        list2.Add(item);
                    }
                }
            }
            foreach (CookieObj obj4 in list2)
            {
                bool flag = false;
                for (int i = 0; i < listInput.Count; i++)
                {
                    CookieObj obj3 = listInput[i];
                    if (obj3.cookieName == obj4.cookieName)
                    {
                        flag = true;
                        obj3.cookieValue = obj4.cookieValue;
                    }
                }
                if (!flag)
                {
                    listInput.Add(obj4);
                }
            }
            return listInput;
        }

        public static void Remove(List<CookieObj> listInput, string CookieName)
        {
            for (int i = 0; i < listInput.Count; i++)
            {
                if (listInput[i].cookieName == CookieName)
                {
                    listInput.RemoveAt(i);
                }
            }
        }

        public static void SetCookie(string name, string value)
        {
            SetCookie(name, value, 420);
        }

        public static void SetCookie(List<CookieObj> listInput, string CookieName, string CookieValue)
        {
            foreach (CookieObj obj2 in listInput)
            {
                if (obj2.cookieName == CookieName)
                {
                    obj2.cookieValue = CookieValue;
                    return;
                }
            }
            CookieObj item = new CookieObj
            {
                cookieName = CookieName,
                cookieValue = CookieValue
            };
            listInput.Add(item);
        }

        public static void SetCookie(string name, string value, int iMinutes)
        {
            HttpResponse response = HttpContext.Current.Response;
            HttpCookie cookie = new HttpCookie(name)
            {
                Value = value,
                Expires = DateTime.Now.AddMinutes((double) iMinutes)
            };
            string str = ConfigurationManager.AppSettings["domain"];
            cookie.Domain = str;

            response.Cookies.Add(cookie);
        }

        public static void SetSessionCookie(string name, string value)
        {
            HttpResponse response = HttpContext.Current.Response;
            HttpCookie cookie = new HttpCookie(name)
            {
                Value = value
            };
            string str = ConfigurationManager.AppSettings["domain"];
            cookie.Domain = str;

            response.Cookies.Add(cookie);
        }
    }
}
