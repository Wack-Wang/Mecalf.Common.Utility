using System.Collections.Generic;

namespace Mecalf.Web.Framework.Services.Dto
{
    /// <summary>
    /// 获取数据的输入
    /// </summary>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public interface IGetInput<TPrimaryKey>
    {
        /// <summary>
        /// 目标记录的唯一ID
        /// </summary>

        IEnumerable<TPrimaryKey> Ids { get; set; }

        /// <summary>
        /// 分页信息
        /// </summary>
        IPagedGetInput Paging { get; set; }
    }
}
