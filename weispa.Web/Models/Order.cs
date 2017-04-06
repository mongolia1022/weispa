using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.ccfw.Model.Base;

namespace com.weispa.Web.Models
{
    public class Order : BaseModel
    {
        public Order()
        {
            PrimaryKey = "OId";
            IsAutoId = true;
            ConnName = DbEnum.weispa.ToString();
        }

        public int OId { get; set; }

        public int MemberId { get; set; }

        public string CustomName { get; set; }

        public string CustomPhone { get; set; }

        /// <summary>
        /// 房间类型 RoomType
        /// </summary>
        public int Room { get; set; }

        public int Status { get; set; }

        public int CreateOn { get; set; }

        public int StartTime { get; set; }

        public int EndTime { get; set; }

        public string Remark { get; set; }
    }
}