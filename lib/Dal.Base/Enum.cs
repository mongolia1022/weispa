using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.ccfw.Dal.Base
{
    /// <summary>
    /// 指示是批量添加还更新
    /// </summary>
    public enum AddUpdateType
    {
        Add = 1, // 强制添加
        Update = 2, // 强制更新
        AddOrUpdate = 3 // 不存在则添加, 存在则更新
    }
}
