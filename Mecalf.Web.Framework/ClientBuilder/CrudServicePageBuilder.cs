//using System.Collections.Generic;
//using Abp.Application.Services.Dto;
//using Mecalf.Web.Framework.Services;
//using Mecalf.Web.Framework.Services.Dto;

//namespace Mecalf.Web.Framework.ClientBuilder
//{

//    /// <summary>
//    /// 生成CRUD操作的页面的生成器
//    /// </summary>
//    /// <typeparam name="TEntityDto">实体的DTO</typeparam>
//    /// <typeparam name="TPrimaryKey">实体的主键</typeparam>
//    /// <typeparam name="TCreateInput">创建实体时的输入 </typeparam>
//    /// <typeparam name="TUpdateInput">更新实体时的输入 </typeparam>
//    /// <typeparam name="TGetListInput">获取所有实体时的输入</typeparam>
//    public class CrudServicePageBuilder<TEntityDto, TPrimaryKey, TCreateInput, TUpdateInput, TGetListInput> : ICrudAppService<TEntityDto, TPrimaryKey, TCreateInput, TUpdateInput, TGetListInput>
//        where TEntityDto : IEntityDto<TPrimaryKey>
//        where TCreateInput : IEntityDto<TPrimaryKey>
//        where TUpdateInput : IEntityDto<TPrimaryKey>
//    {
//        /// <summary>
//        /// 创建实体的输入
//        /// </summary>
//        TCreateInput CreateInput { get; set; }

//        /// <summary>
//        /// 创建实体的输出
//        /// </summary>
//        ApiResult<TEntityDto> CreateOutput { get; set; }

//        /// <summary>
//        /// 删除实体的输入
//        /// </summary>
//        List<TPrimaryKey> DeleteInput { get; set; }

//        /// <summary>
//        /// 删除实体的输出
//        /// </summary>
//        ApiResult DeleteOutput { get; set; }

//        /// <summary>
//        /// 更新实体的输入
//        /// </summary>
//        TUpdateInput UpdateInput { get; set; }
//        /// <summary>
//        /// 更新实体的输出
//        /// </summary>
//        ApiResult<TEntityDto> UpdateOutput { get; set; }

//        /// <summary>
//        /// 单个获取实体的输入
//        /// </summary>
//        TPrimaryKey GetInput { get; set; }
//        /// <summary>
//        /// 单个获取实体的输出
//        /// </summary>
//        ApiResult<TEntityDto> GetOutput { get; set; }

//        /// <summary>
//        /// 批量获取实体的输入
//        /// </summary>
//        TGetListInput GetListInput { get; set; }
//        /// <summary>
//        /// 批量获取实体的输出
//        /// </summary>
//        PagedApiResult<List<TEntityDto>> GetListOutput { get; set; }

//        private TemplateInterpreter interpreter;

//        public CrudServicePageBuilder<TEntityDto, TPrimaryKey, TCreateInput, TUpdateInput, TGetListInput>(ICrudAppService<TEntityDto, TPrimaryKey, TCreateInput, TUpdateInput, TGetListInput> service)
//        {

//        }

//        #region 不实现和Crud服务相关的内容

//        /// <summary>
//        /// 创建一个指定类型的对象并保存到数据库中
//        /// </summary>
//        /// <param name="input"></param>
//        /// <returns></returns>
//        public ApiResult<TEntityDto> Create(TCreateInput input)
//        {
//            throw new System.NotImplementedException();
//        }

//        /// <summary>
//        /// 从系统中删除所有给定Id的记录
//        /// </summary>
//        /// <param name="input">需要删除的Id的数组</param>
//        /// <returns></returns>
//        public ApiResult Delete(List<TPrimaryKey> input)
//        {
//            throw new System.NotImplementedException();
//        }

//        /// <summary>
//        /// 在系统中更新指定的记录（单个）
//        /// </summary>
//        /// <param name="input"></param>
//        /// <returns></returns>
//        public ApiResult<TEntityDto> Update(TUpdateInput input)
//        {
//            throw new System.NotImplementedException();
//        }

//        /// <summary>
//        /// 从系统中读取满足给定条件的所有的数据记录
//        /// 
//        /// </summary>
//        /// <param name="input"></param>
//        /// <returns></returns>
//        public PagedApiResult<List<TEntityDto>> List(TGetListInput input)
//        {
//            throw new System.NotImplementedException();
//        }

//        /// <summary>
//        /// 获取指定Id的数据记录
//        /// </summary>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        public ApiResult<TEntityDto> Get(TPrimaryKey id)
//        {
//            throw new System.NotImplementedException();
//        }

//        /// <summary>
//        /// 获取指定Id的数据记录
//        /// </summary>
//        /// <param name="ids"></param>
//        /// <returns></returns>
//        public ApiResult<List<TEntityDto>> GetByIds(List<TPrimaryKey> ids)
//        {
//            throw new System.NotImplementedException();
//        }
//        #endregion
//    }

//}
