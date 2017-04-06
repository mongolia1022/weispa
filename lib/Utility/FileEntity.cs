using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.ccfw.Utility
{
    public class FileEntity
    {
        private string _FileContent = "";
        private string _FileName = "";
        private string _FilePath = "";

        public string FileContent
        {
            get
            {
                return this._FileContent;
            }
            set
            {
                this._FileContent = value;
            }
        }

        public string FileName
        {
            get
            {
                return this._FileName;
            }
            set
            {
                this._FileName = value;
            }
        }

        public string FilePath
        {
            get
            {
                return this._FilePath;
            }
            set
            {
                this._FilePath = value;
            }
        }
    }
}
