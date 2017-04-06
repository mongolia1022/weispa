using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace com.ccfw.Dal.Base
{
    public static class DbTypeConvert
    {
        public static string CShrapTypeFormat(string typeName, object value, string format)
        {
            string str = value.ToString();
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            typeName = typeName.ToLower();
            string str3 = typeName;
            if (str3 == null)
            {
                return str;
            }
            if (str3 != "int32" && str3 != "int")
            {
                if (str3 != "datetime")
                {
                    if (str3 != "float")
                    {
                        return str;
                    }
                    return Convert.ToDecimal(value).ToString(format);
                }
            }
            else
            {
                return Convert.ToInt32(value).ToString(format);
            }
            return Convert.ToDateTime(value).ToString(format);
        }

        public static DateTime GetDateTime(IDataReader dataReader, int i)
        {
            if (dataReader.IsDBNull(i))
            {
                return DateTime.MinValue;
            }
            return dataReader.GetDateTime(i);
        }

        public static DbType GetDbType(Type sysType)
        {
            DbType type = DbType.String;
            switch (sysType.Name)
            {
                case "String":
                    return DbType.String;

                case "Int16":
                    return DbType.Int16;

                case "Int32":
                    return DbType.Int32;

                case "Int64":
                    return DbType.Int64;

                case "Byte":
                    return DbType.Byte;

                case "DateTime":
                    return DbType.DateTime;

                case "Double":
                    return DbType.Double;

                case "Boolean":
                    return DbType.Boolean;

                case "Guid":
                    return DbType.Guid;
            }
            return type;
        }

        public static int GetInt32(IDataReader dataReader, int i)
        {
            if (dataReader.IsDBNull(i))
            {
                return -2147483648;
            }
            return dataReader.GetInt32(i);
        }

        public static string GetString(IDataReader dataReader, int i)
        {
            if (dataReader.IsDBNull(i))
            {
                return "";
            }
            return dataReader.GetString(i);
        }

        public static object ToCShrapType(string typeName, string value)
        {
            object obj2 = value;
            typeName = typeName.ToLower();
            string str = typeName;
            if (str == null)
            {
                return obj2;
            }
            if (str != "int32" && str != "int")
            {
                if (str != "datetime")
                {
                    if (str != "float")
                    {
                        return obj2;
                    }
                    return ((value == "") ? 0M : Convert.ToDecimal(value));
                }
            }
            else
            {
                return ((value == "") ? 0 : Convert.ToInt32(value));
            }
            return ((value == "") ? new DateTime(0x76c, 1, 1) : Convert.ToDateTime(value));
        }

        public static DateTime ToDateTime(object dbValue)
        {
            if ((dbValue == null) || (dbValue == DBNull.Value))
            {
                return DateTime.MinValue;
            }
            return Convert.ToDateTime(dbValue);
        }

        public static object ToDBValue(DateTime time)
        {
            if (time == DateTime.MinValue)
            {
                return DBNull.Value;
            }
            return time;
        }

        public static object ToDBValue(int d)
        {
            if (d == -2147483648)
            {
                return DBNull.Value;
            }
            return d;
        }

        public static object ToDBValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return DBNull.Value;
            }
            return value;
        }

        public static int ToInt32(object dbValue)
        {
            if ((dbValue == null) || (dbValue == DBNull.Value))
            {
                return -2147483648;
            }
            return Convert.ToInt32(dbValue);
        }

        public static long ToInt64(object dbValue)
        {
            if ((dbValue == null) || (dbValue == DBNull.Value))
            {
                return -9223372036854775808L;
            }
            return Convert.ToInt64(dbValue);
        }
    }
}
