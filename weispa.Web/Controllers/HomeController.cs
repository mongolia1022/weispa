using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using com.ccfw.Dal.Base;
using com.weispa.Web.Models;
using com.weispa.Web.Util;

namespace com.weispa.Web.Controllers
{
    public class HomeController : BaseController
    {
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
    }
}
