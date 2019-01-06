using System;

namespace Mecalf.Web.Framework.Services.Dto
{
    /// <summary>
    /// 排序和分页获取数据的输入信息
    /// </summary>
    [Serializable]
    public class PagedAndSortedGetInput : IPagedAndSortedGetInput
    {
        /// <summary>
        /// 跳过多少条记录
        /// </summary>
        public int? Skip { get; set; } = 0;

        /// <summary>
        /// 每一页有多少条记录
        /// </summary>
        public int? Take { get; set; } = 10;

        /// <summary>
        /// 用于排序的字段
        /// </summary>
        public string OrderBy { get; set; } = "Id";

        /// <summary>
        /// 是否升序排列结果,
        /// </summary>
        public bool Asc { get; set; } = true;
    }
}