using System;
using System.Collections.Generic;
using System.Linq;

namespace Mecalf.Common.Utility
{
    public static class LinqExtension
    {
        /// <summary>
        /// 如果给定的对象不为空，则执行where，否则不执行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">数据源</param>
        /// <param name="obj">需要判空的对象</param>
        /// <param name="predicate">判断条件</param>
        /// <returns></returns>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, object obj, Func<T, bool> predicate)
        {
            return obj != null ? source.Where(predicate) : source;
        }

        /// <summary>
        /// 如果给定的字符串不为空，则执行where，否则不执行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">数据源</param>
        /// <param name="str">需要判空的对象</param>
        /// <param name="predicate">判断条件</param>
        /// <returns></returns>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, string str, Func<T, bool> predicate)
        {
            return string.IsNullOrWhiteSpace(str) == false ? source.Where(predicate) : source;
        }
        /// <summary>
        /// 如果给定的条件不为假，则执行where，否则不执行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">数据源</param>
        /// <param name="condition">需要判断的值</param>
        /// <param name="predicate">判断条件</param>
        /// <returns></returns>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, bool> predicate)
        {
            return condition ? source.Where(predicate) : source;
        }
        /// <summary>
        /// 如果给定的条件不为假，则执行where，否则不执行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">数据源</param>
        /// <param name="condition">需要判断的值</param>
        /// <param name="predicate">判断条件</param>
        /// <returns></returns>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, int, bool> predicate)
        {
            return condition ? source.Where(predicate) : source;
        }
        /// <summary>
        /// 如果给定的条件不为假，则执行where，否则不执行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">数据源</param>
        /// <param name="condition">需要判断的值</param>
        /// <param name="predicate">判断条件</param>
        /// <returns></returns>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool? condition, Func<T, bool> predicate)
        {
            return condition == true ? source.Where(predicate) : source;
        }
        /// <summary>
        /// 如果给定的条件不为假，则执行where，否则不执行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">数据源</param>
        /// <param name="condition">需要判断的值</param>
        /// <param name="predicate">判断条件</param>
        /// <returns></returns>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool? condition, Func<T, int, bool> predicate)
        {
            return condition == true ? source.Where(predicate) : source;
        }
        /// <summary>
        /// 如果给定的条件不为假，则执行where，否则不执行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">数据源</param>
        /// <param name="condition">需要判断的值</param>
        /// <param name="predicate">判断条件</param>
        /// <returns></returns>
        public static IEnumerable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Func<T, bool> predicate)
        {
            return condition ? source.AsEnumerable().Where(predicate) : source;
        }
    }
}
