using System;
using System.Collections.Generic;

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
    }
}