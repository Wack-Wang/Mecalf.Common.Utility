using System;
using System.Collections.Generic;
using System.Linq;

namespace Mecalf.Common.Utility
{
    /// <summary>
    /// 集合操作相关的扩展
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// 对给定的枚举器进行遍历并执行指定的操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="action"></param>
        public static void Each<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T item in enumerable)
            {
                action(item);
            }
        }
        /// <summary>
        /// 判断集合是否为空，为空返回true
        /// </summary>
        /// <typeparam name="T">要验证的对象的类型</typeparam>
        /// <param name="data">要验证的对象</param>        
        public static bool IsNullOrEmpty<T>(this IQueryable<T> data)
        {
            //如果为null
            if (data == null)
            {
                return true;
            }

            //不为空
            return data.Any() == false;
        }

        /// <summary>
        /// 判断集合是否为空，为空返回true
        /// </summary>
        /// <typeparam name="T">要验证的对象的类型</typeparam>
        /// <param name="data">要验证的对象</param>        
        public static bool IsNullOrEmpty<T>(this ICollection<T> data)
        {
            //如果为null
            if (data == null)
            {
                return true;
            }

            //不为空
            return data.Any() == false;
        }

        /// <summary>
        /// 判断集合是否为空，为空返回true
        /// </summary>
        /// <typeparam name="T">要验证的对象的类型</typeparam>
        /// <param name="data">要验证的对象</param>        
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> data)
        {
            //如果为null
            if (data == null)
            {
                return true;
            }

            //不为空
            return data.Any() == false;
        }
    }
}