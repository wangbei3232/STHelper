using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class SThelper
    {
        #region 刘恒洋

        /// <summary>
        /// 反射得到实体类的字段名称和值
        /// var dict = GetProperties(model);
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="Model">实例化</param>
        /// <returns></returns>
        public int AddByModel<T>(T Model)
        {
            //获取表名（Model实体类的名字）
            string TName = Model.GetType().Name;
            //如果表名为空，直接返回int 0 结束
            if (Model == null) { return 0; }

            //用反射函数获取Model的类型属性，如果获取到的行数为空则返回int 0 结束
            PropertyInfo[] properties = Model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (properties.Length <= 0) { return 0; }

            //初始化一个Sqlparameter类型的数组parms
            //声明string变量namestring、valuestring用以后续拼接sql语句的键名和值
            List<SqlParameter> parms = new List<SqlParameter>();
            string namestring = "";
            string valuestring = "";

            foreach (PropertyInfo item in properties)
            {
                string name = item.Name;
                object value = item.GetValue(Model, null);

                //如果item的类型为值的类型或item的name为string类型
                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    //如果value值不为空且name不等FId    
                    //拼接中不拼接空值的列，且FId为sql自动生成，不进行添加。
                    //由于model中FId为必填项，不添加会自动生成值为0，所以在此处排除
                    if (value != null && name != "FId")
                    {
                        //初始化sqlparameter lin并每次循环时添加拼接内容
                        //每次循环添加并拼接sql需要的键名namestring和值valuestring
                        SqlParameter lin = new SqlParameter("@" + name, value);
                        parms.Add(lin);
                        namestring = name + "," + namestring;
                        valuestring = "@" + name + "," + valuestring;
                    }
                }
            }

            //拼接处sql语句
            //对para赋值
            string sql = "insert into " + TName + "(" + namestring.Substring(0, namestring.Length - 1) + ") values (" + valuestring.Substring(0, valuestring.Length - 1) + ")";
            SqlParameter[] para = parms.ToArray();

            return SqlHelper.ExecuteNonQuery(SqlHelper.connStr, CommandType.Text, sql, para);
        }


        /// <summary>
        /// 根据Model修改数据 使用FId为条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Model"></param>
        /// <returns></returns>
        public int EditByModel<T>(T Model)
        {
            //获取表名（Model实体类的名字）
            string TName = Model.GetType().Name;
            //如果表名为空，直接返回int 0 结束
            if (Model == null) { return 0; }

            //用反射函数获取Model的类型属性，如果获取到的行数为空则返回int 0 结束
            PropertyInfo[] properties = Model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (properties.Length <= 0) { return 0; }

            //初始化一个Sqlparameter类型的数组parms
            //声明string变量namestring、valuestring用以后续拼接sql语句的键名和值
            List<SqlParameter> parms = new List<SqlParameter>();
            string sql = "Update "+TName+" SET ";
            foreach (PropertyInfo item in properties)
            {
                string name = item.Name;
                object value = item.GetValue(Model, null);

                //如果item的类型为值的类型或item的name为string类型
                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    //如果value值不为空且name不等FId    
                    //拼接中不拼接空值的列，且FId为sql自动生成，不进行添加。
                    //由于model中FId为必填项，不添加会自动生成值为0，所以在此处排除
                    if (value != null && name != "FId")
                    {
                        //初始化sqlparameter lin并每次循环时添加拼接内容
                        //每次循环添加并拼接sql需要的键名namestring和值valuestring
                        SqlParameter lin = new SqlParameter("@" + name, value);
                        parms.Add(lin);
                        sql = sql + name+"=@"+name+",";
                    }
                    if (name == "FId")
                    {
                        SqlParameter lin = new SqlParameter("@FId", value);
                        parms.Add(lin);
                    }
                }
            }
            sql = sql.Substring(0, sql.Length - 1) + " Where FId=@FId";
            //拼接处sql语句
            //对para赋值
            SqlParameter[] para = parms.ToArray();

            return SqlHelper.ExecuteNonQuery(SqlHelper.connStr, CommandType.Text, sql, para);
        }


        /// <summary>
        /// 删除
        /// 注意Model内如果出现多个值  删除条件为and
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Model"></param>
        /// <returns></returns>
        public int Delete<T>(T Model)
        {
            //获取表名（Model实体类的名字）
            string TName = Model.GetType().Name;
            //如果表名为空，直接返回int 0 结束
            if (Model == null) { return 0; }

            //用反射函数获取Model的类型属性，如果获取到的行数为空则返回int 0 结束
            PropertyInfo[] properties = Model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (properties.Length <= 0) { return 0; }

            //初始化一个Sqlparameter类型的数组parms
            //声明string变量namestring、valuestring用以后续拼接sql语句的键名和值
            List<SqlParameter> parms = new List<SqlParameter>();
            string sql = "delete from " + TName + " where 1=1 ";

            foreach (PropertyInfo item in properties)
            {
                string name = item.Name;
                object value = item.GetValue(Model, null);

                //如果item的类型为值的类型或item的name为string类型
                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    //如果value值不为空且name不等FId    
                    //拼接中不拼接空值的列，且FId为sql自动生成，不进行添加。
                    //由于model中FId为必填项，不添加会自动生成值为0，所以在此处排除
                    if (value != null && (name == "FId"&& value.ToString()!="0"))
                    {
                        //初始化sqlparameter lin并每次循环时添加拼接内容
                        //每次循环添加并拼接sql需要的键名namestring和值valuestring
                        SqlParameter lin = new SqlParameter("@" + name, value);
                        parms.Add(lin);
                        sql = sql + " and " + name + "=@" + name;
                    }
                }
            }
            SqlParameter[] para = parms.ToArray();
            return SqlHelper.ExecuteNonQuery(SqlHelper.connStr, CommandType.Text, sql, para);
        }


        /// <summary>
        /// 删除根据FId
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Model"></param>
        /// <returns></returns>
        public int DeleteByFId<T>(T Model)
        {
            //获取表名（Model实体类的名字）
            string TName = Model.GetType().Name;
            //如果表名为空，直接返回int 0 结束
            if (Model == null) { return 0; }
            //用反射函数获取Model的类型属性，如果获取到的行数为空则返回int 0 结束
            PropertyInfo[] properties = Model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (properties.Length <= 0) { return 0; }

            //初始化一个Sqlparameter类型的数组parms
            //声明string变量namestring、valuestring用以后续拼接sql语句的键名和值
            List<SqlParameter> parms = new List<SqlParameter>();
            string sql = "delete from " + TName + " where 1=1 ";

            foreach (PropertyInfo item in properties)
            {
                //如果item的类型为值的类型或item的name为string类型
                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    string name = item.Name;
                    object value = item.GetValue(Model, null);
                    //主键且值不为0
                    if (name == "FId" && value.ToString() != "0")
                    {
                        //初始化sqlparameter lin并每次循环时添加拼接内容
                        //每次循环添加并拼接sql需要的键名namestring和值valuestring
                        SqlParameter lin = new SqlParameter("@" + name, value);
                        parms.Add(lin);
                        sql = sql + " and " + name + "=@" + name;
                    }
                }
            }
            SqlParameter[] para = parms.ToArray();
            return SqlHelper.ExecuteNonQuery(SqlHelper.connStr, CommandType.Text, sql, para);
        }



        /// <summary>
        /// 根据Model中的FId，获取该FId对应的Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Model"></param>
        /// <returns></returns>
        public T GetModelByFId<T>(T Model)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            //获取表名（Model实体类的名字）
            string TName = Model.GetType().Name;
            //用反射函数获取Model的类型属性
            PropertyInfo[] properties = Model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            string sql = "select Top 1 ";
            foreach (PropertyInfo item in properties)
            {
                string name = item.Name;
                //如果item的类型为值的类型或item的name为string类型
                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    sql = sql + name + ",";
                    if (name == "FId")
                    {
                        object value = item.GetValue(Model, null);
                        //初始化sqlparameter lin并每次循环时添加拼接内容
                        //每次循环添加并拼接sql需要的键名namestring和值valuestring
                        SqlParameter lin = new SqlParameter("@FId", value);
                        parms.Add(lin);
                    }
                }
            }
            //拼接处sql语句
            sql = sql.Substring(0, sql.Length - 1) + " from " + TName + " where FId=@FId";
            SqlParameter[] para = parms.ToArray();

            DataTable dt = SqlHelper.ExecuteDataTable(SqlHelper.connStr, sql, para);
            if (dt.Rows.Count > 0)
            {
                return TableModel.RowConvertModel<T>(dt.Rows[0]);
            }
            else
            {
                return default(T);
            }
            
        }


        /// <summary>
        /// 根据Model中的某个值，获取该值对应的其他参数（一条其他参数，返回值为Model）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Model"></param>
        /// <returns></returns>
        public T GetModelBy<T>(T Model)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            //获取表名（Model实体类的名字）
            string TName = Model.GetType().Name;
            //用反射函数获取Model的类型属性
            PropertyInfo[] properties = Model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            string sql = "select Top 1 ";
            string where = " where 1=1 ";
            foreach (PropertyInfo item in properties)
            {
                string name = item.Name;
                object value = item.GetValue(Model, null);
                //如果item的类型为值的类型或item的name为string类型
                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    sql = sql + name + ",";
                    if (value!=null)
                    {
                        if (name == "FId" && Convert.ToInt32(value) == 0)
                        {

                        }
                        else
                        {
                            where = where + " and " + name + "=@" + name;
                            //初始化sqlparameter lin并每次循环时添加拼接内容
                            //每次循环添加并拼接sql需要的键名namestring和值valuestring
                            SqlParameter lin = new SqlParameter("@" + name, value);
                            parms.Add(lin);
                        }

                    }
                }
            }
            //拼接处sql语句
            sql = sql.Substring(0, sql.Length - 1) + " from " + TName + where;
            SqlParameter[] para = parms.ToArray();

            DataTable dt = SqlHelper.ExecuteDataTable(SqlHelper.connStr, sql, para);
            if (dt.Rows.Count > 0)
            {
                return TableModel.RowConvertModel<T>(dt.Rows[0]);
            }
            else
            {
                return default(T);
            }
        }


        /// <summary>
        /// 根据Model中的某个值，获取该值对应的所有数据的表格
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Model"></param>
        /// <returns></returns>
        public DataTable GetTableBy<T>(T Model)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            //获取表名（Model实体类的名字）
            string TName = Model.GetType().Name;
            //用反射函数获取Model的类型属性
            PropertyInfo[] properties = Model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            string sql = "select ";
            string where = " where 1=1 ";
            string order = "";
            foreach (PropertyInfo item in properties)
            {
                string name = item.Name;
                object value = item.GetValue(Model, null);
                //如果item的类型为值的类型或item的name为string类型
                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    sql = sql + name + ",";
                    if (value != null)
                    {
                        if (name == "FId" && Convert.ToInt32(value) == 0 && value.ToString() == "0")
                        {
                            order = " order by " + name;
                        }
                        else
                        {
                            where = where + " and " + name + "=@" + name;
                            //初始化sqlparameter lin并每次循环时添加拼接内容
                            //每次循环添加并拼接sql需要的键名namestring和值valuestring
                            SqlParameter lin = new SqlParameter("@" + name, value);
                            parms.Add(lin);
                        }

                    }
                }
            }
            //拼接处sql语句
            sql = sql.Substring(0, sql.Length - 1) + " from " + TName + where+ order;
            SqlParameter[] para = parms.ToArray();

            DataTable dt = SqlHelper.ExecuteDataTable(SqlHelper.connStr, sql, para);

            return dt;
        }








        #endregion


        #region 王备
        /// <summary>
        /// 验证数据是否为空
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string IsNullToString(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            else
            {
                return obj.ToString();
            }

        }


        /// <summary>
        /// 验证Model内数据是否重复
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Model"></param>
        /// <returns></returns>
        public int IsRedo<T>(T Model) {
            int i = 0;
            //获取表名（Model实体类的名字）
            string TName = Model.GetType().Name;
            //用反射函数获取Model的类型属性
            PropertyInfo[] properties = Model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo item in properties)
            {
                object value = item.GetValue(Model, null);
                string name = item.Name;
                //如果item的类型为值的类型或item的name为string类型
                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    if (value != null && name != "FId")
                    {
                        string sql = "select COUNT(1) from " + TName + " where " + name + "=@" + name;
                        string a=  SqlHelper.ExecuteScalar(SqlHelper.connStr, sql, new SqlParameter[] { new SqlParameter("@" + name, value) });
                        i =i+Convert.ToInt32(a);
                    }
                }
            }
            return i;
        }


        /// <summary>
        /// 动态获取分页数据列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Model">需要数据的Model</param>
        /// <param name="ModelSearch">进行查询的Model</param>
        /// <param name="searchWhere">查询的条件</param>
        /// <param name="tiaoshu"></param>
        /// <param name="yema"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public DataTable GetTableList<T>(T Model,T ModelSearch,EnumHelpe.SearchWhere searchWhere, int tiaoshu, int yema, out int total)
        {
            //获取表名（Model实体类的名字）
            string TName = Model.GetType().Name;
            //用反射函数获取Model的类型属性
            PropertyInfo[] properties = Model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            //用反射函数获取ModelSearch的类型属性
            PropertyInfo[] propertiesSearch = ModelSearch.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            //初始化一个Sqlparameter类型的数组parms
            //声明string变量namestring、valuestring用以后续拼接sql语句的键名和值
            List<SqlParameter> parms = new List<SqlParameter>();
            string sql = "select ";
            //获取查询的字段
            foreach (PropertyInfo item in properties)
            {
                string name = item.Name;
                object value = item.GetValue(Model, null);
                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    if (value != null)
                    {
                        sql = sql + name + ",";
                    }
                }
                    
            }
            string where = " where 1=1 ";
            string whereF = "";
            
            //获取搜索的字段
            foreach (PropertyInfo item in propertiesSearch)
            {
                string name = item.Name;
                object value = item.GetValue(ModelSearch, null);
                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    if (value != null && name != "FId")
                    {
                        if (item.PropertyType.IsValueType)
                        {
                            if (Convert.ToInt32(value) != 0)
                            {
                                SqlParameter lin = new SqlParameter("@" + name, value);
                                parms.Add(lin);
                                //数字  注意  这里时间需要做特殊处理
                                whereF = whereF  + " " + name + "=@" + name + " " + searchWhere.ToString();
                            }
                        }
                        else if (item.PropertyType.Name.StartsWith("String"))
                        {
                            if (value.ToString()!="") {
                                SqlParameter lin = new SqlParameter("@" + name, value);
                                parms.Add(lin);
                                //字符
                                whereF = whereF + " " + name + "like @" + name + " " + searchWhere.ToString();

                            }
                        }
                    }
                }
            }
            if (whereF.Length > 0)
            {
                where = where +" and "+ whereF.Substring(0, whereF.Length - searchWhere.ToString().Length);
            }


            string sqlstr = sql.Substring(0, sql.Length - 1) + " from " + TName + where;

            string sql1 = "select top (@tiaoshu) * from (select row_number() over(order by FId desc) as rownumber,* from (" + sqlstr+ ") a ) temp_row where rownumber>@yema";
            //计算表格中有多少条数据
            string totalsql1 = "select COUNT(1) from ("+sqlstr+") a";
            SqlParameter[] paraTo = parms.ToArray();
            total = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.connStr, totalsql1, paraTo));

            parms.Add(new SqlParameter("@tiaoshu", tiaoshu));
            parms.Add(new SqlParameter("@yema", yema));
            SqlParameter[] para = parms.ToArray();
            return SqlHelper.ExecuteDataTable(SqlHelper.connStr, CommandType.Text, sql1, para);
        }

        #endregion
    }
}
