using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Mecalf.Common.Utility;
using Mecalf.Web.Framework.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Mecalf.Web.Framework.Services
{
    /// <summary>
    /// 按照RestFul规范设计的通用的增删改查的API接口
    /// </summary>
    /// <typeparam name="TEntity">数据库实体</typeparam>
    /// <typeparam name="TEntityDto">实体的DTO,必须能和 <typeparamref name="TEntity"/> 相互映射</typeparam>
    /// <typeparam name="TPrimaryKey">实体的主键</typeparam>
    public abstract class CrudAppServiceBase<TEntity, TEntityDto, TPrimaryKey> : CrudAppServiceBase<TEntity, TEntityDto,
        TPrimaryKey, PagedAndSortedGetInput<TPrimaryKey>>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
        /// <summary>
        /// 构造方法，暂无特殊说明
        /// </summary>
        /// <param name="repository"></param>
        protected CrudAppServiceBase(IRepository<TEntity, TPrimaryKey> repository) : base(repository)
        {
        }
    }

    /// <summary>
    /// 按照RestFul规范设计的通用的增删改查的API接口
    /// </summary>
    /// <typeparam name="TEntity">数据库实体</typeparam>
    /// <typeparam name="TEntityDto">实体的DTO,必须能和 <typeparamref name="TEntity"/> 相互映射</typeparam>
    /// <typeparam name="TPrimaryKey">实体的主键</typeparam>
    /// <typeparam name="TGetInput">获取实体时的输入</typeparam>
    public abstract class CrudAppServiceBase<TEntity, TEntityDto, TPrimaryKey, TGetInput> : CrudAppServiceBase<TEntity,
        TEntityDto, TPrimaryKey, TEntityDto, TEntityDto, TGetInput>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TGetInput : IGetInput<TPrimaryKey>
    {
        /// <summary>
        /// 构造方法，暂无特殊说明
        /// </summary>
        /// <param name="repository"></param>
        protected CrudAppServiceBase(IRepository<TEntity, TPrimaryKey> repository) : base(repository)
        {
        }
    }

    /// <summary>
    /// 按照RestFul规范设计的通用的增删改查的API接口
    /// </summary>
    /// <typeparam name="TEntity">数据库实体</typeparam>
    /// <typeparam name="TEntityDto">实体的DTO</typeparam>
    /// <typeparam name="TPrimaryKey">实体的主键</typeparam>
    /// <typeparam name="TCreateInput">创建实体时的输入，必须能够映射到<typeparamref name="TEntity"/> </typeparam>
    /// <typeparam name="TUpdateInput">更新实体时的输入，必须能够映射到<typeparamref name="TEntity"/> </typeparam>
    /// <typeparam name="TGetInput">获取实体时的输入</typeparam>

    public abstract class CrudAppServiceBase<TEntity, TEntityDto, TPrimaryKey, TCreateInput, TUpdateInput, TGetInput> : ApplicationService
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TGetInput : IGetInput<TPrimaryKey>
        where TCreateInput : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {
        private readonly CrudAppService<TEntity, TEntityDto, TPrimaryKey, PagedAndSortedResultRequestDto, TCreateInput, TUpdateInput> _innerService;
        private readonly IRepository<TEntity, TPrimaryKey> _repository;

        /// <summary>
        /// 构造方法，暂无特殊说明
        /// </summary>
        /// <param name="repository"></param>
        protected CrudAppServiceBase(IRepository<TEntity, TPrimaryKey> repository)
        {
            _innerService = new McmsCrudServiceInner(repository);
            _repository = repository;
        }

        #region 已经完成
        /// <summary>
        /// 创建一个指定类型的对象并保存到数据库中
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult<TEntityDto> Create(TCreateInput input)
        {
            try
            {
                return new ApiResult<TEntityDto>(true, _innerService.Create(input));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ApiResult<TEntityDto>(false) { Msg = e.ToString() };
            }
        }

        /// <summary>
        /// 从系统中删除所有给定Id的记录
        /// </summary>
        /// <param name="input">需要删除的Id的数组</param>
        /// <returns></returns>
        [HttpDelete]
        public ApiResult Delete(List<TPrimaryKey> input)
        {
            try
            {
                _repository.Delete(entity => input.Contains(entity.Id));
                return new ApiResult(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ApiResult(false) { Msg = e.ToString() };
            }
        }

        /// <summary>
        /// 在系统中更新指定的记录（单个）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //TODO：警告：这里的Update和RestFul的Put请求并不匹配
        [HttpPut]
        public ApiResult<TEntityDto> Update(TUpdateInput input)
        {
            try
            {
                return new ApiResult<TEntityDto>(true, _innerService.Update(input));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ApiResult<TEntityDto>(false) { Msg = e.ToString() };
            }
        }
        #endregion


        /// <summary>
        /// 从系统中读取满足给定条件的所有的数据记录，有两种工作，一种是
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<List<TEntityDto>> Get(IGetInput<TPrimaryKey> input)
        {
            try
            {
                var query = _repository.GetAllIncluding();
                if (input.Ids.IsNullOrEmpty() == false)
                {
                    query = query.Where(entity => input.Ids.Contains(entity.Id));
                }

                query = Get_Paging(query, input.Paging);
                var result = query.Select(entity => ObjectMapper.Map<TEntityDto>(entity)).ToList();
                return ApiResult.Succeed(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ApiResult.Failed<List<TEntityDto>>(e.ToString());
            }
        }

        /// <summary>
        /// 查询数据中的部分功能：对查询到的结果进行分页处理
        /// </summary>
        /// <param name="result">需要分页的数据</param>
        /// <param name="paging">分页参数</param>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> Get_Paging(IQueryable<TEntity> result, IPagedGetInput paging)
        {
            if (result.IsNullOrEmpty() || paging.IsNullOrEmpty())
            {
                return result;
            }

            if (paging.OrderBy.IsNotNullOrWhiteSpace())
            {
                result = System.Linq.Dynamic.DynamicQueryable.OrderBy(result, $"{paging.OrderBy} {(paging.Asc ? "ASC" : "DESC")}");
            }

            if (paging.Skip.HasValue)
            {
                result = result.Skip(paging.Skip.Value);
            }

            if (paging.Take.HasValue)
            {
                result = result.Take(paging.Take.Value);
            }

            return result;
        }

        /// <summary>
        /// 实现基础的增删改查服务的内部类
        /// </summary>
        class McmsCrudServiceInner : CrudAppService<TEntity, TEntityDto, TPrimaryKey, PagedAndSortedResultRequestDto, TCreateInput, TUpdateInput>
        {
            public McmsCrudServiceInner(IRepository<TEntity, TPrimaryKey> repository) : base(repository)
            {
            }
        }
    }
}
