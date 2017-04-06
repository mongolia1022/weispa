using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.weispa.Web.Models;

namespace com.weispa.Web.ViewModels
{
    public class BaseVm
    {
        public string BodyStyle { get; set; }

        public MemberInfo MemberInfo { get; set; }
    }
}