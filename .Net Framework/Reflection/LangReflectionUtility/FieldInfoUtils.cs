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
    public static class FieldInfoUtils
    {


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        public static T[] GetAllCustomAttributes<T>(this FieldInfo field)
        {
            object[] attrs = field.GetCustomAttributes(typeof(T), false);
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
        /// <param name="field"></param>
        /// <returns></returns>
        public static bool HasCustomAttributes<T>(this FieldInfo field)
        {
            T[] attrs = field.GetAllCustomAttributes<T>();
            return attrs != null && attrs.Length != 0;
        }
    }
}
