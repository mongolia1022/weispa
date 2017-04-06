using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.weispa.Web.Models
{
    public enum DbEnum
    {
        weispa
    }

    /// <summary>
    /// 订单状态
    /// </summary>
    public enum OrderStatus
    {
        未确认=0,
        已确认=1,
        已完成=2,
        未完成=3,
        已取消=4,
        已锁定 = 5
    }

    public enum RoomType
    {
        玫瑰=0,
        薰衣草=1
    }

    public enum MemberRole
    {
         顾客=0,
         管理员=1
    }
}