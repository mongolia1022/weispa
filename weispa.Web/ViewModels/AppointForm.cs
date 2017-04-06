using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.weispa.Web.Models;

namespace com.weispa.Web.ViewModels
{
    public class AppointFormVm : BaseVm
    {
        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public int Room { get; set; }
    }

    public class AppointRq
    {
        public DateTime Start { get; set; }

        public int Room { get; set; }

        public string CustomName { get; set; }

        public string CustomPhone { get; set; }

        public string Remark { get; set; }
    }
}