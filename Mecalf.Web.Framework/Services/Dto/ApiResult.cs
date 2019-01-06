using System;
using Newtonsoft.Json;

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
        /// 错误代码，不出错时一般为空。
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? ErrorCode { get; set; }

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
        /// <returns></returns>
        public static ApiResult Succeed()
        {
            return new ApiResult(true);
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
        /// <param name="msg">返回的消息</param>
        /// <returns></returns>
        public static ApiResult Failed(string msg)
        {
            return new ApiResult(false) { Msg = msg };
        }

        /// <summary>
        /// 返回一个代表着处理失败的Api返回结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg">返回的消息</param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResult<T> Failed<T>(string msg = "", T data = default(T))
        {
            return new ApiResult<T>(false, data, msg);
        }
    }

    /// <summary> 
    /// 分页的Api结果返回
    /// </summary>
    public class PagedApiResult<TData> : ApiResult<TData>
    {
        /// <summary>
        /// 默认的构造方法,不初始化任何信息
        /// </summary>
        public PagedApiResult()
        {

        }

        /// <summary>
        /// 构造方法,默认Success为True,Data为空
        /// </summary>
        public PagedApiResult(bool success = true, TData data = default(TData))
        {
            Success = success;
            Data = data;
        }

        /// <summary>
        /// 构造方法,默认Success为True,Data为空
        /// </summary>
        public PagedApiResult(bool success = true, TData data = default(TData), string msg = "")
        {
            Success = success;
            Data = data;
            Msg = msg;
        }

        /// <summary>
        /// 总的符合条件的记录数
        /// </summary>
        public int Total { get; set; }
    }

    /// <inheritdoc />
    public class PagedApiResult : PagedApiResult<object>
    {
        /// <summary>
        /// 返回一个代表着处理成功的Api返回结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">返回的数据</param>
        /// <returns></returns>
        public static PagedApiResult<T> Succeed<T>(int total, T data = default(T))
        {
            return new PagedApiResult<T>(true, data) { Total = total };
        }

        /// <summary>
        /// 返回一个代表着处理失败的Api返回结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg">提示消息</param>
        /// <param name="data">返回的数据</param>
        /// <returns></returns>
        public static PagedApiResult<T> Failed<T>(string msg = "", T data = default(T))
        {
            return new PagedApiResult<T>(false, data, msg);
        }
    }
}
