using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using com.ccfw.Model.Base;
using com.ccfw.Utility;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace com.ccfw.Dal.Base
{
    /// <summary>
    /// 数据访问基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseDAL<T> where T : BaseModel, new()
    {
        private string _PrimaryKey = "";
        private string _connName = "";
        private string _TableName = "";
        private T _t;

        #region 属性
        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseDAL() { }

        /// <summary>
        /// Model层变量Ｔ的实例
        /// </summary>
        private T t
        {
            get
            {
                if (_t == null)
                    _t = new T();
                return _t;
            }
        }

        /// <summary>
        /// 要连接的数据库名
        /// </summary>
        //public string connName
        //{
        //    get
        //    {
        //        return t.DataBaseName.ToString();
        //    }
        //}
        public string connName
        {
            get
            {
                return t.ConnName;
            }
            set { _connName = value; }
        }

        /// <summary>
        /// 主键字段名
        /// </summary>
        public string PrimaryKey
        {
            get
            {
                if (_PrimaryKey == "")
                    _PrimaryKey = t.PrimaryKey;
                return _PrimaryKey;
            }
            set { _PrimaryKey = value; }
        }

        /// <summary>
        /// 数据表名
        /// </summary>
        public virtual string TableName
        {
            get
            {
                if (_TableName == "")
                {
                    _TableName = "" + typeof(T).Name + "";
                }
                return _TableName;
            }
            set { _TableName = value; }
        }

        /// <summary>
        /// 最大ID值
        /// </summary>
        public int MaxID
        {
            get
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(string.Format("select max({0}) from {1}", PrimaryKey, TableName));
                Database db = DatabaseFactory.CreateDatabase(connName);
                using (DbCommand cmd = db.GetSqlStringCommand(strSql.ToString()))
                {
                    object obj = db.ExecuteScalar(cmd);
                    if (obj == null)
                    {
                        return 0;
                    }

                    #region Added BY  何斌

                    if (obj.ToString().Trim() == "")
                        return 0;

                    #endregion

                    return int.Parse(obj.ToString());
                }
            }
        }
        PropertyInfo[] _modelProperties = null;
        /// <summary>
        /// add by 李欣  2010-08-27
        /// 独立出所有属性，避免重复反射 提高性能
        /// </summary>
        public PropertyInfo[] ModelProperties
        {
            get
            {
                if (_modelProperties == null)
                {
                    _modelProperties =
                typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public);
                }
                return _modelProperties;
            }
        }




        #endregion 属性

        #region BaseMethod

        /// <summary>
        /// 判断记录是否存在
        /// </summary>
        /// <param name="id">记录ID</param>
        public virtual bool Exists(int id)
        {
            return Exists(string.Format("{0}=@{0}", PrimaryKey),
                          new List<DbParam> { new DbParam { ParamName = PrimaryKey, ParamDbType = DbType.Int32, ParamValue = id } });
        }

        public virtual bool Exists(long id)
        {
            return Exists(string.Format("{0}=@{0}", PrimaryKey),
                          new List<DbParam> { new DbParam { ParamName = PrimaryKey, ParamDbType = DbType.Int64, ParamValue = id } });
        }

        /// <summary>
        /// 判断记录是否存在
        /// </summary>
        /// <param name="strWhere">Where子句</param>
        public virtual bool Exists(string strWhere)
        {
            return Exists(strWhere, null);
        }

        public virtual bool Exists(string strWhere, List<DbParam> listPm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from " + TableName);
            if (strWhere != "")
                strSql.Append(" where " + strWhere);
            Database db = DatabaseFactory.CreateDatabase(connName);
            using (DbCommand cmd = db.GetSqlStringCommand(strSql.ToString()))
            {
                if (listPm != null)
                {
                    foreach (DbParam pm in listPm)
                    {
                        db.AddInParameter(cmd, pm.ParamName, pm.ParamDbType, pm.ParamValue);
                    }
                }
                object obj = db.ExecuteScalar(cmd);
                int cmdresult;
                if ((Equals(obj, null)) || (Equals(obj, DBNull.Value)))
                {
                    cmdresult = 0;
                }
                else
                {
                    cmdresult = int.Parse(obj.ToString());
                }
                if (cmdresult == 0)
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="id">记录ID</param>
        public virtual int Delete(int id)
        {
            return Delete(string.Format("{0}={1}", PrimaryKey, id));
        }

        public virtual int Delete(long id)
        {
            return Delete(string.Format("{0}={1}", PrimaryKey, id));
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="strWhere">Where子句</param>
        public virtual int Delete(string strWhere)
        {
            return Delete(strWhere, null);
        }

        public virtual int Delete(string strWhere, List<DbParam> listPm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete " + TableName);
            if (strWhere != "")
                strSql.Append(" where " + strWhere);
            Database db = DatabaseFactory.CreateDatabase(connName);
            using (DbCommand cmd = db.GetSqlStringCommand(strSql.ToString()))
            {
                if (listPm != null)
                {
                    foreach (DbParam pm in listPm)
                    {
                        db.AddInParameter(cmd, pm.ParamName, pm.ParamDbType, pm.ParamValue);
                    }
                }
                return db.ExecuteNonQuery(cmd);
            }
        }

        /// <summary>
        /// 获得记录数
        /// </summary>
        /// <param name="strWhere">Where子句</param>
        public virtual int GetCount(string strWhere)
        {
            return GetCount(strWhere, null);
        }

        public virtual int GetCount(string strWhere, List<DbParam> listPm)
        {
            string strSQL = "select count(0) from " + TableName;
            if (!string.IsNullOrEmpty(strWhere))
            {
                strSQL += " where " + strWhere;
            }
            Database db = DatabaseFactory.CreateDatabase(connName);
            using (DbCommand cmd = db.GetSqlStringCommand(strSQL))
            {
                if (listPm != null)
                {
                    foreach (DbParam pm in listPm)
                    {
                        db.AddInParameter(cmd, pm.ParamName, pm.ParamDbType, pm.ParamValue);
                    }
                }
                return int.Parse(db.ExecuteScalar(cmd).ToString());
            }
        }
        #endregion BaseMethod

        #region Add

        /// <summary>
        /// 添加记录，返回当前插入的这条记录的ID
        /// </summary>
        /// <param name="model">实体层某实体的实例</param>
        public virtual int Add(T model)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder strParameter = new StringBuilder();
            strSql.Append(string.Format("insert into {0}(", TableName));
            PropertyInfo[] pis = ModelProperties;
            //typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public);
            List<DbParam> listParam = new List<DbParam>();
            for (int i = 0; i < pis.Length; i++)
            {
                if (t.IsAutoId)
                {
                    if (t.PrimaryKey == pis[i].Name)
                        continue;
                }
                strSql.Append("" + pis[i].Name + ","); //构造SQL语句前半部份 
                strParameter.Append("@" + pis[i].Name + ","); //构造参数SQL语句
                listParam.Add(new DbParam
                {
                    ParamName = pis[i].Name,
                    ParamDbType = DbTypeConvert.GetDbType(pis[i].PropertyType),
                    ParamValue = pis[i].GetValue(model, null)
                });
            }
            strSql = strSql.Replace(",", ")", strSql.Length - 1, 1);
            strParameter = strParameter.Replace(",", ")", strParameter.Length - 1, 1);
            strSql.Append(" values (");
            strSql.Append(strParameter + ";");
            if (t.IsAutoId)
            {
                strSql.Append("select last_insert_id()");
            }
            Database db = DatabaseFactory.CreateDatabase(connName);
            using (DbCommand cmd = db.GetSqlStringCommand(strSql.ToString()))
            {
                foreach (DbParam dbpm in listParam)
                {
                    db.AddInParameter(cmd, dbpm.ParamName, dbpm.ParamDbType, dbpm.ParamValue);
                }
                object obj = db.ExecuteScalar(cmd);
                return DbTypeConvert.ToInt32(obj);
            }
        }

        #endregion Add

        #region Update
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="model">需要更新到数据库的实体类</param>
        public virtual bool Update(T model)
        {
            return Update(model, null);
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="model">需要更新到数据库的实体类</param>
        /// <param name="sColList">需要更新的字段</param>
        public virtual bool Update(T model, string sColList)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update  " + TableName + " set ");
            PropertyInfo[] pis = ModelProperties;
            //  typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public);
            List<DbParam> listParam = new List<DbParam>();
            if (string.IsNullOrEmpty(sColList))
            {
                for (int i = 0; i < pis.Length; i++)
                {
                    if (pis[i].Name != PrimaryKey)
                    {
                        strSql.AppendFormat("[{0}]=@{0},", pis[i].Name);
                    }
                    listParam.Add(new DbParam
                    {
                        ParamDbType = DbTypeConvert.GetDbType(pis[i].PropertyType),
                        ParamName = pis[i].Name,
                        ParamValue = pis[i].GetValue(model, null)
                    });
                }
            }
            else
            {
                string[] strArr = sColList.Split(',');
                foreach (PropertyInfo pi in pis)
                {
                    for (int i = 0; i < strArr.Length; i++)
                    {
                        if (pi.Name == PrimaryKey || pi.Name != strArr[i]) continue;
                        strSql.AppendFormat("[{0}]=@{0},", pi.Name);
                    }

                    listParam.Add(new DbParam
                    {
                        ParamDbType = DbTypeConvert.GetDbType(pi.PropertyType),
                        ParamName = pi.Name,
                        ParamValue = pi.GetValue(model, null)
                    });
                }
            }
            strSql = strSql.Replace(",", " ", strSql.Length - 1, 1);
            strSql.Append(" where " + PrimaryKey + "=@" + PrimaryKey);
            Database db = DatabaseFactory.CreateDatabase(connName);
            using (DbCommand cmd = db.GetSqlStringCommand(strSql.ToString()))
            {
                foreach (DbParam dbpm in listParam)
                {
                    db.AddInParameter(cmd, dbpm.ParamName, dbpm.ParamDbType, dbpm.ParamValue);
                }
                return db.ExecuteNonQuery(cmd) > 0 ? true : false;
            }
        }
        #endregion Update

        #region GetModel

        /// <summary>
        /// 获得一个Model对象实例
        /// </summary>
        /// <param name="id">主键ID的值</param>
        public virtual T GetModel(int id)
        {
            return GetModel(string.Format("{0}={1}", PrimaryKey, id));
        }

        public virtual T GetModel(long id)
        {
            return GetModel(string.Format("{0}={1}", PrimaryKey, id));
        }

        /// <summary>
        /// 获得一个Model对象实例
        /// </summary>
        /// <param name="ID">主键ID的值</param>
        /// <param name="sColList">以逗号分隔的查询字段名称</param>
        public virtual T GetModel(int ID, string sColList)
        {
            string strWhere = PrimaryKey + "=" + ID.ToString();
            return GetModel(strWhere, sColList, 0);
        }

        /// <summary>
        /// 获得一个Model对象实例
        /// </summary>
        /// <param name="strWhere">Where子句</param>
        public virtual T GetModel(string strWhere)
        {
            return GetModel(strWhere, null);
        }

        public virtual T GetModel(string strWhere, List<DbParam> listPm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format("select top 1 {0} from {1}", "*", TableName));
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append("  where " + strWhere);
            T model = default(T);
            Database db = DatabaseFactory.CreateDatabase(connName);
            using (DbCommand cmd = db.GetSqlStringCommand(strSql.ToString()))
            {
                if (listPm != null)
                {
                    foreach (DbParam pm in listPm)
                    {
                        db.AddInParameter(cmd, pm.ParamName, pm.ParamDbType, pm.ParamValue);
                    }
                }
                using (IDataReader dr = db.ExecuteReader(cmd))
                {
                    if (dr.Read())
                        model = GetModel(dr);
                }
            }
            return model;
        }

        /// <summary>
        /// 获得一个Model对象实例
        /// </summary>
        /// <param name="strWhere">Where子句</param>
        /// <param name="sColList">以逗号分隔的查询字段名称</param>
        /// <param name="iIndex">标识该实体在数据库中是第几个</param>
        public virtual T GetModel(string strWhere, string sColList, int iIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format("select top {0} {1} from {2}", iIndex + 1, sColList, TableName));
            if (strWhere != "")
                strSql.Append(string.Format("  where {0}", strWhere));
            T model = default(T);
            Database db = DatabaseFactory.CreateDatabase(connName);
            DbCommand cmd = db.GetSqlStringCommand(strSql.ToString());
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                //while (iIndex > 0)
                //{
                //    if (dr.Read())
                //        iIndex--;
                //    else
                //        return null;
                //}
                if (dr.Read())
                    model = GetModel(dr);
            }
            return model;
        }

        /// <summary>
        /// 获得一个Model对象实例,带参数
        /// </summary>
        public virtual T GetModelNew(IDataReader dr, string colList)
        {

            T model = new T();
            PropertyInfo[] pis = ModelProperties;
            //   typeof (T).GetProperties(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public);
            int iIndex;
            StringBuilder sb = new StringBuilder();
            foreach (PropertyInfo pi in pis)
            {
                string[] colListArr = colList.Split(',');

                if (colListArr.Contains(pi.Name) || pi.Name == colList)
                {
                    try
                    {

                        iIndex = dr.GetOrdinal(pi.Name);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        continue;
                    }
                    if (dr.IsDBNull(iIndex))
                        continue;
                    dr.GetValue(iIndex);
                    pi.SetValue(model, dr.GetValue(iIndex), null);
                }
            }

            return GetModel(model);
        }

        /// <summary>
        /// 获得一个Model对象实例
        /// </summary>
        public virtual T GetModel(IDataReader dr)
        {

            T model = new T();
            PropertyInfo[] pis = ModelProperties;
            //   typeof (T).GetProperties(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public);
            int iIndex;
            StringBuilder sb = new StringBuilder();
            foreach (PropertyInfo pi in pis)
            {

                try
                {

                    iIndex = dr.GetOrdinal(pi.Name);
                }
                catch (IndexOutOfRangeException)
                {
                    continue;
                }
                if (dr.IsDBNull(iIndex))
                    continue;
                dr.GetValue(iIndex);
                pi.SetValue(model, dr.GetValue(iIndex), null);

            }

            return GetModel(model);
        }

        public virtual T GetModel(T model)
        {
            return model;
        }

        /// <summary>
        /// 获得一个Model对象实例
        /// </summary>
        /// <param name="drv">数据行视图</param>
        /// <returns>Model对象实例</returns>
        public virtual T GetModel(DataRowView drv)
        {
            T model = new T();
            PropertyInfo[] pis = model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public);
            foreach (PropertyInfo pi in pis)
            {
                try
                {
                    if (drv[pi.Name].ToString() != "")
                    {
                        pi.SetValue(model, drv[pi.Name], null);
                    }
                }
                catch (ArgumentException)
                {
                    continue;
                }
            }
            return model;
        }

        #endregion GetModel

        #region GetMosaicModel

        public virtual T GetMosaicModel(string strSql)
        {
            T model = default(T);
            Database db = DatabaseFactory.CreateDatabase(connName);
            using (DbCommand cmd = db.GetSqlStringCommand(strSql))
            {
                using (IDataReader dr = db.ExecuteReader(cmd))
                {
                    if (dr.Read())
                        model = GetModel(dr);
                }
            }
            return model;
        }

        #endregion

        #region  GetList

        /// <summary>
        /// 获得数据集
        /// </summary>
        /// <returns></returns>
        public virtual List<T> GetList()
        {
            return GetList(null, 200000, 1, true, "*", PrimaryKey, TableName);
        }

        /// <summary>
        /// 获得数据集
        /// </summary>
        /// <param name="strWhere">Where子句</param>
        public virtual List<T> GetList(string strWhere,List<DbParam> parms=null)
        {
            return GetList(strWhere, 200000, 1, true, "*", PrimaryKey, TableName, parms);
        }

        /// <summary>
        /// 获得数据集
        /// </summary>
        /// <param name="strWhere">Where子句</param>
        /// <param name="PageSize">每页显示记录数</param>
        /// <param name="PageIndex">当前页码</param>
        public virtual List<T> GetList(string strWhere, int PageSize, int PageIndex, List<DbParam> parms = null)
        {
            return GetList(strWhere, PageSize, PageIndex, true, "*", PrimaryKey, TableName, parms);
        }

        /// <summary>
        /// 获得数据集
        /// </summary>
        /// <param name="strWhere">Where子句</param>
        /// <param name="PageSize">每页显示记录数</param>
        /// <param name="PageIndex">当前页码</param>
        /// <param name="bOrderType">排序规则(true-降序；flase-升序)</param>
        public virtual List<T> GetList(string strWhere, int PageSize, int PageIndex, bool bOrderType, List<DbParam> parms = null)
        {
            return GetList(strWhere, PageSize, PageIndex, bOrderType, "*", PrimaryKey, TableName, parms);
        }

        /// <summary>
        /// 获得数据集
        /// </summary>
        /// <param name="strWhere">Where子句</param>
        /// <param name="PageSize">每页显示记录数</param>
        /// <param name="PageIndex">当前页码</param>
        /// <param name="bOrderType">排序规则(true-降序；flase-升序)</param>
        /// <param name="colList">以逗号分隔的查询字段名称</param>
        public virtual List<T> GetList(string strWhere, int PageSize, int PageIndex, bool bOrderType, string colList, List<DbParam> parms = null)
        {
            return GetList(strWhere, PageSize, PageIndex, bOrderType, colList, PrimaryKey, TableName, parms);
        }

        /// <summary>
        /// 获得数据集
        /// </summary>
        /// <param name="strWhere">Where子句</param>
        /// <param name="PageSize">每页显示记录数</param>
        /// <param name="PageIndex">当前页码</param>
        /// <param name="bOrderType">排序规则(true-降序；flase-升序)</param>
        /// <param name="colList">以逗号分隔的查询字段名称</param>
        /// <param name="fldOrder">排序字段</param>
        public virtual List<T> GetList(string strWhere, int PageSize, int PageIndex, bool bOrderType, string colList,
                                       string fldOrder, List<DbParam> parms = null)
        {
            return GetList(strWhere, PageSize, PageIndex, bOrderType, colList, fldOrder, TableName, parms);
        }

        /// <summary>
        /// 获得数据集
        /// </summary>
        /// <param name="strWhere">Where子句</param>
        /// <param name="PageSize">每页显示记录数</param>
        /// <param name="PageIndex">当前页码</param>
        /// <param name="bOrderType">排序规则(true-降序；flase-升序)</param>
        /// <param name="colList">以逗号分隔的查询字段名称</param>
        /// <param name="fldOrder">排序字段</param>
        /// <param name="sTableName">表名</param>
        protected virtual List<T> GetList(string strWhere, int PageSize, int PageIndex, bool bOrderType, string colList,
                                          string fldOrder, string sTableName, List<DbParam> parms = null)
        {
            using (
                IDataReader dr = GetDataReaderByPage(strWhere, PageSize, PageIndex, bOrderType, colList, fldOrder,
                                                     TableName, parms))
            {

                if (colList.Trim() == "*")
                {
                    return GetList(dr);
                }
                return GetListNew(dr, colList);
            }
        }

        /// <summary>
        /// 获得新List集合 By 陈江
        /// </summary>
        /// <param name="dr">将DataReader里的实体转到List</param>
        public virtual List<T> GetListNew(IDataReader dr, string colList)
        {
            List<T> list = new List<T>();
            while (dr.Read())
            {
                list.Add(GetModelNew(dr, colList));
            }
            return list;
        }

        /// <summary>
        /// 获得List集合
        /// </summary>
        /// <param name="dr">将DataReader里的实体转到List</param>
        public virtual List<T> GetList(IDataReader dr)
        {
            List<T> list = new List<T>();
            while (dr.Read())
            {
                list.Add(GetModel(dr));
            }
            return list;
        }

        /// <summary>
        /// 获得数据集
        /// </summary>
        /// <param name="dv">数据视图</param>
        /// <returns>数据集</returns>
        public virtual List<T> GetList(DataView dv)
        {
            List<T> list = new List<T>();
            foreach (DataRowView drv in dv)
            {
                list.Add(GetModel(drv));
            }
            return list;
        }

        /// <summary>
        /// 获得数据集
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <returns>数据集</returns>
        public virtual List<T> GetList(DataTable dt)
        {
            List<T> list = new List<T>();
            foreach (DataRowView drv in dt.DefaultView)
            {
                list.Add(GetModel(drv));
            }
            return list;
        }

        #endregion  GetList
        protected virtual List<T> GetListPro(string strWhere, int PageSize, int PageIndex, bool bOrderType, string colList, string fldOrder, string sTableName)
        {
            List<T> list;
            using (IDataReader dataReaderByPagePro = this.GetDataReaderByPagePro(strWhere, PageSize, PageIndex, bOrderType, colList, fldOrder, this.TableName))
            {
                list = this.GetList(dataReaderByPagePro);
            }
            return list;
        }
        #region GetMosaicList
        public virtual List<T> GetMosaicList(string strSql)
        {
            Database db = DatabaseFactory.CreateDatabase(connName);
            using (DbCommand cmd = db.GetSqlStringCommand(strSql))
            {
                using (IDataReader dr = db.ExecuteReader(cmd))
                {
                    return GetList(dr);
                }
            }
        }
        #endregion

        #region  GetReader

        /// <summary>
        /// 获得数据集
        /// </summary>
        /// <returns></returns>
        public IDataReader GetReader()
        {
            return GetReader(null);
        }

        /// <summary>
        /// 获得数据集
        /// </summary>
        /// <param name="strWhere">Where子句</param>
        public virtual IDataReader GetReader(string strWhere)
        {
            return GetReader(strWhere, 2000, 1, true, "*", PrimaryKey, TableName);
        }

        /// <summary>
        /// 获得数据集
        /// </summary>
        /// <param name="strWhere">Where子句</param>
        /// <param name="PageSize">每页显示记录数</param>
        /// <param name="PageIndex">当前页码</param>
        public virtual IDataReader GetReader(string strWhere, int PageSize, int PageIndex)
        {
            return GetReader(strWhere, PageSize, PageIndex, true, "*", PrimaryKey, TableName);
        }

        /// <summary>
        /// 获得数据集
        /// </summary>
        /// <param name="strWhere">Where子句</param>
        /// <param name="PageSize">每页显示记录数</param>
        /// <param name="PageIndex">当前页码</param>
        /// <param name="bOrderType">排序规则(true-降序；flase-升序)</param>
        public virtual IDataReader GetReader(string strWhere, int PageSize, int PageIndex, bool bOrderType)
        {
            return GetReader(strWhere, PageSize, PageIndex, bOrderType, "*", PrimaryKey, TableName);
        }

        /// <summary>
        /// 获得数据集
        /// </summary>
        /// <param name="strWhere">Where子句</param>
        /// <param name="PageSize">每页显示记录数</param>
        /// <param name="PageIndex">当前页码</param>
        /// <param name="bOrderType">排序规则(true-降序；flase-升序)</param>
        /// <param name="colList">以逗号分隔的查询字段名称</param>
        public virtual IDataReader GetReader(string strWhere, int PageSize, int PageIndex, bool bOrderType,
                                             string colList)
        {
            return GetReader(strWhere, PageSize, PageIndex, bOrderType, colList, PrimaryKey, TableName);
        }

        /// <summary>
        /// 获得数据集
        /// </summary>
        /// <param name="strWhere">Where子句</param>
        /// <param name="PageSize">每页显示记录数</param>
        /// <param name="PageIndex">当前页码</param>
        /// <param name="bOrderType">排序规则(true-降序；flase-升序)</param>
        /// <param name="colList">以逗号分隔的查询字段名称</param>
        /// <param name="fldOrder">排序字段</param>
        public virtual IDataReader GetReader(string strWhere, int PageSize, int PageIndex, bool bOrderType,
                                             string colList, string fldOrder)
        {
            return GetReader(strWhere, PageSize, PageIndex, bOrderType, colList, fldOrder, TableName);
        }

        internal IDataReader GetReader(string strWhere, int PageSize, int PageIndex, bool bOrderType, string colList,
                                       string fldOrder, string sTableName)
        {
            return GetDataReaderByPage(strWhere, PageSize, PageIndex, bOrderType, colList, fldOrder, TableName);
        }

        #endregion  GetReader

        #region 分页查询

        /// <summary>
        /// 根据翻页信息获取记录到DataReader
        /// </summary>
        /// <param name="strWhere">Where子句</param>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="PageIndex">当前页码</param>
        /// <param name="OrderType">排序规则(true-降序；flase-升序)</param>
        /// <param name="colList">以逗号分隔的查询列名称</param>
        /// <param name="fldOrder">排序字段名称</param>
        /// <param name="tblName">表名</param>
        private IDataReader GetDataReaderByPage(string strWhere, int PageSize, int PageIndex, bool OrderType,
                                                string colList, string fldOrder, string tblName, List<DbParam> parms = null)
        {
            string strSql = BuildSql(strWhere, PageSize, PageIndex, OrderType, colList, fldOrder, tblName);
            Database db = DatabaseFactory.CreateDatabase(connName);
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            if (parms != null)
            {
                foreach (var parm in parms)
                {
                    db.AddInParameter(cmd, parm.ParamName, parm.ParamDbType, parm.ParamValue);
                }
            }
            using (cmd)
            {
                return db.ExecuteReader(cmd);
            }
        }
        private IDataReader GetDataReaderByPagePro(string strWhere, int PageSize, int PageIndex, bool OrderType, string colList, string fldOrder, string tblName)
        {
            Database database = DatabaseFactory.CreateDatabase(this.connName);
            DbCommand storedProcCommand = database.GetStoredProcCommand("GetRecordFromPageV2");
            database.AddInParameter(storedProcCommand, "tblName", DbType.String, "Topics");
            database.AddInParameter(storedProcCommand, "fldName", DbType.String, fldOrder);
            database.AddInParameter(storedProcCommand, "strWhere", DbType.String, strWhere);
            database.AddInParameter(storedProcCommand, "IsCount", DbType.Byte, false);
            database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, PageSize);
            database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, PageIndex);
            database.AddInParameter(storedProcCommand, "OrderType", DbType.Byte, OrderType);
            return database.ExecuteReader(storedProcCommand);
        }
        protected virtual List<T> GetListProcedure(string procedureName, List<DbParam> listPm)
        {
            List<T> list;
            using (IDataReader dataReaderByPagePro = this.GetDataReaderByProcedure(procedureName, listPm))
            {
                list = this.GetList(dataReaderByPagePro);
            }
            return list;
        }
        private IDataReader GetDataReaderByProcedure(string procedureName, List<DbParam> listPm)
        {
            Database db = DatabaseFactory.CreateDatabase(this.connName);
            DbCommand cmd = db.GetStoredProcCommand(procedureName);
            if (listPm != null)
            {
                foreach (DbParam pm in listPm)
                {
                    db.AddInParameter(cmd, pm.ParamName, pm.ParamDbType, pm.ParamValue);
                }
            }
            return db.ExecuteReader(cmd);
        }

        /// <param name="strWhere">组成SQL语句的Where子句</param>
        /// <param name="PageSize">每页显示记录数</param>
        /// <param name="PageIndex">当前需要获取的页码</param>
        /// <param name="OrderType">排序规则(true-降序；flase-升序)</param>
        /// <param name="colList">逗号分隔的字段名字符串</param>
        /// <param name="fldOrder">排序字段</param>
        /// <param name="tblName">表名</param>
        private string BuildSql(string strWhere, int PageSize, int PageIndex, bool OrderType, string colList,
                                string fldOrder, string tblName)
        {
            string sColList = "";
            if (string.IsNullOrEmpty(colList) || colList == "*")
            {
                PropertyInfo[] pis = ModelProperties;
                // typeof (T).GetProperties(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public);
                foreach (PropertyInfo pi in pis)
                {
                    sColList += "" + pi.Name + ",";
                }
                sColList = sColList.Substring(0, sColList.Length - 1);
            }
            else
            {
                sColList = colList; //CString.GetSQLFildList(colList);
            }
            StringBuilder strSql = new StringBuilder();
            string strOrder; // -- 排序类型
            if (string.IsNullOrEmpty(fldOrder))
            {
                fldOrder = PrimaryKey;
            }
            if (OrderType)
            {
                strOrder = string.Format(" order by {0} desc", fldOrder);
            }
            else
            {
                strOrder = string.Format(" order by {0} asc", fldOrder);
            }
            if (string.IsNullOrEmpty(strWhere))
            {
                if (PageIndex == 1)
                {
                    strSql.Append(string.Format("select {0} from {1} {2} limit {3}",
                                                sColList, tblName, strOrder,PageSize));
                }
                else
                {
                    strSql.Append(string.Format("select {0} from {1} {2} limit {3},{4}",
                                                sColList, tblName, strOrder, (PageIndex - 1) * PageSize + 1,PageSize));
                }
            }
            else
            {
                if (PageIndex == 1)
                {
                    strSql.Append(string.Format("select {0} from {1} Where {2} {3} limit {4}",
                                                 sColList, tblName, strWhere, strOrder,PageSize));
                }
                else
                {
                    strSql.Append(string.Format("select {0} from {1} where {2} {3} limit {4},{5}",
                                                sColList, tblName, strWhere, strOrder, (PageIndex - 1) * PageSize + 1,PageSize));
                }
            }
            return strSql.ToString();
        }

        #endregion 分页查询

        #region 辅助函数

        /// <summary>
        /// 批量添加或更新
        /// </summary>
        /// <param name="list"></param>
        /// <param name="eAdd"></param>
        /// <returns></returns>
        public virtual int AddUpdateList(List<T> list, AddUpdateType eAdd)
        {
            int iCount = 0;
            bool IsAdd = false;
            if (eAdd == AddUpdateType.Add)
                IsAdd = true;
            Database db = DatabaseFactory.CreateDatabase(connName);
            using (DbCommand cmd = db.GetSqlStringCommand(GetAddUpdateSql(IsAdd)))
            {
                List<DbParam> listParam = GetAddUpdatePms(null, IsAdd);
                foreach (DbParam dbpm in listParam)
                {
                    db.AddInParameter(cmd, dbpm.ParamName, dbpm.ParamDbType, null);
                }
                foreach (T model in list)
                {
                    listParam = GetAddUpdatePms(model, IsAdd);
                    foreach (DbParam dbpm in listParam)
                    {
                        cmd.Parameters[dbpm.ParamName].Value = dbpm.ParamValue;
                    }
                    if (db.ExecuteNonQuery(cmd) > 0)
                    {
                        iCount++;
                    }
                }
                return iCount;
            }
        }

        protected virtual string GetAddUpdateSql(bool IsAdd)
        {
            StringBuilder strSql = new StringBuilder();
            if (IsAdd)
            {
                StringBuilder strParameter = new StringBuilder();
                strSql.Append(string.Format("insert into {0}(", TableName));
                PropertyInfo[] pis = ModelProperties;
                // typeof (T).GetProperties(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public);
                for (int i = 0; i < pis.Length; i++)
                {
                    if (t.IsAutoId)
                    {
                        if (t.PrimaryKey == pis[i].Name)
                            continue;
                    }
                    strSql.Append(pis[i].Name + ","); //构造SQL语句前半部份 
                    strParameter.Append("@" + pis[i].Name + ","); //构造参数SQL语句
                }
                strSql = strSql.Replace(",", ")", strSql.Length - 1, 1);
                strParameter = strParameter.Replace(",", ")", strParameter.Length - 1, 1);
                strSql.Append(" values (");
                strSql.Append(strParameter.ToString());
            }
            else
            {
                strSql.Append("update  " + TableName + " set ");
                PropertyInfo[] pis = ModelProperties;
                //  typeof (T).GetProperties(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public);
                for (int i = 0; i < pis.Length; i++)
                {
                    if (pis[i].Name != PrimaryKey)
                    {
                        strSql.Append(pis[i].Name + "=" + "@" + pis[i].Name + ",");
                    }
                    //strSql.Append("\r\n");
                }
                strSql = strSql.Replace(",", " ", strSql.Length - 1, 1);
                strSql.Append(" where " + PrimaryKey + "=@" + PrimaryKey);
            }
            return strSql.ToString();
        }

        protected virtual List<DbParam> GetAddUpdatePms(T model, bool IsAdd)
        {
            PropertyInfo[] pis = ModelProperties;
            List<DbParam> listParam = new List<DbParam>();
            for (int i = 0; i < pis.Length; i++)
            {
                if (IsAdd)
                {
                    if (t.IsAutoId)
                    {
                        if (t.PrimaryKey == pis[i].Name)
                            continue;
                    }
                }
                if (model == null)
                    listParam.Add(new DbParam
                    {
                        ParamName = pis[i].Name,
                        ParamDbType = DbTypeConvert.GetDbType(pis[i].PropertyType),
                        ParamValue = null
                    });
                else
                    listParam.Add(new DbParam
                    {
                        ParamName = pis[i].Name,
                        ParamDbType = DbTypeConvert.GetDbType(pis[i].PropertyType),
                        ParamValue = pis[i].GetValue(model, null)
                    });
            }
            return listParam;
        }

        #endregion 辅助函数

        #region GetDataTable

        /// <summary>
        /// 获得数据表
        /// </summary>
        /// <param name="strWhere">Where子句</param>
        /// <param name="PageSize">每页显示记录数</param>
        /// <param name="PageIndex">当前页码</param>
        /// <param name="OrderType">排序规则(true-降序；flase-升序)</param>
        /// <param name="colList">以逗号分隔的查询字段名称</param>
        /// <param name="fldOrder">排序字段</param>
        /// <returns>数据表</returns>
        public virtual DataTable GetDataTable(string strWhere, int PageSize, int PageIndex, bool OrderType, string colList, string fldOrder)
        {
            string strSQL = BuildSql(strWhere, PageSize, PageIndex, OrderType, colList, fldOrder, TableName);
            Database db = DatabaseFactory.CreateDatabase(connName);
            DbCommand cmd = db.GetSqlStringCommand(strSQL);
            return db.ExecuteDataSet(cmd).Tables[0];
        }

        #endregion GetDataTable
    }
}
