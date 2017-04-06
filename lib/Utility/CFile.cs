using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;

namespace com.ccfw.Utility
{
    public class CFile
    {
        public static void Create(string sPath)
        {
            FileStream stream = null;
            try
            {
                string directoryName = Path.GetDirectoryName(sPath);
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }
                stream = File.Create(sPath);
                stream.Close();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        public static void Delete(string sPath)
        {
            try
            {
                FileInfo info = new FileInfo(sPath);
                if (info.Exists)
                {
                    info.Attributes = FileAttributes.Normal;
                    info.Delete();
                }
            }
            catch
            {
            }
        }

        public static string Read(string sFile)
        {
            return Read(sFile, "gb2312");
        }

        public static string Read(string sFile, string sCoding)
        {
            Encoding encoding = Encoding.GetEncoding(sCoding);
            StreamReader reader = null;
            string str = "";
            try
            {
                reader = new StreamReader(sFile, encoding);
                str = reader.ReadToEnd();
            }
            catch
            {
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return str;
        }

        public static void SetLastModify(string sFile)
        {
            SetLastModify(sFile, DateTime.Parse("1990-1-1"));
        }

        public static void SetLastModify(string sFile, DateTime lastModify)
        {
            FileInfo info = new FileInfo(sFile);
            if (info.Exists)
            {
                info.Attributes = FileAttributes.Normal;
                info.LastWriteTime = lastModify;
            }
        }

        //public static string UploadFile(HtmlInputFile file, string basePath)
        //{
        //    if (file.PostedFile.ContentLength > 0x100000)
        //    {
        //        jsHelper.Alert("上传文件不得大于1M");
        //        return "";
        //    }
        //    if (!file.PostedFile.FileName.ToLower().EndsWith(".jpg"))
        //    {
        //        jsHelper.Alert("只能上传jpg图片");
        //        return "";
        //    }
        //    string str = string.Format("{0}/{1}/{2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //    string str2 = string.Format("{0}.jpg", BitConverter.ToUInt32(Guid.NewGuid().ToByteArray(), 0).ToString());
        //    CDirectory.Create(Path.Combine(basePath, str));
        //    file.PostedFile.SaveAs(Path.Combine(Path.Combine(basePath, str), str2));
        //    return string.Format("http://bbsimg.39.net/{0}/{1}", str, str2);
        //}

        //public static string UploadImage(HtmlInputFile file, string basePath)
        //{
        //    if (file.PostedFile.ContentLength > 0x100000)
        //    {
        //        jsHelper.Alert("上传文件不得大于1M");
        //        return "";
        //    }
        //    if (!file.PostedFile.FileName.ToLower().EndsWith(".jpg"))
        //    {
        //        jsHelper.Alert("只能上传jpg图片");
        //        return "";
        //    }
        //    string str = string.Format("{0}/{1}/{2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //    str = string.Format("{0}/{1}", basePath.TrimEnd(new char[] { '/' }), str);
        //    string str2 = string.Format("{0}.jpg", BitConverter.ToUInt32(Guid.NewGuid().ToByteArray(), 0).ToString());
        //    string path = HttpContext.Current.Server.MapPath(str);
        //    CDirectory.Create(path);
        //    file.PostedFile.SaveAs(Path.Combine(path, str2));
        //    return string.Format("{0}/{1}", str, str2);
        //}

        public static void Write(FileEntity file)
        {
            CDirectory.Create(file.FilePath);
            Write(file.FilePath + @"\" + file.FileName, file.FileContent);
        }

        public static void Write(List<FileEntity> listFile)
        {
            foreach (FileEntity entity in listFile)
            {
                Write(entity);
            }
        }

        public static void Write(string sFullFilePath, string sContent)
        {
            Write(sFullFilePath, sContent, "gb2312");
        }

        public static void Write(string sFullFilePath, byte[] btData)
        {
            FileStream stream = null;
            try
            {
                FileInfo info = new FileInfo(sFullFilePath);
                if (info.Exists)
                {
                    info.Attributes = FileAttributes.Normal;
                    info.Delete();
                }
                stream = new FileStream(sFullFilePath, FileMode.Create, FileAccess.Write);
                stream.Write(btData, 0, btData.Length);
                stream.Flush();
                stream.Close();
            }
            catch (Exception exception)
            {
                throw new Exception(sFullFilePath + exception.Message);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        public static void Write(string sFullFilePath, string sContent, string sCoding)
        {
            StreamWriter writer = null;
            Encoding encoding = Encoding.GetEncoding(sCoding);
            try
            {
                FileInfo info = new FileInfo(sFullFilePath);
                if (info.Exists)
                {
                    info.Attributes = FileAttributes.Normal;
                    Create(sFullFilePath);
                    writer = new StreamWriter(sFullFilePath, false, encoding);
                    writer.Write(sContent);
                    writer.Flush();
                    writer.Close();
                }
                else
                {
                    writer = new StreamWriter(sFullFilePath, false, encoding);
                    writer.Write(sContent);
                    writer.Flush();
                    writer.Close();
                }
            }
            catch (Exception exception)
            {
                throw new Exception(sFullFilePath + exception.Message);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }

        public static void WriteAppendLine(string sFullFilePath, string sContent)
        {
            StreamWriter writer = null;
            try
            {
                FileInfo info = new FileInfo(sFullFilePath);
                if (!info.Exists)
                {
                    Create(sFullFilePath);
                }
                writer = info.AppendText();
                writer.WriteLine(sContent);
                writer.Flush();
                writer.Close();
            }
            catch (Exception exception)
            {
                throw new Exception(sFullFilePath + exception.Message);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }
    }
}
