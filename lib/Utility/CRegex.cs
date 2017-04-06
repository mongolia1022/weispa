using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace com.ccfw.Utility
{
    public class CRegex
    {
        public static readonly string sIFrameReg = @"<iframe[\s\S]+?</iframe>";
        public static readonly string sImgReg = "<img[^>]+src=\\s*(?:'(?<src>[\\w/\\./:-])'|\"(?<src>[\\w\\./:-]+)\"|(?<src>[\\w\\./:-]+))[^>]*>";
        public static readonly string sScriptReg = @"<script[\s\S]+?</script>";

        public static bool CheckUrl(string sInput)
        {
            string pattern = @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";
            return Regex.Match(sInput, pattern, RegexOptions.IgnoreCase).Success;
        }

        public static string GetAuthor(string sInput, string sRegex)
        {
            string str = CString.ClearTag(GetText(sInput, sRegex, "Author"));
            if (str.Length > 0x63)
            {
                str = str.Substring(0, 0x63);
            }
            return str;
        }

        public static string GetBody(string sInput)
        {
            return GetText(sInput, @"<Body[^>]*>(?<Body>[\s\S]{10,})</body>", "Body");
        }

        public static string GetBody(string sInput, string sRegex)
        {
            return GetText(sInput, sRegex, "Body");
        }

        public static string GetContent(string sOriContent, string sOtherRemoveReg, string sPageUrl)
        {
            string input = sOriContent;
            input = Regex.Replace(Regex.Replace(input, @"<script[\s\S]*?</script>", "", RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.IgnoreCase), @"<iframe[^>]*>[\s\S]*?</iframe>", "", RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.IgnoreCase);
            string[] strArray = sOtherRemoveReg.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string str2 in strArray)
            {
                input = Replace(input, str2, "", 0);
            }
            MatchCollection matchs = new Regex("<img[^>]+src=\\s*(?:'(?<src>[^']+)'|\"(?<src>[^\"]+)\"|(?<src>[^>\\s]+))\\s*[^>]*>", RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.IgnoreCase).Matches(input);
            string oldValue = "";
            string newValue = "";
            foreach (Match match in matchs)
            {
                oldValue = match.Value;
                newValue = oldValue.Replace(match.Groups["src"].Value, GetUrl(sPageUrl, match.Groups["src"].Value));
                input = input.Replace(oldValue, newValue);
            }
            matchs = new Regex("<a[^>]+href=\\s*(?:'(?<href>[^']+)'|\"(?<href>[^\"]+)\"|(?<href>[^>\\s]+))\\s*[^>]*>", RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.IgnoreCase).Matches(input);
            oldValue = "";
            newValue = "";
            foreach (Match match in matchs)
            {
                oldValue = match.Value;
                newValue = Replace(oldValue, @"<[^>]*\ba\b[^>]*>", "", 0);
                input = input.Replace(oldValue, newValue);
            }
            return input;
        }

        public static DateTime GetCreateDate(string sInput, string sRegex)
        {
            DateTime now = DateTime.Now;
            Match match = new Regex(sRegex, RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.IgnoreCase).Match(sInput);
            if (match.Success)
            {
                try
                {
                    int year = int.Parse(match.Groups["Year"].Value);
                    int month = int.Parse(match.Groups["Month"].Value);
                    int day = int.Parse(match.Groups["Day"].Value);
                    int hour = now.Hour;
                    int minute = now.Minute;
                    string s = match.Groups["Hour"].Value;
                    string str2 = match.Groups["Mintue"].Value;
                    if (s != "")
                    {
                        hour = int.Parse(s);
                    }
                    if (str2 != "")
                    {
                        minute = int.Parse(str2);
                    }
                    now = new DateTime(year, month, day, hour, minute, 0);
                }
                catch
                {
                }
            }
            return now;
        }

        public static string GetDomain(string sInput)
        {
            return GetText(sInput, @"http(s)?://([\w-]+\.)+(\w){2,}", 0);
        }

        public static string GetHost(string sInput)
        {
            return GetText(sInput, @"(http(s)?://)?(?<Host>([\w-]+\.)+(\w){2,})", "Host");
        }

        public static string GetHtml(string sInput)
        {
            return Replace(sInput, "(?<Head>[^<]+)<", "", "Head");
        }

        public static string GetImgSrc(string sInput)
        {
            return GetText(sInput, "<img[^>]+src=\\s*(?:'(?<src>[^']+)'|\"(?<src>[^\"]+)\"|(?<src>[^>\\s]+))\\s*[^>]*>", "src");
        }

        public static List<string> GetImgTag(string sInput)
        {
            return GetList(sInput, "<img[^>]+src=\\s*(?:'(?<src>[^']+)'|\"(?<src>[^\"]+)\"|(?<src>[^>\\s]+))\\s*[^>]*>", "");
        }

        public static string GetKeyWord(string sInput)
        {
            List<string> list = Split(sInput, @"(,|，|\+|＋|。|;|；|：|:|“)|”|、|_|\(|（|\)|）", 2);
            List<string> list2 = new List<string>();
            foreach (string str in list)
            {
                Regex regex = new Regex("[a-zA-z]+", RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);
                MatchCollection matchs = regex.Matches(str);
                string input = str;
                foreach (Match match in matchs)
                {
                    if (match.Value.ToString().Length > 2)
                    {
                        list2.Add(match.Value.ToString());
                    }
                    input = input.Replace(match.Value.ToString(), ",");
                }
                regex = new Regex(",{1}", RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);
                matchs = regex.Matches(input);
                foreach (string str3 in regex.Split(input))
                {
                    if (str3.Trim().Length > 2)
                    {
                        list2.Add(str3);
                    }
                }
            }
            string str4 = "";
            for (int i = 0; i < (list2.Count - 1); i++)
            {
                for (int j = i + 1; j < list2.Count; j++)
                {
                    if (list2[i] == list2[j])
                    {
                        list2[j] = "";
                    }
                }
            }
            foreach (string str in list2)
            {
                if (str.Length > 2)
                {
                    str4 = str4 + str + ",";
                }
            }
            if (str4.Length > 0)
            {
                str4 = str4.Substring(0, str4.Length - 1);
            }
            else
            {
                str4 = sInput;
            }
            if (str4.Length > 0x63)
            {
                str4 = str4.Substring(0, 0x63);
            }
            return str4;
        }

        public static string GetLink(string sInput)
        {
            return GetText(sInput, "<a[^>]+href=\\s*(?:'(?<href>[^']+)'|\"(?<href>[^\"]+)\"|(?<href>[^>\\s]+))\\s*[^>]*>", "href");
        }

        public static List<string> GetLinks(string sInput)
        {
            return GetList(sInput, "<a[^>]+href=\\s*(?:'(?<href>[^']+)'|\"(?<href>[^\"]+)\"|(?<href>[^>\\s]+))\\s*[^>]*>", "href");
        }

        public static List<string> GetLinks(string sInput, string sContentUrlRule)
        {
            return GetList(sInput, "<a[^>]+href=\\s*(?:'(?<href>[^']+)'|\"(?<href>[^\"]+)\"|(?<href>[^>\\s]+))\\s*[^>]*>", "href", sContentUrlRule);
        }

        public static List<string> GetList(string sInput, string sRegex, int iGroupIndex)
        {
            List<string> list = new List<string>();
            MatchCollection matchs = new Regex(sRegex, RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.IgnoreCase).Matches(sInput);
            foreach (Match match in matchs)
            {
                if (iGroupIndex > 0)
                {
                    list.Add(match.Groups[iGroupIndex].Value);
                }
                else
                {
                    list.Add(match.Value);
                }
            }
            return list;
        }

        public static List<string> GetList(string sInput, string sRegex, string sGroupName)
        {
            List<string> list = new List<string>();
            MatchCollection matchs = new Regex(sRegex, RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.IgnoreCase).Matches(sInput);
            foreach (Match match in matchs)
            {
                if (sGroupName != "")
                {
                    list.Add(match.Groups[sGroupName].Value);
                }
                else
                {
                    list.Add(match.Value);
                }
            }
            return list;
        }

        public static List<string> GetList(string sInput, string sRegex, string sGroupName, string sContentUrlRule)
        {
            List<string> list = new List<string>();
            MatchCollection matchs = new Regex(sRegex, RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.IgnoreCase).Matches(sInput);
            foreach (Match match in matchs)
            {
                if (sGroupName != "")
                {
                    if (IsPassUrlRule(match.Groups[sGroupName].Value, sContentUrlRule))
                    {
                        list.Add(match.Groups[sGroupName].Value);
                    }
                }
                else if (IsPassUrlRule(match.Value, sContentUrlRule))
                {
                    list.Add(match.Value);
                }
            }
            return list;
        }

        public static List<string> GetPageLinks(string sInput, string sRegex)
        {
            return GetList(sInput, sRegex, "href");
        }

        public static int GetPostID(string sInput)
        {
            return int.Parse(GetText(sInput, @"http://community.39.net/Topics/[\w]+/(?<ID>[\d]+).xml", "ID"));
        }

        public static string GetSource(string sInput, string sRegex)
        {
            string str = CString.ClearTag(GetText(sInput, sRegex, "Source"));
            if (str.Length > 0x63)
            {
                str = str.Substring(0, 0x63);
            }
            return str;
        }

        public static string GetText(string sInput, string sRegex, int iGroupIndex)
        {
            Match match = new Regex(sRegex, RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.IgnoreCase).Match(sInput);
            if (match.Success)
            {
                if (iGroupIndex > 0)
                {
                    return match.Groups[iGroupIndex].Value;
                }
                return match.Value;
            }
            return "";
        }

        public static string GetText(string sInput, string sRegex, string sGroupName)
        {
            Match match = new Regex(sRegex, RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.IgnoreCase).Match(sInput);
            if (match.Success)
            {
                if (sGroupName != "")
                {
                    return match.Groups[sGroupName].Value;
                }
                return match.Value;
            }
            return "";
        }

        public static string GetTitle(string sInput, string sRegex)
        {
            string str = CString.ClearTag(GetText(sInput, sRegex, "Title"));
            if (str.Length > 0x63)
            {
                str = str.Substring(0, 0x63);
            }
            return str;
        }

        public static string GetUrl(string sInput, string sRelativeUrl)
        {
            string sOrg = sInput.Trim();
            if (sRelativeUrl.ToLower().StartsWith("http") || sRelativeUrl.ToLower().StartsWith("https"))
            {
                return sRelativeUrl.Trim();
            }
            if (sRelativeUrl.StartsWith("/"))
            {
                return (GetDomain(sInput) + sRelativeUrl);
            }
            if (sRelativeUrl.StartsWith("../"))
            {
                while (sRelativeUrl.IndexOf("../") >= 0)
                {
                    sOrg = CString.GetPreStrByLast(sOrg, "/");
                    sRelativeUrl = sRelativeUrl.Substring(3);
                }
                return (sOrg + "/" + sRelativeUrl.Trim());
            }
            if (sRelativeUrl.Trim() != "")
            {
                return (CString.GetPreStrByLast(sInput, "/") + "/" + sRelativeUrl);
            }
            sRelativeUrl = sInput;
            return "";
        }

        public static bool IsDomain(string sInput)
        {
            string sRegex = @"^([\w-]+\.)+(\w){2,}$";
            return IsMatch(sInput, sRegex);
        }

        public static bool IsMatch(string sInput, string sRegex)
        {
            Regex regex = new Regex(sRegex, RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.IgnoreCase);
            return regex.Match(sInput).Success;
        }

        private static bool IsPassUrlRule(string sValue, string sRule)
        {
            if ((sRule != "") && (sRule != null))
            {
                Regex regex = new Regex(sRule, RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.IgnoreCase);
                return regex.IsMatch(sValue);
            }
            return true;
        }

        public static string Replace(string sInput, string sRegex, string sReplace, int iGroupIndex)
        {
            MatchCollection matchs = new Regex(sRegex, RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.IgnoreCase).Matches(sInput);
            foreach (Match match in matchs)
            {
                if (iGroupIndex > 0)
                {
                    sInput = sInput.Replace(match.Groups[iGroupIndex].Value, sReplace);
                }
                else
                {
                    sInput = sInput.Replace(match.Value, sReplace);
                }
            }
            return sInput;
        }

        public static string Replace(string sInput, string sRegex, string sReplace, string sGroupName)
        {
            MatchCollection matchs = new Regex(sRegex, RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.IgnoreCase).Matches(sInput);
            foreach (Match match in matchs)
            {
                if (sGroupName != "")
                {
                    sInput = sInput.Replace(match.Groups[sGroupName].Value, sReplace);
                }
                else
                {
                    sInput = sInput.Replace(match.Value, sReplace);
                }
            }
            return sInput;
        }

        public static List<string> Split(string sInput, string sRegex, int iStrLen)
        {
            string[] strArray = new Regex(sRegex, RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.IgnoreCase).Split(sInput);
            List<string> list = new List<string>();
            list.Clear();
            foreach (string str in strArray)
            {
                if (str.Trim().Length >= iStrLen)
                {
                    list.Add(str.Trim());
                }
            }
            return list;
        }
    }
}
