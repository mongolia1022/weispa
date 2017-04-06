using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using com.ccfw.Utility;
using com.weispa.Web.Dal;
using com.weispa.Web.Filter;
using com.weispa.Web.Models;
using com.weispa.Web.Util;
using com.weispa.Web.ViewModels;

namespace com.weispa.Web.Controllers
{
    
    public class AccountController:BaseController
    {
        #region 绑定微信用户
        /// <summary>
        /// 绑定微信用户
        /// </summary>
        /// <param name="state"></param>
        /// <param name="code"></param>
        /// <param name="redirect"></param>
        /// <returns></returns>
        public ActionResult Login(string state, string code,string redirect)
        {
            if (state == "clientAuthed")
            {
                if (string.IsNullOrEmpty(code))
                {
                    return Content("由于您未授权,登录不成功");
                }

                var wxuser =new WxHelper().GetUserInfoFromAuthCode(code);
                if (wxuser == null)
                {
                    return Content("获取微信用户信息失败");
                }

                BindUser(wxuser);
                return Redirect(Server.UrlDecode(redirect));
            }
             return Content("请求来源不正确");
        }

        /// <summary>
        /// 绑定用户
        /// </summary>
        /// <param name="wxuser"></param>
        
        private MemberInfo BindUser(WechatUserInfo wxuser)
        {
            var memberInfoDal = new MemberInfoDal();
            var memberInfo = memberInfoDal.GetModel(string.Format("OpenId='{0}'", wxuser.OpenId));

            if (memberInfo == null)
            {
                memberInfo = new MemberInfo
                {
                    OpenId = wxuser.OpenId,
                    NickName = wxuser.Nickname,
                    Role = (int)MemberRole.顾客,
                    CreateOn = DateTime.Now.ToUnixStamp()
                };

                memberInfo.Uid=memberInfoDal.Add(memberInfo);
            }
            else
            {
                memberInfo.NickName = wxuser.Nickname;

                memberInfoDal.Update(memberInfo);
            }

            SetCookie(memberInfo);
            return memberInfo;
        }

        private void SetCookie(MemberInfo memberInfo)
        {
            CookieHelper.SetCookie("weispaUser", memberInfo.Uid.ToString(), 1440);
        }
        #endregion

        /// <summary>
        /// 创建邀请码页面
        /// </summary>
        /// <returns></returns>
        [WxLogin]
        public ActionResult CreateInvitationCode()
        {
            if (MemberInfo.Role != (int) MemberRole.管理员)
            {
                return Content("没有权限");
            }
            return View();
        }

        /// <summary>
        /// 创建管理员邀请码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [WxLogin]
        public ActionResult CreateInvitationCodePost(string code)
        {
            if (MemberInfo.Role != (int)MemberRole.管理员)
                return ReturnJson(false, "没有权限");

            if (string.IsNullOrEmpty(code))
                return ReturnJson(false, "请输入邀请码");
            

            MemoryCache.Set("InvitationCode_" + code, code, DateTime.Now.AddMinutes(WeispaConfig.InvitationCodeKeeTime));
           
            return ReturnJson(true,"成功");
        }

        /// <summary>
        /// 注册管理员页面
        /// </summary>
        /// <returns></returns>
        [WxLogin]
        public ActionResult JoinAdmin()
        {
            return View();
        }

        /// <summary>
        /// 注册管理员
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [WxLogin]
        public ActionResult JoinAdminPost(string code)
        {
            if (MemoryCache.Get<string>("InvitationCode_" + code) == null)
                return ReturnJson(false, "邀请码不正确");

            if (MemberInfo.Role == (int)MemberRole.管理员)
                return ReturnJson(false, "您已经是管理员");


            MemberInfo.Role = (int) MemberRole.管理员;
            var b = new MemberInfoDal().Update(MemberInfo,"Role");
            if (b)
            {
                MemoryCache.Remove("InvitationCode_" + code);
                return ReturnJson(true, "注册成功");
            }
            else
                return ReturnJson(false, "注册失败请重试");
        }

        /// <summary>
        /// 创建邀请码成功页面
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [WxLogin]
        public ActionResult InvitationCodeCreateSuccess(string code)
        {
            if (MemberInfo.Role != (int)MemberRole.管理员)
                return Content("没有权限");

            return View(new InvitationCodeCreateSuccessVm{Code = code});
        }

        /// <summary>
        /// 注册成功页面
        /// </summary>
        /// <returns></returns>
        [WxLogin]
        public ActionResult JoinSuccess()
        {
            return View();
        }
    }
}