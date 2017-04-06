
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.weispa.Web.Models;

namespace com.weispa.Web.ViewModels
{
    public class MyAppointVm:BaseVm
    {
        public int Type;
        public List<MyOrderVm> MyOrders { get; set; }
    }

    public class MyOrderVm
    {
        public Order Order { get; set; }
        public List<int> ToStatusList { get; set; } 
    }
}