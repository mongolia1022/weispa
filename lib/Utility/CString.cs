using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace com.ccfw.Utility
{
    public class CString
    {
        public static string AcquireAssignString(string str, int num)
        {
            string str2 = str;
            return StringToHtml(GetLetter(str2, num, false));
        }

        public static string AddBlankAtForefront(string str)
        {
            return str;
        }

        public static void AddWhere(StringBuilder sbWhere, string Condition)
        {
            if (!string.IsNullOrEmpty(Condition))
            {
                if (sbWhere.Length == 0)
                {
                    sbWhere.Append(Condition);
                }
                else
                {
                    sbWhere.Append(" and " + Condition);
                }
            }
        }

        public static bool CheckValidity(string s)
        {
            if (s == null)
            {
                return false;
            }
            if (s == "")
            {
                return false;
            }
            return Regex.Match(s, @"^\w+$", RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.IgnoreCase).Success;
        }

        public static string ClearSpecial(string sInput)
        {
            sInput = sInput.Replace("\"", "'");
            return sInput;
        }

        public static string ClearTag(string sHtml)
        {
            if (sHtml == "")
            {
                return "";
            }
            sHtml = CRegex.Replace(sHtml, CRegex.sIFrameReg, "", 0);
            sHtml = CRegex.Replace(sHtml, CRegex.sScriptReg, "", 0);
            sHtml = Regex.Replace(sHtml, "<[^>]*>|&nbsp;", string.Empty, RegexOptions.IgnoreCase);
            sHtml = Regex.Replace(sHtml, @"\s\s", " ", RegexOptions.IgnoreCase);
            sHtml = Regex.Replace(sHtml, @"\s\s", " ", RegexOptions.IgnoreCase);
            sHtml = Regex.Replace(sHtml, @"\s\s", " ", RegexOptions.IgnoreCase);
            sHtml = Regex.Replace(sHtml, "&ldquo;", "“", RegexOptions.IgnoreCase);
            sHtml = Regex.Replace(sHtml, "&rdquo;", "”", RegexOptions.IgnoreCase);
            return sHtml;
        }

        public static string ClearTag(string sHtml, bool bClear)
        {
            if (sHtml == "")
            {
                return "";
            }
            sHtml = CRegex.Replace(sHtml, CRegex.sIFrameReg, "", 0);
            sHtml = CRegex.Replace(sHtml, CRegex.sScriptReg, "", 0);
            sHtml = CRegex.Replace(sHtml, @"(<[^>\s]*\b(\w)+\b[^>]*>)|([\s]+)|(<>)|(&nbsp;)", "", 0);
            sHtml = sHtml.Replace("\"", "").Replace("<", "").Replace(">", "");
            return sHtml;
        }

        public static string ClearTag(string sHtml, string sRegex)
        {
            string str = sHtml;
            Regex regex = new Regex(sRegex, RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.IgnoreCase);
            return regex.Replace(sHtml, "");
        }

        public static DateTime ConvertToDate(string sDate)
        {
            DateTime now = DateTime.Now;
            if (sDate != null)
            {
                if (sDate == "")
                {
                    return now;
                }
                try
                {
                    now = DateTime.Parse(sDate);
                }
                catch
                {
                }
            }
            return now;
        }

        public static string ConvertToJS(string sHtml)
        {
            StringBuilder builder = new StringBuilder();
            string[] strArray = new Regex(@"\r\n", RegexOptions.IgnoreCase).Split(sHtml);
            foreach (string str in strArray)
            {
                builder.Append("document.writeln(\"" + str.Replace("\"", "\\\"") + "\");\r\n");
            }
            return builder.ToString();
        }

        public static int CovnertToInt(string strNum, int defInt)
        {
            int num = defInt;
            try
            {
                num = int.Parse(strNum);
            }
            catch
            {
            }
            return num;
        }

        public static string CutPath(string sPath, char cSeparator, int iCutNum)
        {
            string[] strArray = sPath.Split(new char[] { cSeparator });
            int length = strArray.Length;
            if (length > 0)
            {
                string str = strArray[length - iCutNum];
                for (int i = iCutNum - 1; i > 0; i--)
                {
                    str = str + cSeparator.ToString() + strArray[length - i];
                }
                return str;
            }
            return sPath;
        }

        public static string CutString(string str, int len)
        {
            byte[] bytes = Encoding.Default.GetBytes(str);
            if (bytes.Length > len)
            {
                return (Encoding.Default.GetString(bytes, 0, len) + "...");
            }
            return str;
        }

        public static string CutString(string str, int len, bool bAddDot)
        {
            return GetLetter(ClearTag(str), len, bAddDot);
        }

        public static string GetFileName(DateTime dt)
        {
            return string.Format("{0:x}", dt.Ticks).Substring(4);
        }

        public static string GetFirstSplit(string s)
        {
            return s.Split(new char[] { ',' })[0];
        }

        public static int GetLength(string str)
        {
            byte[] bytes = new ASCIIEncoding().GetBytes(str);
            int num = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] == 0x3f)
                {
                    num++;
                }
                num++;
            }
            return num;
        }

        public static string GetLetter(string str, int iNum, bool bAddDot)
        {
            string str2 = "";
            int length = iNum;
            if (str != null)
            {
                char[] chArray;
                str2 = str;
                if (((str2.Length <= 0) || (length <= 0)) || ((str2.Length * 2) <= length))
                {
                    return str2;
                }
                if (str2.Length >= length)
                {
                    chArray = str.ToCharArray(0, length);
                }
                else
                {
                    chArray = str.ToCharArray(0, str2.Length);
                }
                int num2 = 0;
                int num3 = 0;
                int num4 = 0;
                foreach (char ch in chArray)
                {
                    num4++;
                    if (char.GetUnicodeCategory(ch) == UnicodeCategory.OtherLetter)
                    {
                        num3 += 2;
                    }
                    else
                    {
                        num2 = ch;
                        if (num2 < 0)
                        {
                            num2 = 0x10000;
                        }
                        if (num2 > 0xff)
                        {
                            num3 += 2;
                        }
                        else
                        {
                            if ((num2 >= 0x41) && (num2 <= 90))
                            {
                                num3++;
                            }
                            num3++;
                        }
                    }
                    if (num3 >= length)
                    {
                        break;
                    }
                }
                if (bAddDot)
                {
                    str2 = str2.Substring(0, num4 - 2) + "...";
                }
                else
                {
                    str2 = str2.Substring(0, num4);
                }
            }
            return str2;
        }

        public static Dictionary<string, string> GetNameValueFromString(string sourceString)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            string[] strArray = sourceString.Split(new char[] { ';' });
            foreach (string str in strArray)
            {
                string[] strArray2 = str.Split(new char[] { '=' });
                dictionary.Add(strArray2[0], strArray2[1]);
            }
            return dictionary;
        }

        public static string GetPath(DateTime dt)
        {
            return (dt.Year.ToString() + "/" + dt.Month.ToString("00") + "/" + dt.Day.ToString("00") + "/");
        }

        public static string GetPreStrByLast(string sOrg, string sLast)
        {
            int length = sOrg.LastIndexOf(sLast);
            if (length > 0)
            {
                return sOrg.Substring(0, length);
            }
            return sOrg;
        }

        public static Dictionary<string, string> GetProperties(string sHtml)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            MatchCollection matchs = new Regex("\\s+(?<Key>[^\\s]+?)\\s*=\\s*\"(?<Value>[\\s\\S]*?)(?<![\\\\])\"").Matches(sHtml);
            string key = null;
            foreach (Match match in matchs)
            {
                key = match.Groups["Key"].Value.ToLower();
                if (!dictionary.ContainsKey(key))
                {
                    dictionary.Add(match.Groups["Key"].Value.ToLower(), match.Groups["Value"].Value.Replace("\\\"", "\""));
                }
            }
            return dictionary;
        }

        //public static string GetSQLFildList(string fldList)
        //{
        //    if (fldList == null)
        //    {
        //        return "*";
        //    }
        //    if (fldList.Trim() == "")
        //    {
        //        return "*";
        //    }
        //    if (fldList.Trim() == "*")
        //    {
        //        return "*";
        //    }
        //    string str = fldList;
        //    str = str.Trim().Replace("[", "").Replace("]", "");
        //    return ("[" + str + "]").Replace(0xff0c, ',').Replace(",", "],[");
        //}

        public static string GetStrByLast(string sOrg, string sLast)
        {
            int num = sOrg.LastIndexOf(sLast);
            if (num > 0)
            {
                return sOrg.Substring(num + 1);
            }
            return sOrg;
        }

        public static bool IsEmpty(string str)
        {
            return ((str == null) || (str == ""));
        }

        public static bool IsVariableName(string s)
        {
            Regex regex = new Regex("^[a-z][a-z0-9]*$", RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.IgnoreCase);
            return regex.IsMatch(s);
        }

        public static string JsToHtml(string strJS)
        {
            return CRegex.Replace(strJS.Replace("document.writeln(\"", "").Replace("document.write(\"", "").Replace("document.write('", ""), @"(?<backslash>\\)[^\\]", "", "backslash").Replace(@"\\", @"\").Replace(@"/\\\", @"\").Replace(@"/\\\'", @"\'").Replace(@"/\\\//", @"\/").Replace("\");", "").Replace("\")", "").Replace("');", "");
        }

        public static string PassURLParameters(NameValueCollection cQueryString, string[] sKeys)
        {
            if (sKeys == null)
            {
                return "";
            }
            string str = "";
            for (int i = 0; i < sKeys.Length; i++)
            {
                string str2 = cQueryString[sKeys[i]];
                if (str2 != null)
                {
                    str2 = "&" + sKeys[i] + "=" + str2;
                    str = str + str2;
                }
            }
            return str;
        }

        public static string ReassembleSubString(string sCode, char cSeparator, int iSubCount)
        {
            return ReassembleSubString(sCode, cSeparator, iSubCount, false);
        }

        public static string ReassembleSubString(string sCode, char cSeparator, int iSubCount, bool IsFromEnd)
        {
            string str;
            int num2;
            string[] strArray = sCode.Split(new char[] { cSeparator });
            if (iSubCount > strArray.Length)
            {
                return sCode;
            }
            if (IsFromEnd)
            {
                int length = strArray.Length;
                str = strArray[length - 1];
                for (num2 = 1; num2 < iSubCount; num2++)
                {
                    str = strArray[(length - 1) - num2] + cSeparator.ToString() + str;
                }
                return str;
            }
            str = strArray[0];
            for (num2 = 1; num2 < iSubCount; num2++)
            {
                str = str + cSeparator.ToString() + strArray[num2];
            }
            return str;
        }

        public static string ReplaceIfTag(string sContent, string sPath)
        {
            MatchCollection matchs = new Regex("<!--#if expr=\"\\$HTTP_HOST = '(.*?)'\" -->(.*?)<!--#else -->(.*?)<!--#endif -->", RegexOptions.Singleline).Matches(sContent);
            if (matchs.Count != 0)
            {
                string str = string.Empty;
                string str2 = string.Empty;
                string newValue = string.Empty;
                for (int i = 0; i < matchs.Count; i++)
                {
                    newValue = string.Empty;
                    str = matchs[i].Groups[1].Value;
                    str2 = (HttpContext.Current.Request.Url.Host == str) ? matchs[i].Groups[2].Value : matchs[i].Groups[3].Value;
                    newValue = ReplaceIncTag(str2, sPath);
                    sContent = sContent.Replace(matchs[i].Value, newValue);
                }
            }
            return sContent;
        }

        public static string ReplaceInclude(string sContent, string sPath)
        {
            sContent = ReplaceIfTag(sContent, sPath);
            sContent = ReplaceIncTag(sContent, sPath);
            return sContent;
        }

        public static string ReplaceIncTag(string sContent, string sPath)
        {
            MatchCollection matchs = new Regex("<!--#include virtual=\"(.*?)\".*?-->", RegexOptions.Singleline).Matches(sContent);
            if (matchs.Count != 0)
            {
                string path = string.Empty;
                string newValue = string.Empty;
                for (int i = 0; i < matchs.Count; i++)
                {
                    newValue = string.Empty;
                    path = matchs[i].Groups[1].Value;
                    if (path.StartsWith("/"))
                    {
                        path = HttpContext.Current.Server.MapPath(path);
                    }
                    else if (path.StartsWith("../"))
                    {
                        path = sPath.TrimEnd(new char[] { '\\' }) + @"\..\" + path.Replace("/", @"\");
                    }
                    else
                    {
                        path = sPath.Substring(0, sPath.LastIndexOf(@"\") + 1) + path;
                    }
                    if (File.Exists(path))
                    {
                        newValue = ReplaceIncTag(CFile.Read(path), path);
                    }
                    sContent = sContent.Replace(matchs[i].Value, newValue);
                }
            }
            return sContent;
        }

        public static string ReplaceNbsp(string str)
        {
            string str2 = str;
            if (str2.Length > 0)
            {
                str2 = str2.Replace(" ", "").Replace("&nbsp;", "");
                str2 = "&nbsp;&nbsp;&nbsp;&nbsp;" + str2;
            }
            return str2;
        }

        public static string SpecialCharTrim(string sInput, char c)
        {
            char[] arrC = new char[] { c };
            return SpecialCharTrim(sInput, arrC);
        }

        public static string SpecialCharTrim(string sInput, char[] arrC)
        {
            for (int i = 0; i < arrC.Length; i++)
            {
                while (sInput.StartsWith(arrC[i].ToString()))
                {
                    sInput = sInput.Substring(1);
                }
                while (sInput.EndsWith(arrC[i].ToString()))
                {
                    sInput = sInput.Remove(sInput.Length - 1, 1);
                }
            }
            return sInput;
        }

        public static string StringToHtml(string str)
        {
            string str2 = str;
            if (str2.Length > 0)
            {
                string oldValue = "\r\n";
                str2 = str2.Replace(oldValue, "<br />");
            }
            return str2;
        }

        public static string SwitchStringSeparator(string sInputString, char OriginalSeparator, char NewSeparator)
        {
            string[] strArray = sInputString.Split(new char[] { OriginalSeparator });
            if (strArray.Length > 1)
            {
                string str = strArray[0];
                for (int i = 1; i < strArray.Length; i++)
                {
                    str = str + ((strArray[i] == "") ? "" : (NewSeparator.ToString() + strArray[i]));
                }
                return str;
            }
            return sInputString;
        }

        public static string TranslateToHtmlString(string str, int num)
        {
            string str2 = str;
            return StringToHtml(GetLetter(str2, num, false));
        }
    }
}
