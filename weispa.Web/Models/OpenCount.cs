﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.ccfw.Model.Base;

namespace com.weispa.Web.Models
{
    public class OpenCount : BaseModel
    {
        public OpenCount()
        {
            PrimaryKey = "Id";
            IsAutoId = true;
            ConnName = DbEnum.weispa.ToString();
        }

        public int Id { get; set; }

        public DateTime CreateOn { get; set; }

        public string Ip { get; set; }
    }
}