using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.Data.SqlClient;
using System.Reflection;
namespace LeaveHelper.Core
{
    public static class MyConvert
    {
        /// <summary>
        /// 对象xml序列化
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="o">对象值</param>
        /// <returns></returns>
        public static string ObjectToXml<T>(T o)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            StringBuilder sb = new StringBuilder();
            XmlWriter xw = XmlWriter.Create(sb);
            xs.Serialize(xw, o);
            xw.Flush();
            xw.Close();

            return sb.ToString().Replace("utf-16", "utf-8");
        }
        public static T SqlDataReaderToObject<T>(SqlDataReader reader)
        {
            if (reader == null) return default(T);
            T obj = Activator.CreateInstance<T>();
            PropertyInfo[] pis = typeof(T).GetProperties();
            string value;
            foreach (PropertyInfo pi in pis)
            {
                try
                {
                    value = reader[reader.GetOrdinal(pi.Name)].ToString();
                }
                catch
                {

                    continue;
                }
                switch (pi.PropertyType.ToString())
                {
                    case "System.String": pi.SetValue(obj, value, null); break;
                    case "System.Int32": pi.SetValue(obj, int.Parse(value), null); break;
                    case "System.Int64": pi.SetValue(obj, long.Parse(value), null); break;
                    case "System.DateTime": pi.SetValue(obj, DateTime.Parse(value), null); break;
                    case "System.Int16": pi.SetValue(obj, int.Parse(value), null); break;
                }
            }
            return obj;
        }

        //public static string ObjectToXml(Object o,Type t)
        //{
        //    XmlSerializer xs = new XmlSerializer(t);
        //    StringBuilder sb = new StringBuilder();
        //    XmlWriter xw = XmlWriter.Create(sb);
        //    xs.Serialize(xw, o);
        //    xw.Flush();
        //    xw.Close();
        //    return sb.ToString();
        //}
    }
}
