using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.weispa.Web.Models;

namespace com.weispa.Web.ViewModels
{
    /// <summary>
    /// 预定护理页视图模型
    /// </summary>
    public class AppointVm:BaseVm
    {
        public List<DateTime> DateList { get; set; }

        public List<AppointItem> AppointList { get; set; }

        public DateTime SelectDate { get; set; }

        public class AppointItem
        {
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }

            public List<Room> Rooms { get; set; }
        }

        public class Room
        {
            public int Type { get; set; }
            public string Name { get; set; }

            /// <summary>
            /// 状态 0：可预约 1：已预约 2：不可预约
            /// </summary>
            public int Status { get; set; }

            public int OrderId { get; set; }

            /// <summary>
            /// 可操作的订单状态
            /// 0 可预约 1 可锁定 2 可解锁
            /// </summary>
            public List<int> ToStatusList { get; set; }
        }
    }

    public class AdminAppointVm:BaseVm
    {
        public AdminAppointRq Rq { get; set; }

        public List<MyOrderVm> Orders { get; set; }

        public int TotalCount { get; set; }

        public class AdminAppointRq
        {
            public DateTime? Start { get; set; }

            public DateTime? End { get; set; }

            public RoomType? RoomType { get; set; }

            public OrderStatus? Status { get; set; }

            public string Phone { get; set; }

            public string Name { get; set; }

            public int Pn { get; set; }
        }
    }
}

