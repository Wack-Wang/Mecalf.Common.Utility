namespace Mecalf.Web.Framework.Services.Dto
{
    /// <summary>
    /// 分页获取数据的输入
    /// </summary>
    public interface IPagedGetInput
    {
        /// <summary>
        /// 跳过多少条记录
        /// </summary>
        int? Skip { get; set; }

        /// <summary>
        /// 每一页有多少条记录
        /// </summary>
        int? Take { get; set; }

        /// <summary>
        /// 用于排序的字段
        /// </summary>
        string OrderBy { get; set; }

        /// <summary>
        /// 是否升序排列结果,
        /// </summary>
        bool Asc { set; get; }
    }
}
