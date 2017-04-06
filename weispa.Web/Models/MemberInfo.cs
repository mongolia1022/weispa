using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.ccfw.Model.Base;

namespace com.weispa.Web.Models
{
    public class MemberInfo:BaseModel
    {
        public MemberInfo()
        {
            PrimaryKey = "Uid";
            IsAutoId = true;
            ConnName = DbEnum.weispa.ToString();
        }

        public int Uid{get; set; }

        public string OpenId { get; set; }

        public string NickName { get; set; }

        public int Role { get; set; }

        public int CreateOn { get; set; }
    }
}