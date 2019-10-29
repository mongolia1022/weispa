using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

        public static string HttpGet(string url, string contentType = "application/json", Dictionary<string, string> headers = null)
        {
            using (HttpClient client = new HttpClient())
            {
                if (contentType != null)
                    client.DefaultRequestHeaders.Add("ContentType", contentType);
                if (headers != null)
                {
                    foreach (var header in headers)
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
                HttpResponseMessage response = client.GetAsync(url).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }
    }
}