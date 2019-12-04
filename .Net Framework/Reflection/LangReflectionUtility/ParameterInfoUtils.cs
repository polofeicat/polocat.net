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
    public static class ParameterInfoUtils
    {

        /// <summary>
        /// 判断Parameter是否有可变参数标记param，因为ParameterInfo类的IsOptional属性无效，使用这个方法代替。
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static bool IsVariableParameter(this ParameterInfo parameter)
        {
            object[] attrs = parameter.GetCustomAttributes(typeof(ParamArrayAttribute), false);
            return attrs != null && attrs.Length != 0;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static T[] GetAllCustomAttributes<T>(this ParameterInfo parameter)
        {
            object[] attrs = parameter.GetCustomAttributes(typeof(T), false);
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
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static bool HasCustomAttributes<T>(this ParameterInfo parameter)
        {
            T[] attrs = parameter.GetAllCustomAttributes<T>();
            return attrs != null && attrs.Length != 0;
        }

    }
}
