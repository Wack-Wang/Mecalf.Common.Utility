using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Mecalf.Common.Utility
{
    /// <summary>
    /// 安全相关的扩展
    /// </summary>
    public static class SecurityExtentions
    {
        private static readonly Lazy<MD5> _md5Lazy = new Lazy<MD5>(MD5.Create);
        /// <summary>
        /// 获取字符串的MD5
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Md5(this string str)
        {
            var sourceBit = Encoding.UTF8.GetBytes(str);
            var directBit = _md5Lazy.Value.ComputeHash(sourceBit);
            var directStr = BitConverter.ToString(directBit).Replace("-", "");
            return directStr;
        }

        /// <summary>
        /// 获取指定的字节的Md5
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string Md5(this byte[] bytes)
        {
            var directBit = _md5Lazy.Value.ComputeHash(bytes);
            var directStr = BitConverter.ToString(directBit).Replace("-", "");
            return directStr;
        }

        /// <summary>
        /// 从指定流中读取数据并计算数据的MD5
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string Md5(this Stream stream)
        {
            var directBit = _md5Lazy.Value.ComputeHash(stream);
            var directStr = BitConverter.ToString(directBit).Replace("-", "");
            return directStr;
        }

        /// <summary>
        /// 读取路径所指向的文件，计算MD5
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string FileMd5(this string filePath)
        {
            var directBit = _md5Lazy.Value.ComputeHash(File.OpenRead(filePath));
            var directStr = BitConverter.ToString(directBit).Replace("-", "");
            return directStr;
        }
    }
}