using System.Collections.Generic;
using Abp.Application.Services;
using Abp.Application.Services.Dto;


namespace Mecalf.Web.Framework.Services
{
    /// <summary>
    /// 按照RestFul规范设计的通用的增删改查的API接口
    /// </summary>
    /// <typeparam name="TEntityDto">实体的DTO</typeparam>
    public interface ICrudAppService<TEntityDto> : ICrudAppService<TEntityDto, int>
        where TEntityDto : IEntityDto<int>
    {

    }
    /// <summary>
    /// 按照RestFul规范设计的通用的增删改查的API接口
    /// </summary>
    /// <typeparam name="TEntityDto">实体的DTO</typeparam>
    /// <typeparam name="TPrimaryKey">实体的主键</typeparam>
    public interface ICrudAppService<TEntityDto, TPrimaryKey> : ICrudAppService<TEntityDto, TPrimaryKey, TEntityDto, TEntityDto, PagedAndSortedResultRequestDto, TEntityDto>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {

    }

    /// <summary>
    /// 按照RestFul规范设计的通用的增删改查的API接口
    /// </summary>
    /// <typeparam name="TEntityDto">实体的DTO</typeparam>
    /// <typeparam name="TPrimaryKey">实体的主键</typeparam>
    /// <typeparam name="TCreateInput">创建实体时的输入 </typeparam>
    /// <typeparam name="TUpdateInput">更新实体时的输入 </typeparam>
    public interface ICrudAppService<TEntityDto, TPrimaryKey, in TCreateInput, in TUpdateInput> : ICrudAppService<
        TEntityDto, TPrimaryKey, TCreateInput, TUpdateInput, PagedAndSortedResultRequestDto, TEntityDto>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TCreateInput : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {

    }
    /// <summary>
    /// 按照RestFul规范设计的通用的增删改查的API接口
    /// </summary>
    /// <typeparam name="TEntityDto">实体的DTO</typeparam>
    /// <typeparam name="TPrimaryKey">实体的主键</typeparam>
    /// <typeparam name="TCreateInput">创建实体时的输入 </typeparam>
    /// <typeparam name="TUpdateInput">更新实体时的输入 </typeparam>
    /// <typeparam name="TGetListInput">获取所有实体时的输入</typeparam>
    public interface ICrudAppService<TEntityDto, TPrimaryKey, in TCreateInput, in TUpdateInput, in TGetListInput> : ICrudAppService<
        TEntityDto, TPrimaryKey, TCreateInput, TUpdateInput, TGetListInput, TEntityDto>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TCreateInput : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {

    }

    /// <summary>
    /// 按照RestFul规范设计的通用的增删改查的API接口
    /// </summary>
    /// <typeparam name="TEntityDto">实体的DTO</typeparam>
    /// <typeparam name="TPrimaryKey">实体的主键</typeparam>
    /// <typeparam name="TCreateInput">创建实体时的输入 </typeparam>
    /// <typeparam name="TUpdateInput">更新实体时的输入 </typeparam>
    /// <typeparam name="TGetListInput">获取所有实体时的输入</typeparam>
    /// <typeparam name="TListEntityDto">列表显示实体时的输出</typeparam>
    public interface ICrudAppService<TEntityDto, TPrimaryKey, in TCreateInput, in TUpdateInput, in TGetListInput, TListEntityDto> : IApplicationService
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TCreateInput : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TListEntityDto : IEntityDto<TPrimaryKey>
    {
        /// <summary>
        /// 创建一个指定类型的对象并保存到数据库中
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        TEntityDto Create(TCreateInput input);

        /// <summary>
        /// 从系统中删除所有给定Id的记录
        /// </summary>
        /// <param name="input">需要删除的Id的数组</param>
        /// <returns></returns>
        object Delete(List<TPrimaryKey> input);

        /// <summary>
        /// 在系统中更新指定的记录（单个）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        TEntityDto Update(TUpdateInput input);

        /// <summary>
        /// 从系统中读取满足给定条件的所有的数据记录
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagedResultDto<TListEntityDto> GetList(TGetListInput input);

        /// <summary>
        /// 获取指定Id的数据记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntityDto Get(TPrimaryKey id);

        /// <summary>
        /// 获取指定Id的数据记录
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        ListResultDto<TEntityDto> GetByIds(List<TPrimaryKey> ids);

    }
}