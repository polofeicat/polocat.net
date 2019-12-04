using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Text;

namespace LangReflectionUtility
{

    /// <summary>
    /// 
    /// </summary>
    public static class TypeUtils
    {

        public static readonly List<Type> NumberTypeList;               //实数类型。
        public static readonly List<Type> NullableNumberTypeList;       //可空实数类型。
        public static readonly List<Type> NormalValueTypeList;          //常用（可空）值类型。
        public static readonly List<Type> TableDataSetTypeList;

        /// <summary>
        /// 静态构造器中初始化Number类型列表，仅执行一次。
        /// </summary>
        static TypeUtils()
        {

            NormalValueTypeList = new List<Type>()
            {
                typeof(DateTime),   typeof(DateTime?),      //值类型的DateTime结构体
                typeof(Guid),       typeof(Guid?),          //值类型的GUID
                typeof(TimeSpan),   typeof(TimeSpan?)       //值类型的时间间隔
            };


            NumberTypeList = new List<Type>()
            {

                typeof(sbyte),      //8位有符号整形
                typeof(byte),       //8位无符号整形
                typeof(short),      //16位有符号整形
                typeof(ushort),     //16位无符号整形
                typeof(int),        //32位有符号整形
                typeof(uint),       //32位无符号整形
                typeof(long),       //64位有符号整形
                typeof(ulong),      //64位无符号整形

                typeof(float),      //32位浮点
                typeof(double),     //64位浮点
                typeof(decimal)     //128位浮点

            };

            NullableNumberTypeList = new List<Type>()
            {
                typeof(sbyte?),      //8位有符号整形
                typeof(byte?),       //8位无符号整形
                typeof(short?),      //16位有符号整形
                typeof(ushort?),     //16位无符号整形
                typeof(int?),        //32位有符号整形
                typeof(uint?),       //32位无符号整形
                typeof(long?),       //64位有符号整形
                typeof(ulong?),      //64位无符号整形

                typeof(float?),      //32位浮点
                typeof(double?),     //64位浮点
                typeof(decimal?)     //128位浮点
            };

            TableDataSetTypeList = new List<Type>()
            {
                typeof(DataSet),
                typeof(DataTable),
                typeof(DataView),
                typeof(DataRow),
                typeof(DataRowView),
                typeof(DataColumn),
                typeof(DataRowCollection),
                typeof(DataColumnCollection),
                typeof(DataTableCollection),
                typeof(DataViewSettingCollection),
                typeof(DataRelation),
                typeof(DataRelationCollection)
            };

        }


        /// <summary>
        /// 判断类型是否为16位Unicode字符。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsCharType(this Type type)
        {
            return typeof(char) == type;
        }


        /// <summary>
        /// 判断类型是否为可空16位Unicode字符。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNullableCharType(this Type type)
        {
            return typeof(char?) == type;
        }

        /// <summary>
        /// 判断类型是否为8位布尔类型。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsBooleanType(this Type type)
        {
            return typeof(bool) == type;
        }


        /// <summary>
        /// 判断类型是否为可空8位布尔类型。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNullableBooleanType(this Type type)
        {
            return typeof(bool?) == type;
        }


        /// <summary>
        /// 判断类型是否是可空数值类型(numeric?)。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNullableNumericType(this Type type)
        {
            bool result = false;
            foreach (var item in NullableNumberTypeList)
            {
                if (type == item)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }


        /// <summary>
        /// 判断类型是否是数值类型(numeric)。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNumericType(this Type type)
        {
            bool result = false;
            foreach (var item in NumberTypeList)
            {
                if (type == item)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool IsNormalValueType(this Type type)
        {
            bool result = false;
            foreach (var item in NormalValueTypeList)
            {
                if (type == item)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }


        /// <summary>
        /// 检查类型是否是数值类型、可空数值类型、简单值类型以及枚举类型和可空枚举类型。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsSimpleValueType(this Type type)
        {
            return type.IsNumericType()                         //整形、浮点等数值类型
                    || type.IsNullableNumericType()             //可空整形、浮点等数值类型
                    || type.IsCharType()                        //16位unicode字符类型
                    || type.IsNullableCharType()                //可空16位unicode字符类型
                    || type.IsBooleanType()                     //布尔类型
                    || type.IsNullableBooleanType()             //可空布尔类型
                    || type.IsNormalValueType()                 //常用值类型
                    || type.IsEnumType()                        //枚举类型
                    || type.IsNullableEnumType()                //可空枚举类型
                    || type.IsWin32PtrType();                   //是IntPtr或UIntPtr类型的Native指针
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsWin32PtrType(this Type type)
        {
            return type == typeof(IntPtr) || type == typeof(IntPtr?) || type == typeof(UIntPtr) || type == typeof(UIntPtr?);
        }


        /// <summary>
        /// 判断类型是否是Enum值类型。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsEnumType(this Type type)
        {
            return type.IsSubclassOf(typeof(Enum));
        }


        /// <summary>
        /// 判断类型是否是Enum可空值类型。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNullableEnumType(this Type type)
        {
            if (!type.IsNullableGeneric())
                return false;

            Type gt = type.GetGenericArguments()[0];
            return gt.IsEnumType();
        }


        /// <summary>
        /// 判断类型是否为可数序列(包括数组和集合)。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsCountableCollection(this Type type)
        {
            if (type.IsArray)
                return true;

            Type[] interfaceTypes = type.GetInterfaces();
            foreach (var itemType in interfaceTypes)
            {
                if (itemType == typeof(ICollection))
                    return true;
                else if (itemType.IsGenericType && itemType.GetGenericTypeDefinition() == typeof(ICollection<>))
                    return true;
            }

            return false;
        }


        /// <summary>
        /// 判断类型是否为IList或者为IList<>类型对象。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsListCollection(this Type type)
        {
            //if (typeof(ArrayList) == type || type.FullName.StartsWith("System.Collections.Generic.List"))
            //    return true;

            Type[] interfaceTypes = type.GetInterfaces();
            foreach (var itemType in interfaceTypes)
            {
                if (itemType == typeof(IList))
                    return true;
                else if (itemType.IsGenericType && itemType.GetGenericTypeDefinition() == typeof(IList<>))
                    return true;
            }

            return false;
        }


        /// <summary>
        /// 判断类型是否为ISet<>类型对象。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsSetCollection(this Type type)
        {
            //if (type.FullName.StartsWith("System.Collections.Generic.HashSet") || type.FullName.StartsWith("System.Collections.Generic.SortedSet"))
            //    return true;

            Type[] interfaceTypes = type.GetInterfaces();
            foreach (var itemType in interfaceTypes)
            {
                if (itemType.IsGenericType && itemType.GetGenericTypeDefinition() == typeof(ISet<>))
                    return true;
            }
            return false;

        }


        /// <summary>
        /// 判断类型是否为IDictionary或者为IDictionary<,>类型对象。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsDictionaryCollection(this Type type)
        {
            //if (type == typeof(Hashtable) || type == typeof(SortedList))
            //    return true;

            Type[] interfaceTypes = type.GetInterfaces();
            foreach (var itemType in interfaceTypes)
            {
                if (itemType == typeof(IDictionary))
                    return true;
                if (itemType.IsGenericType && itemType.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                    return true;
            }
            return false;
        }


        /// <summary>
        /// 判断类型是否为Stack或者为Stack<>类型对象。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsStackCollection(this Type type)
        {
            return type == typeof(Stack) || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Stack<>));
        }


        /// <summary>
        /// 判断类型是否为Queue或者为Queue<>类型对象。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsQueueCollection(this Type type)
        {
            return type == typeof(Queue) || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Queue<>));
        }


        /// <summary>
        /// 判断类型是否为只读集合(ReadOnlyCollection<>)类型。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsReadOnlyCollection(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ReadOnlyCollection<>);
        }


        /// <summary>
        /// 判断类型是否为LinkedList<>类型对象。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsLinkCollection(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(LinkedList<>);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsStreamType(this Type type)
        {
            return type == typeof(Stream) || type.IsSubclassOf(typeof(Stream));
        }


        /// <summary>
        /// 检查类型是否为表格形式的DataSet类型。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsTableDataSetType(this Type type)
        {
            bool result = false;
            foreach (var item in TableDataSetTypeList)
            {
                if (type == item)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }


        /// <summary>
        /// 检查类型是否是.Net Framework内置类型对象。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsFrameworkType(this Type type)
        {
            if (type.Assembly.GlobalAssemblyCache)
            {
                string assemblyName = type.Assembly.FullName;

                if (assemblyName.StartsWith("mscorlib"))
                    return true;

                if (assemblyName.StartsWith("Microsoft"))
                    return true;

                if (assemblyName.StartsWith("System"))
                    return true;

                if (assemblyName.StartsWith("Presentation"))
                    return true;

                if (assemblyName.StartsWith("Windows"))
                    return true;

                if (assemblyName.StartsWith("UIAutomation"))
                    return true;

            }

            return false;
        }


        /// <summary>
        /// 获取不含命名空间路径的简单类型名称。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetSimpleName(this Type type)
        {
            string typeFullName = type.FullName;
            int idx = typeFullName.LastIndexOf(".");
            if (idx == -1)
            {
                return typeFullName;
            }

            return typeFullName.Substring(idx + 1);
        }


        /// <summary>
        /// 判断类型是否是可空T类型 Nullable[T]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNullableOf<T>(this Type type)
        {
            if (!IsNullableGeneric(type))
                return false;

            Type[] generic_types = type.GetGenericArguments();
            return typeof(T) == generic_types[0];
        }


        /// <summary>
        /// 判断type是否为可空泛型。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNullableGeneric(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }


        /// <summary>
        /// 是（可变）字符串类型。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsStringSequence(this Type type)
        {
            return type == typeof(string) || type == typeof(StringBuilder);
        }


        /// <summary>
        /// 判断给定的type类型是否是T类型或者从T类型派生而来的子类。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsEqualOrDeriveFrom<T>(this Type type)
        {
            return typeof(T) == type || type.IsSubclassOf(typeof(T));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetCollectionElementType(this Type type)
        {
            if (!type.IsCountableCollection())
                return null;

            if (type.IsArray)
                return type.GetElementType();

            if (!type.IsGenericType)
                return typeof(object);

            if (type.IsDictionaryCollection())
            {
                return type.GetGenericArguments()[1];
            }

            return type.GetGenericArguments()[0];
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T[] GetAllCustomAttributes<T>(this Type type)
        {
            object[] attrs = type.GetCustomAttributes(typeof(T), false);
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
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool HasCustomAttributes<T>(this Type type)
        {
            T[] attrs = type.GetAllCustomAttributes<T>();
            return attrs != null && attrs.Length != 0;
        }


    }
}
