using System.Collections.Generic;

namespace Mecalf.Web.Framework.Services.Dto
{
    /// <summary>
    /// 排序和分页获取数据的输入信息
    /// </summary>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public class PagedAndSortedGetInput<TPrimaryKey> : IGetInput<TPrimaryKey>
    {
        /// <summary>
        /// 目标记录的唯一ID
        /// </summary>
        public IEnumerable<TPrimaryKey> Ids { get; set; }

        /// <summary>
        /// 分页信息
        /// </summary>
        public IPagedGetInput Paging { get; set; }
    }
}