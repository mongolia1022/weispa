using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace com.ccfw.Utility
{
    public class CWebRequest
    {
        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        public static byte[] GetBypes(RequestArgs mReq)
        {
            byte[] array = null;
            GZipStream stream3;
            byte[] buffer3;
            bool flag;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(mReq.Url);
            request.Method = mReq.Method;
            request.Accept = "*/*";
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; Maxthon; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
            if (mReq.cookie != string.Empty)
            {
                request.Headers.Add(HttpRequestHeader.Cookie, mReq.cookie);
            }
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            request.KeepAlive = true;
            if (mReq.RefererUrl != "")
            {
                request.Referer = mReq.RefererUrl;
            }
            request.AllowAutoRedirect = mReq.blRedirect;
            if (mReq.blPolicy)
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CWebRequest.CheckValidationResult);
            }
            if (mReq.Method == "POST")
            {
                StreamWriter writer = null;
                writer = new StreamWriter(request.GetRequestStream());
                writer.Write(mReq.postData);
                writer.Close();
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            byte[] buffer = new byte[0x7d000];
            int num = 0;
            int offset = 0;
            while (true)
            {
                flag = true;
                num = responseStream.Read(buffer, offset, buffer.Length - offset);
                if (num <= 0)
                {
                    if (!(response.ContentEncoding == "gzip"))
                    {
                        array = new byte[offset];
                        for (int i = 0; i < offset; i++)
                        {
                            array[i] = buffer[i];
                        }
                        goto Label_0235;
                    }
                    MemoryStream stream = new MemoryStream(buffer, 0, offset, true);
                    stream3 = new GZipStream(stream, CompressionMode.Decompress, true);
                    stream.Seek(0L, SeekOrigin.Begin);
                    buffer3 = new byte[0x4b000];
                    offset = 0;
                    break;
                }
                offset += num;
            }
        Label_01E8:
            flag = true;
            num = stream3.Read(buffer3, offset, 0x400);
            if (num == 0)
            {
                stream3.Close();
                array = new byte[offset];
                buffer3.CopyTo(array, offset);
            }
            else
            {
                offset += num;
                goto Label_01E8;
            }
        Label_0235:
            request.Abort();
            response.Close();
            return array;
        }

        public static string GetHead(string strUrl)
        {
            string str = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strUrl);
            request.Method = "GET";
            request.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/msword, */*";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; Maxthon; .NET CLR 1.1.4322;)";
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-cn");
            request.KeepAlive = true;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            foreach (string str2 in response.Headers)
            {
                string str4 = str;
                str = str4 + str2 + ":" + response.Headers[str2] + "\r\n";
            }
            request.Abort();
            response.Close();
            return str;
        }

        public static string GetPost(RequestArgs mReq)
        {
            HttpWebResponse response;
            GZipStream stream3;
            byte[] buffer2;
            string str2;
            bool flag;
            string str = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(mReq.Url);
            request.Method = mReq.Method;
            request.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/msword, */*";
            if (string.IsNullOrEmpty(mReq.charset))
            {
                request.ContentType = "application/x-www-form-urlencoded";
            }
            else
            {
                request.ContentType = "application/x-www-form-urlencoded; charset=" + mReq.charset;
            }
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; Maxthon; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
            if (mReq.cookie != string.Empty)
            {
                request.Headers.Add(HttpRequestHeader.Cookie, mReq.cookie);
            }
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-cn");
            request.KeepAlive = true;
            if (mReq.RefererUrl != "")
            {
                request.Referer = mReq.RefererUrl;
            }
            request.AllowAutoRedirect = mReq.blRedirect;
            if (mReq.blPolicy)
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CWebRequest.CheckValidationResult);
            }
            if (mReq.Method == "POST")
            {
                StreamWriter writer = null;
                writer = new StreamWriter(request.GetRequestStream());
                writer.Write(mReq.postData);
                writer.Close();
            }
            Encoding encoding = Encoding.UTF8;
            string str5 = mReq.Encode.ToUpper();
            if (str5 != null)
            {
                if (!(str5 == "UTF-8"))
                {
                    if (str5 == "GB2312")
                    {
                        encoding = Encoding.GetEncoding("gb2312");
                        goto Label_01DA;
                    }
                    if (str5 == "BIG5")
                    {
                        encoding = Encoding.GetEncoding("BIG5");
                        goto Label_01DA;
                    }
                }
                else
                {
                    encoding = Encoding.UTF8;
                    goto Label_01DA;
                }
            }
            encoding = Encoding.GetEncoding("gb2312");
        Label_01DA:
            response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            if (!(response.ContentEncoding == "gzip"))
            {
                StreamReader reader = new StreamReader(responseStream, encoding);
                str = reader.ReadToEnd();
                reader.Close();
                goto Label_02E9;
            }
            byte[] buffer = new byte[0x19000];
            int num = 0;
            int offset = 0;
            while (true)
            {
                flag = true;
                num = responseStream.Read(buffer, offset, buffer.Length - offset);
                if (num <= 0)
                {
                    MemoryStream stream = new MemoryStream(buffer, 0, offset, true);
                    stream3 = new GZipStream(stream, CompressionMode.Decompress, true);
                    stream.Seek(0L, SeekOrigin.Begin);
                    buffer2 = new byte[0x4b000];
                    offset = 0;
                    break;
                }
                offset += num;
            }
        Label_02B1:
            flag = true;
            num = stream3.Read(buffer2, offset, 0x400);
            if (num == 0)
            {
                stream3.Close();
                str = encoding.GetString(buffer2, 0, offset);
            }
            else
            {
                offset += num;
                goto Label_02B1;
            }
        Label_02E9:
            str2 = "";
            if (mReq.blGetHeaders)
            {
                foreach (string str3 in response.Headers)
                {
                    string str6 = str2;
                    str2 = str6 + str3 + ":" + response.Headers[str3] + "\r\n";
                }
            }
            str = str2 + str;
            request.Abort();
            response.Close();
            return str;
        }

        public static HttpWebRequest GetRequest(string sUrl, string sReferer, string sCookie)
        {
            try
            {
                Uri requestUri = new Uri(sUrl);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUri);
                request.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/msword, application/vnd.ms-excel, application/x-shockwave-flash, application/vnd.ms-powerpoint, */*";
                request.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-cn");
                if (sReferer != "")
                {
                    request.Referer = sReferer;
                }
                else
                {
                    request.Referer = "http://" + requestUri.Host;
                }
                request.Headers.Add("UA-CPU:x86");
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1;)";
                request.KeepAlive = true;
                if (sCookie != "")
                {
                    request.Headers.Add(HttpRequestHeader.Cookie, sCookie);
                }
                request.Timeout = 0x2710;
                return request;
            }
            catch
            {
                return null;
            }
        }

        private static byte[] HttpDecompress(HttpWebRequest req)
        {
            return HttpDecompress((HttpWebResponse)req.GetResponse());
        }

        private static byte[] HttpDecompress(HttpWebResponse resp)
        {
            int offset = 0;
            byte[] buffer = null;
            if (resp != null)
            {
                MemoryStream stream2;
                Stream responseStream = null;
                try
                {
                    bool flag;
                    responseStream = resp.GetResponseStream();
                    byte[] buffer2 = new byte[0x19000];
                    int num2 = 0;
                    goto Label_0057;
                Label_0034:
                    num2 = responseStream.Read(buffer2, offset, buffer2.Length - offset);
                    if (num2 <= 0)
                    {
                        goto Label_005C;
                    }
                    offset += num2;
                Label_0057:
                    flag = true;
                    goto Label_0034;
                Label_005C:
                    if (!(resp.ContentEncoding == "gzip"))
                    {
                        goto Label_00DE;
                    }
                    stream2 = new MemoryStream(buffer2, 0, offset, true);
                    GZipStream stream3 = new GZipStream(stream2, CompressionMode.Decompress, true);
                    stream2.Seek(0L, SeekOrigin.Begin);
                    buffer = new byte[0x100000];
                    offset = 0;
                    goto Label_00CE;
                Label_00A6:
                    num2 = stream3.Read(buffer, offset, 0x400);
                    if (num2 == 0)
                    {
                        goto Label_00D3;
                    }
                    offset += num2;
                Label_00CE:
                    flag = true;
                    goto Label_00A6;
                Label_00D3:
                    stream3.Close();
                    goto Label_00E2;
                Label_00DE:
                    buffer = buffer2;
                Label_00E2:
                    responseStream.Close();
                    resp.Close();
                }
                catch
                {
                    if (responseStream != null)
                    {
                        responseStream.Close();
                    }
                    return null;
                }
                if (offset > 0)
                {
                    stream2 = new MemoryStream(buffer, 0, offset);
                    buffer = stream2.ToArray();
                    stream2.Close();
                    return buffer;
                }
            }
            return null;
        }

        private static string HttpDecompress(HttpWebRequest req, Encoding encoding)
        {
            if (req == null)
            {
                return "";
            }
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            return HttpDecompress(resp, encoding);
        }

        private static string HttpDecompress(HttpWebResponse resp, Encoding encoding)
        {
            GZipStream stream3;
            byte[] buffer2;
            bool flag;
            if (resp.StatusCode != HttpStatusCode.OK)
            {
                return "";
            }
            string str = "";
            Stream responseStream = resp.GetResponseStream();
            if (!(resp.ContentEncoding == "gzip"))
            {
                StreamReader reader = new StreamReader(responseStream, encoding);
                str = reader.ReadToEnd();
                responseStream.Close();
                reader.Close();
                return str;
            }
            byte[] buffer = new byte[0x19000];
            int num = 0;
            int offset = 0;
            while (true)
            {
                flag = true;
                num = responseStream.Read(buffer, offset, buffer.Length - offset);
                if (num <= 0)
                {
                    MemoryStream stream = new MemoryStream(buffer, 0, offset, true);
                    stream3 = new GZipStream(stream, CompressionMode.Decompress, true);
                    stream.Seek(0L, SeekOrigin.Begin);
                    buffer2 = new byte[0xfa000];
                    offset = 0;
                    break;
                }
                offset += num;
            }
            while (true)
            {
                flag = true;
                num = stream3.Read(buffer2, offset, 0x400);
                if (num == 0)
                {
                    stream3.Close();
                    responseStream.Close();
                    resp.Close();
                    return encoding.GetString(buffer2, 0, offset);
                }
                offset += num;
            }
        }

        public static string ReadHtml(string sUrl, string sCoding)
        {
            return ReadHtml(sUrl, sCoding, "", "");
        }

        public static string ReadHtml(string sUrl, string sCoding, string sReferer, string sCookie)
        {
            try
            {
                return HttpDecompress(GetRequest(sUrl, sReferer, sCookie), Encoding.GetEncoding(sCoding));
            }
            catch (Exception exception)
            {
                string message = exception.Message;
                return "";
            }
        }

        public static string ReadHtml(string sUrl, string sCoding, string sReferer, string sCookie, out string sOutReferer, out string sOutCookie)
        {
            string str = "";
            try
            {
                HttpWebResponse resp = (HttpWebResponse)GetRequest(sUrl, sReferer, sCookie).GetResponse();
                if (resp.Headers["Set-Cookie"] != "")
                {
                    sOutCookie = resp.Headers["Set-Cookie"];
                }
                else
                {
                    sOutCookie = sCookie;
                }
                sOutReferer = sUrl;
                str = HttpDecompress(resp, Encoding.GetEncoding(sCoding));
            }
            catch
            {
                str = "";
                sOutCookie = sCookie;
                sOutReferer = sReferer;
            }
            return str;
        }
    }
}
