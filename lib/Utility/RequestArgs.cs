using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.ccfw.Utility
{
    public class RequestArgs
    {
        private string _Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/msword, */*";
        private bool _blGetHeaders = false;
        private bool _blPolicy = false;
        private bool _blRedirect = false;
        private string _charset = "";
        private string _ContentType = "";
        private string _cookie = "";
        private string _Encode = "UTF-8";
        private string _IpAddress = "";
        private List<CookieObj> _listCookie = new List<CookieObj>();
        private string _Method = "GET";
        private string _postData = "";
        private string _RefererUrl = "";
        private int _TimeOut = 0x1388;
        private string _Url = "";

        public string Accept
        {
            get
            {
                return this._Accept;
            }
            set
            {
                this._Accept = value;
            }
        }

        public bool blGetHeaders
        {
            get
            {
                return this._blGetHeaders;
            }
            set
            {
                this._blGetHeaders = value;
            }
        }

        public bool blPolicy
        {
            get
            {
                return this._blPolicy;
            }
            set
            {
                this._blPolicy = value;
            }
        }

        public bool blRedirect
        {
            get
            {
                return this._blRedirect;
            }
            set
            {
                this._blRedirect = value;
            }
        }

        public string charset
        {
            get
            {
                return this._charset;
            }
            set
            {
                this._charset = value;
            }
        }

        public string ContentType
        {
            get
            {
                return this._ContentType;
            }
            set
            {
                this._ContentType = value;
            }
        }

        public string cookie
        {
            get
            {
                if (string.IsNullOrEmpty(this._cookie))
                {
                    foreach (CookieObj obj2 in this.listCookie)
                    {
                        this._cookie = this._cookie + string.Format("{0}={1};", obj2.cookieName, obj2.cookieValue);
                    }
                }
                return this._cookie;
            }
            set
            {
                this._cookie = value;
            }
        }

        public string Encode
        {
            get
            {
                return this._Encode;
            }
            set
            {
                this._Encode = value;
            }
        }

        public string IpAddress
        {
            get
            {
                return this._IpAddress;
            }
            set
            {
                this._IpAddress = value;
            }
        }

        public List<CookieObj> listCookie
        {
            get
            {
                return this._listCookie;
            }
            set
            {
                this._cookie = "";
                this._listCookie = value;
            }
        }

        public string Method
        {
            get
            {
                return this._Method;
            }
            set
            {
                this._Method = value;
            }
        }

        public string postData
        {
            get
            {
                return this._postData;
            }
            set
            {
                this._postData = value;
            }
        }

        public string RefererUrl
        {
            get
            {
                return this._RefererUrl;
            }
            set
            {
                this._RefererUrl = value;
            }
        }

        public int TimeOut
        {
            get
            {
                return this._TimeOut;
            }
            set
            {
                this._TimeOut = value;
            }
        }

        public string Url
        {
            get
            {
                return this._Url;
            }
            set
            {
                this._Url = value;
            }
        }
    }
}
