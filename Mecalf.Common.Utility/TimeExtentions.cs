namespace Mecalf.Common.Utility
{
    /// <summary>
    /// 时间相关扩展
    /// </summary>
    public static class TimeExtentions
    {
        /// <summary>
        /// 将时间转换Java中的时间戳
        /// </summary>
        public static int ToTimestamp(this System.DateTime value)
        {
            var span = (value - new System.DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
            return (int)span.TotalSeconds;
        }

        /// <summary>
        /// 将Java的时间戳转换成DateTime对象
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static System.DateTime ToDateTime(this double timestamp)
        {
            var converted = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            var newDateTime = converted.AddSeconds(timestamp);
            return newDateTime.ToLocalTime();
        }
    }


}