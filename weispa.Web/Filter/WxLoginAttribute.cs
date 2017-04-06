using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using com.ccfw.Utility;
using com.weispa.Web.Dal;
using com.weispa.Web.Models;
using com.weispa.Web.Util;

namespace com.weispa.Web.Filter
{
    public class WxLoginAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            ////非微信来源不执行微信授权步骤
            //if (!WxHelper.IsFromWx(context.HttpContext))
            //{
            //    context.Controller.ViewBag.isFromWx = false;
            //    context.Result = new ContentResult() { Content = "仅支持在微信端打开此页面" };
            //    return;
            //}

            //context.Controller.ViewBag.isFromWx = true;
            var request = context.HttpContext.Request;

            MemberInfo member=null;
            var uid = ConvertHelper.StrToInt(CookieHelper.GetCookie("weispaUser"));
            if (uid >0)
            {
                member = new MemberInfoDal().GetModel(uid);
            }

            //已登录直接通过
            if (member != null)
            {
                return;
            }

            //post提交时验证登录状态，仅支持get请求授权
            if (!request.HttpMethod.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                context.Result = new JsonResult() { Data = new { Success = false, Info = "您未登录成功,请先刷新页面" } };
                return;
            }

            //跳转授权绑定页面
            var urlHelper = ((Controller) context.Controller).Url;
            var url = request.Url;
            var webrootPath = url.Scheme + "://" + url.Authority;
            var bindUrl = webrootPath + urlHelper.Action("Login", "Account",
                new { redirect = HttpUtility.UrlEncode(request.Url.ToString())});

            context.Result = new RedirectResult(WxHelper.Oauth2AuthorizeUrl(bindUrl, 1, "clientAuthed"));
        }

        public void OnActionExecuted(ActionExecutedContext filterContext){}


        private WxHelper _wxHelper;

        public WxHelper WxHelper
        {
            get { return _wxHelper ?? (_wxHelper=new WxHelper()); }
        }
    }
}