using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using com.ccfw.Dal.Base;
using com.weispa.Web.Models;
using com.weispa.Web.Util;
using com.weispa.Web.Utils;

namespace com.weispa.Web.Controllers
{
    public class HomeController : BaseController
    {
        //private static string customerBaseUrl = "http://lizhanpeng3.oicp.net:8855/jd_web_test/api/ERP_Connecter";
        private static string customerBaseUrl = "http://198.198.195.93/jd_web_test/api/ERP_Connecter";

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Jwplayer()
        {
            WxHelper wxhelper=new WxHelper();
            SignPackage config = wxhelper.GetSignPackage(JsApiEnum.onMenuShareAppMessage | JsApiEnum.onMenuShareTimeline, false);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ViewBag.config = serializer.Serialize(config);
            return View();
        }

        //im
        public string getServer(string phone)
        {
            return WeispaUtil.HttpGet(customerBaseUrl + "/GetServicerList?clientPhone=" + phone);
        }

        //im
        public String getCustomer(string phone)
        {
            return WeispaUtil.HttpGet(customerBaseUrl +"/GetClientList?servicerPhone=" + phone);
        }

        //h5打开人数
        public string openCount()
        {
            OpenCount openCount=new OpenCount()
            {
                CreateOn = DateTime.Now,
                Ip = Request.ServerVariables.Get("Remote_Addr")
            };
            new BaseDAL<OpenCount>().Add(openCount);

            return "{status:1}";
        }

        //相框使用人数
        public string useCount(string frameName)
        {
            UseCount openCount = new UseCount()
            {
                CreateOn = DateTime.Now,
                Ip = Request.ServerVariables.Get("Remote_Addr"),
                frameName = frameName
            };
            new BaseDAL<UseCount>().Add(openCount);

            return "{status:1}";
        }
    }
}
