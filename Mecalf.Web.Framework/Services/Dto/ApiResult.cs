using Newtonsoft.Json;
using System;

namespace Mecalf.Web.Framework.Services.Dto
{

    /// <summary>
    /// API的基础返回对象
    /// </summary>
    /// <typeparam name="T">返回的数据内容的类型</typeparam>
    [Serializable]
    public class ApiResult<T>
    {
        /// <summary>
        /// 请求是否处理成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 返回的数据
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public T Data { get; set; }

        /// <summary>
        /// 附加的错误信息，不出错时一般为空。
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Msg { get; set; }

        /// <summary>
        /// 默认的构造方法,默认Success为True,Data为空
        /// </summary>
        public ApiResult(bool success = true, T data = default(T), string msg = "")
        {
            Success = success;
            Data = data;
            Msg = msg;
        }
    }

    /// <summary>
    /// API的基础返回对象（非泛型版本）
    /// </summary>
    public class ApiResult : ApiResult<object>
    {
        /// <summary>
        /// 默认的构造方法,默认Success为True,Data为空
        /// </summary>
        public ApiResult(bool success = true, object data = null)
        {
            Success = success;
            Data = data;
        }

        /// <summary>
        /// 返回一个代表着处理成功的Api返回结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResult<T> Succeed<T>(T data = default(T))
        {
            return new ApiResult<T>(true, data);
        }

        /// <summary>
        /// 返回一个代表着处理失败的Api返回结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResult<T> Failed<T>(string msg = "", T data = default(T))
        {
            return new ApiResult<T>(false, data, msg);
        }
    }
}
