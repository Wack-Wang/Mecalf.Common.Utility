namespace Mecalf.Common.Utility
{
    /// <summary>
    /// 对象操作相关的扩展方法
    /// </summary>
    public static class ObjectExtensions
    {
        #region 判空相关扩展

        /// <summary>
        /// 判断对象是否为空，为空返回true
        /// </summary>
        /// <typeparam name="T">要验证的对象的类型</typeparam>
        /// <param name="data">要验证的对象</param>        
        public static bool IsNull<T>(this T data) where T : class
        {
            return data == null;
        }

        /// <summary>
        /// 判断一个对象是否不为null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNotNull<T>(this T obj) where T : class
        {
            return obj != null;
        }

        /// <summary>
        /// 判断对象是否类型默认值(如常见的null和0)，为空返回true
        /// </summary>
        /// <typeparam name="T">要验证的对象的类型</typeparam>
        /// <param name="data">要验证的对象</param>        
        public static bool IsDefault<T>(this T data)
        {
            return data.Equals(default(T));
        }

        /// <summary>
        /// 判断对象是否不为类型默认值(如常见的null和0)，不为空返回true
        /// </summary>
        /// <typeparam name="T">要验证的对象的类型</typeparam>
        /// <param name="data">要验证的对象</param>        
        public static bool IsNotDefault<T>(this T data)
        {
            return !data.Equals(default(T));
        }


        #endregion
    }
}