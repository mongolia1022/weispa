using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using com.ccfw.Utility;
using com.weispa.Web.Models;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace com.weispa.Web.Util
{
    public class WxHelper
    {
        #region 常量变量
        const string WechatUrl = "https://api.weixin.qq.com";
        protected const string Token = "weispa2016";
        private string cacheKey_AccessToken;
        private string CACHE_TICKET_KEY;
        /// <summary>
        /// 普通文本消息
        /// </summary>
        public const string MsgTextXml = @"<xml>
                            <ToUserName><![CDATA[{0}]]></ToUserName>
                            <FromUserName><![CDATA[{1}]]></FromUserName>
                            <CreateTime>{2}</CreateTime>
                            <MsgType><![CDATA[text]]></MsgType>
                            <Content><![CDATA[{3}]]></Content>
                            </xml>";

        #endregion

        #region 初始化
        public WxHelper()
        {
            m_WeChatAppKey = WeispaConfig.WxAppKey;
            m_WeChatAppSecret = WeispaConfig.WxAppSecret;

            cacheKey_AccessToken = string.Format("WeiXinAccessToken_{0}", m_WeChatAppKey);
            CACHE_TICKET_KEY = string.Format("WeiXinCACHE_TICKET_KEY_{0}", m_WeChatAppKey);
        }
        #endregion

        #region 属性
        private int m_WeChatAccountType;
        /// <summary>
        /// WeChatAccountType ID
        /// </summary>
        public int WeChatAccountType { get { return m_WeChatAccountType; } }

        private string m_WeChatAppKey;
        /// <summary>
        /// AppKey
        /// </summary>
        public string WeChatAppKey { get { return m_WeChatAppKey; } }

        private string m_WeChatAppSecret;
        /// <summary>
        /// AppSecret
        /// </summary>
        public string WeChatAppSecret { get { return m_WeChatAppSecret; } }
        #endregion

        #region 判断微信来源
        /// <summary>
        /// 根据userAgent判断是否微信来源
        /// </summary>
        public static bool IsFromWx(HttpContextBase httpcontext)
        {
            var userAgent = httpcontext.Request.UserAgent ?? string.Empty;

            return !string.IsNullOrWhiteSpace(userAgent)
                   && userAgent.IndexOf("MicroMessenger", StringComparison.InvariantCultureIgnoreCase) != -1;
        }
        #endregion

        #region 将微信post过来的xml字符串转换为实体
        /// <summary>
        /// 将微信post过来的信息转换为实体
        /// </summary>
        /// <param name="rootElement"></param>
        /// <returns></returns>
        public static RequestXML GetRequestXML(Stream s)
        {
            RequestXML result = null;
            try
            {
                byte[] b = new byte[s.Length];
                s.Read(b, 0, (int)s.Length);
                var postStr = Encoding.UTF8.GetString(b);
                if (!string.IsNullOrEmpty(postStr))
                {
                    var doc = new XmlDocument();
                    doc.LoadXml(postStr);
                    XmlElement rootElement = doc.DocumentElement;
                    result = GetRequestXML(rootElement);
                }
                return result;
            }
            catch
            {
                return result;
            }
        }

        /// <summary>
        /// 将微信post过来的xml字符串转换为实体
        /// </summary>
        /// <param name="rootElement"></param>
        /// <returns></returns>
        private static RequestXML GetRequestXML(XmlElement rootElement)
        {
            XmlNode MsgType = rootElement.SelectSingleNode("MsgType");//消息类型

            RequestXML requestXML = new RequestXML();
            requestXML.ToUserName = rootElement.SelectSingleNode("ToUserName").InnerText;
            requestXML.FromUserName = rootElement.SelectSingleNode("FromUserName").InnerText;
            requestXML.CreateTime = rootElement.SelectSingleNode("CreateTime").InnerText;
            requestXML.MsgType = MsgType.InnerText;

            if (requestXML.MsgType == "text")
            {
                requestXML.Content = rootElement.SelectSingleNode("Content").InnerText;

                var tempInt = 0L;
                long.TryParse(rootElement.SelectSingleNode("MsgId").InnerText, out tempInt);
                requestXML.MsgId = tempInt;
            }
            else if (requestXML.MsgType == "location")
            {
                requestXML.Location_X = rootElement.SelectSingleNode("Location_X").InnerText;
                requestXML.Location_Y = rootElement.SelectSingleNode("Location_Y").InnerText;
                requestXML.Scale = rootElement.SelectSingleNode("Scale").InnerText;
                requestXML.Label = rootElement.SelectSingleNode("Label").InnerText;
            }
            else if (requestXML.MsgType == "event")
            {
                requestXML.EventType = rootElement.SelectSingleNode("Event").InnerText;
                if (requestXML.EventType == "CLICK")
                {
                    requestXML.Content = rootElement.SelectSingleNode("EventKey").InnerText;
                    requestXML.EventKey = rootElement.SelectSingleNode("EventKey").InnerText;
                }
                else if (requestXML.EventType.ToLower() == "subscribe")
                {
                    requestXML.EventKey = rootElement.SelectSingleNode("EventKey").InnerText;
                }
                else if (requestXML.EventType.ToLower() == "scan")
                {
                    requestXML.EventKey = rootElement.SelectSingleNode("EventKey").InnerText;
                }
            }
            else if (requestXML.MsgType == "image")
            {
                requestXML.PicUrl = rootElement.SelectSingleNode("PicUrl").InnerText;
            }

            return requestXML;
        }
        #endregion

        #region 获取access_token
        /// <summary>
        /// 获取access_token
        /// </summary>
        /// <returns></returns>
        public string AccessToken
        {
            get
            {
                var accessToken =MemoryCache.Get<string>(cacheKey_AccessToken);
                if (string.IsNullOrEmpty(accessToken))
                {
                    var mArgs = new RequestArgs
                    {
                        Encode = "utf-8",
                        Method = "Get",
                        Url = AccessTokenUrl,
                        blGetHeaders = false
                    };
                    string returnData = CWebRequest.GetPost(mArgs);

                    var returnJson = JsonConvert.DeserializeObject<Hashtable>(returnData);
                    if (returnJson.Contains("access_token"))
                    {
                        accessToken = returnJson["access_token"].ToString();
                        MemoryCache.Set(cacheKey_AccessToken, accessToken, DateTime.Now.AddSeconds(5400));
                    }
                }

                return accessToken;
            }
        }
        #endregion

        #region 获取微信用户信息
        /// <summary>
        /// 获取用户微信基本信息
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public WechatUserInfo GetWechatUserInfo(string openId)
        {
            WechatUserInfo wechatUserInfo = new WechatUserInfo()
            {
                Uinionid = string.Empty,
                Nickname = string.Empty,
            };

            var userInfo = RequestGet(GetUserInfoUrl(openId));
            if (userInfo["errcode"] != null)//获取失败则刷新AccessToken缓存并再获取一次用户信息
            {
                MemoryCache.Remove(cacheKey_AccessToken);

                userInfo = RequestGet(GetUserInfoUrl(openId));

                if (userInfo["errcode"] == null)
                    return null;
            }

            if (userInfo["unionid"] != null)
            {
                wechatUserInfo.Uinionid = userInfo["unionid"].ToString();
            }
            if (userInfo["nickname"] != null)
            {
                wechatUserInfo.Nickname = userInfo["nickname"].ToString();
            }
            return wechatUserInfo;
        }
        #endregion

        #region 通过授权码获取openid
        /// <summary>
        /// 通过授权码获取openid
        /// </summary>
        /// <returns></returns>
        public string GetOpenIdFromAuthCode()
        {
            string code = HttpContext.Current.Request["code"];
            string openid = null;
            var url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", WeChatAppKey, WeChatAppSecret, code);

            var accessInfo = RequestGet(url);
            if (accessInfo["openid"] != null)
            {
                openid = accessInfo["openid"].ToString();
            }
            return openid;
        }

        public WechatUserInfo GetUserInfoFromAuthCode(string code)
        {
            var url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", WeChatAppKey, WeChatAppSecret, code);
            JObject accessInfo;
            try
            {
                accessInfo = RequestGet(url);
            }
            catch
            {
                return null;
            }

            if (accessInfo["openid"] == null) 
                return null;

            var wxuser = new WechatUserInfo { OpenId = accessInfo["openid"].ToString() };
            
            if (accessInfo["scope"].ToString() == "snsapi_base")//只获取基础信息到此为止
                return wxuser;

            //获取更多信息
            string access_token = accessInfo["access_token"].ToString();
            url = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN", access_token, wxuser.OpenId);
            try
            {
                return JsonConvert.DeserializeObject<WechatUserInfo>(RequestGet(url).ToString());
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region 重建菜单
        public JObject RepeatMenu(string menuText)
        {
            var returnData = DeleteMenu();

            if (returnData["errmsg"].ToString() == "ok")
                returnData = RequestPost(MenuCreateUrl, menuText);

            return returnData;
        }

        public JObject DeleteMenu()
        {
            return RequestGet(MenuDeleteUrl);
        }
        #endregion

        #region 推送文本消息给微信用户
        /// <summary>
        /// 推送文本消息给微信用户
        /// </summary>
        public JObject TextMsgPusher(string openId, string message)
        {
            var postData = new JObject(
                               new JProperty("touser", openId),
                               new JProperty("msgtype", "text"),
                               new JProperty("text", new JObject(
                                   new JProperty("content", message))
                                   )
                               );

            return RequestPost(CustomerServiceMsgUrl, JsonConvert.SerializeObject(postData));
        }
        #endregion

        #region 批量推送图文消息
        public void ArticleMsgPusher(List<string> openids, JArray articles)
        {
            foreach (var openid in openids)
            {
                var postData = new JObject(
                               new JProperty("touser", openid),
                               new JProperty("msgtype", "news"),
                               new JProperty("news", new JObject(
                                   new JProperty("articles", articles)
                                   )
                               )
                               );

                RequestPost(CustomerServiceMsgUrl, JsonConvert.SerializeObject(postData));
            }
        }
        #endregion

        #region other

        /// <summary>
        /// 请求帮助方法
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private JObject RequestGet(string url)
        {
            var mArgs = new RequestArgs
            {
                Encode = "utf-8",
                Method = "GET",
                Url = url
            };


            var returnData = CWebRequest.GetPost(mArgs);

            if (!string.IsNullOrEmpty(returnData))
                return JsonConvert.DeserializeObject<JObject>(returnData);

            return null;
        }

        private JObject RequestPost(string url, string data)
        {
            RequestArgs mArgsResponse = new RequestArgs
            {
                Encode = "utf-8",
                Method = "POST",
                Url = url,
                postData = data
            };
            var returnData = CWebRequest.GetPost(mArgsResponse);

            if (!string.IsNullOrEmpty(returnData))
                return JsonConvert.DeserializeObject<JObject>(returnData);

            return null;
        }

        #endregion

        #region 常用微信请求Url
        /// <summary>
        /// 获取AccessToken接口
        /// </summary>
        public string AccessTokenUrl
        {
            get
            {
                return string.Format("{0}/cgi-bin/token?grant_type=client_credential&appid={1}&secret={2}", WechatUrl, WeChatAppKey, WeChatAppSecret);
            }
        }

        /// <summary>
        /// 用户基本信息接口
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public string GetUserInfoUrl(string openId)
        {
            return string.Format("{0}/cgi-bin/user/info?access_token={1}&openid={2}&lang=zh_CN", WechatUrl, AccessToken, openId);
        }

        /// <summary>
        /// 删除自定义菜单的API
        /// </summary>
        public string MenuDeleteUrl
        {
            get
            {
                return string.Format("{0}/cgi-bin/menu/delete?access_token={1}", WechatUrl, AccessToken);
            }
        }

        /// <summary>
        /// 创建自定义菜单的API
        /// </summary>
        public string MenuCreateUrl
        {
            get
            {
                return string.Format("{0}/cgi-bin/menu/create?access_token={1}", WechatUrl, AccessToken);
            }
        }

        /// <summary>
        /// Oauth2Authorize跳转
        /// </summary>
        /// <param name="redirectUrl"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public string Oauth2AuthorizeUrl(string redirectUrl, int scope = 0, string state = "STATE")
        {
            return string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope={3}&state={2}#wechat_redirect", WeChatAppKey, HttpUtility.UrlEncode(redirectUrl), state, scope == 0 ? "snsapi_base" : "snsapi_userinfo");
        }

        /// <summary>
        /// 客服消息推送接口
        /// </summary>
        public string CustomerServiceMsgUrl
        {
            get
            {
                return string.Format("{0}/cgi-bin/message/custom/send?access_token={1}", WechatUrl, AccessToken);
            }
        }
        public string Url_Format_Ticket(string accsecc_token)
        {
            return string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi", accsecc_token);
        }
        #endregion

        #region jssdk
        /// <summary>
        /// 获取jssdk签名配置对象
        /// </summary>
        /// <param name="jsapi">JsApiEnum,如:JsApiEnum.scanQRCode|JsApiEnum.onMenuShareQQ</param>
        /// <returns>微信公众平台JsSdk的配置对象</returns>
        public SignPackage GetSignPackage(JsApiEnum jsapi, bool debug)
        {
            HttpContext httpcontext = System.Web.HttpContext.Current;
            string url = (!string.IsNullOrEmpty(httpcontext.Request.ServerVariables["HTTPS"])) && httpcontext.Request.ServerVariables["HTTPS"] != "off" ? "https://" : "http://";
            url += httpcontext.Request.ServerVariables["HTTP_HOST"];
            url += httpcontext.Request.ServerVariables["URL"];
            url += string.IsNullOrEmpty(httpcontext.Request.ServerVariables["QUERY_STRING"]) ? "" : "?" + httpcontext.Request.ServerVariables["QUERY_STRING"];
            return GetSignPackage(url, jsapi, debug);
        }

        /// <summary>
        /// 获取jssdk签名配置对象
        /// </summary>
        /// <param name="url">当前页面url</param>
        /// <param name="jsapi">JsApiEnum,如:JsApiEnum.scanQRCode|JsApiEnum.onMenuShareQQ</param>
        /// <returns>微信公众平台JsSdk的配置对象</returns>
        public SignPackage GetSignPackage(string url, JsApiEnum jsapi, bool debug)
        {

            /*
             * 签名字段
            noncestr=Wm3WZYTPz0wzccnW
            jsapi_ticket=sM4AOVdWfPE4DxkXGEs8VMCPGGVi4C3VM0P37wVUCFvkVAy_90u5h9nbSlYy3-Sl-HhTdfl2fzFy1AOcHKP7qg
            timestamp=1414587457
            url=http://mp.weixin.qq.com?params=value  
             */
            string noncestr = this.CreateNonceStr(16);
            string jsapi_tkcket = this.GetJsApiTicket();
            long timestamp = TimeStamp.Now();
            Dictionary<string, string> signData = new Dictionary<string, string>() { 
                {"noncestr",noncestr},
                {"jsapi_ticket",jsapi_tkcket},
                {"timestamp",timestamp.ToString()},
                {"url",url}
            };

            SignPackage result = new SignPackage()
            {
                appId = m_WeChatAppKey,
                timestamp = timestamp,
                nonceStr = noncestr,
                debug = debug,
                signature = new Signature().Sign(signData),
                jsApiList = jsapi.ToString().Replace(" ", "").Split(',')
            };
            return result;
        }

        /// <summary>
        /// 生成签名的随机字符串
        /// </summary>
        /// <param name="length">长度，小于32位，默认16位</param>
        /// <returns>随机字符串</returns>
        private string CreateNonceStr(int length = 16)
        {
            return Guid.NewGuid().ToString().Replace("-", "").Substring(0, length > 32 ? 32 : length);
        }

        /// <summary>
        /// 获取ApiTicket
        /// </summary>
        /// <returns>ApiTicket</returns>
        private string GetJsApiTicket()
        {
            var ticket = MemoryCache.Get<string>(CACHE_TICKET_KEY);
            if (ticket != null)
                return ticket.ToString();
            try
            {
                var result = RequestGet(Url_Format_Ticket(AccessToken));
                if (result["ticket"] != null)
                {
                    ticket = result["ticket"].ToString();
                    MemoryCache.Set(CACHE_TICKET_KEY, ticket, DateTime.Now.AddSeconds(7000));
                }
                else
                {
                    //为了程序正常运行，不抛出错误，可以记录日志
                    ticket = result["errmsg"].ToString();
                }
            }
            catch
            {
                //为了程序正常运行，不抛出错误，可以记录日志
                ticket = "there_is_an_error_when_getting_apiticket";
            }

            return ticket.ToString();
        }
        #endregion
    }
}