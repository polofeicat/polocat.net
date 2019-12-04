using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LangReflectionUtility
{

    /// <summary>
    /// 
    /// </summary>
    public static class EnumerableListUtils
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static int CalcSize(this IEnumerable<object> collection)
        {
            int i = 0;

            foreach (var item in collection)
            {
                ++i;
            }

            return i;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static int CalcSize(this IEnumerable collection)
        {
            int i = 0;

            foreach (var item in collection)
            {
                ++i;
            }

            return i;
        }

    }
}
