using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LangReflectionUtility
{

    /// <summary>
    /// 
    /// </summary>
    public static class PropertyInfoUtils
    {


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        /// <returns></returns>
        public static T[] GetAllCustomAttributes<T>(this PropertyInfo property)
        {
            object[] attrs = property.GetCustomAttributes(typeof(T), false);
            if (attrs == null || attrs.Length == 0)
                return new T[0];

            List<T> lst = new List<T>();
            foreach (var attr in attrs)
            {
                lst.Add((T)attr);
            }
            return lst.ToArray();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        /// <returns></returns>
        public static bool HasCustomAttributes<T>(this PropertyInfo property)
        {
            T[] attrs = property.GetAllCustomAttributes<T>();
            return attrs != null && attrs.Length != 0;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static bool IsIndexProperty(this PropertyInfo property)
        {
            return property.GetIndexParameters().Length > 0;
        }

    }
}
