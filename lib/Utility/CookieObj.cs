using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.ccfw.Utility
{
    public class CookieObj
    {
        private string _cookieName = "";
        private string _cookieValue = "";

        public string cookieName
        {
            get
            {
                return this._cookieName;
            }
            set
            {
                this._cookieName = value;
            }
        }

        public string cookieValue
        {
            get
            {
                return this._cookieValue;
            }
            set
            {
                this._cookieValue = value;
            }
        }
    }
}
