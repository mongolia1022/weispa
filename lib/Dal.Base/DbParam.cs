using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.ccfw.Dal.Base
{
    public class DbParam
    {
        public DbParam()
        {
            ParamDbType = System.Data.DbType.String;
        }
        public string ParamName { get; set; }

        public System.Data.DbType ParamDbType{ get; set; }

        public object ParamValue{ get; set; }
    }
}
