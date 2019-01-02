using System;

namespace Mecalf.Common.Utility
{
    /// <summary>
    /// 在业务处理中常用到的一些数据转换或计算的扩展
    /// </summary>
    public static class ValueExtensions
    {
        /// <summary>
        /// 由当前的DateTime对象计算出生日
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int ToAge(this DateTime dateTime)
        {
            //todo::这样计算出的年龄有较大误差

            return (DateTime.Today - dateTime).Days / 365;
        }
        /// <summary>
        /// 由当前的DateTime对象计算出生日,如果指定的时间为空则返回<see cref="int.MaxValue"/>
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int ToAge(this DateTime? dateTime)
        {
            //todo::这样计算出的年龄有较大误差,且可能无法在Linq中使用
            return dateTime.HasValue ? (DateTime.Today - dateTime.Value).Days / 365 : int.MaxValue;
        }
    }
}
