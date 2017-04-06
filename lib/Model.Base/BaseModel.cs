using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.ccfw.Model.Base
{
    [Serializable]
    public class BaseModel
    {
        /// <summary>
        /// 主键字段名
        /// </summary>
        public string PrimaryKey { get; set; }

        /// <summary>
        /// 主键是否自动增长
        /// </summary>
        public bool IsAutoId { get; set; }

        /// <summary>
        /// 数据库连接名称
        /// </summary>
        public string ConnName { get; set; }
    }
}
