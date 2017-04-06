using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using com.ccfw.Utility;
using com.weispa.Web.Dal;
using com.weispa.Web.Models;

namespace com.weispa.Web.Controllers
{
    public class BaseController:Controller
    {
        private MemberInfo _memberInfo;

        public MemberInfo MemberInfo
        {
            get
            {
                if (_memberInfo != null)
                    return _memberInfo;

                var uid = ConvertHelper.StrToInt(CookieHelper.GetCookie("weispaUser"));
                if(uid>0)
                    _memberInfo = MemberDal.GetModel(uid);

                return _memberInfo ?? (_memberInfo=new MemberInfo());
            }
        }

        public bool IsLogin
        {
            get { return MemberInfo.Uid > 0; }
        }


        private MemberInfoDal _memberdal;
        protected MemberInfoDal MemberDal
        {
            get { return _memberdal ?? (_memberdal=new MemberInfoDal()); }
        }

        protected ActionResult ReturnJson(bool success, object msg)
        {
            return Json(new { success, msg });
        }
    }
}