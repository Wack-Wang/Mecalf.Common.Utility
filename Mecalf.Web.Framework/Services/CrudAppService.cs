using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Repositories;
using Mecalf.Common.Utility;
using Mecalf.Web.Framework.Services.Dto;

namespace Mecalf.Web.Framework.Services
{

    /// <inheritdoc />
    /// <summary>
    /// 按照RestFul规范设计的通用的增删改查的API接口
    /// </summary>
    /// <typeparam name="TEntity">数据库实体</typeparam>
    /// <typeparam name="TEntityDto">实体的DTO,必须能和 <typeparamref name="TEntity" /> 相互映射</typeparam>
    public abstract class CrudAppService<TEntity, TEntityDto> : CrudAppService<TEntity, TEntityDto, int, TEntityDto, TEntityDto>
        where TEntity : class, IEntity<int>, ICreationAudited
        where TEntityDto : IEntityDto<int>
    {
        /// <inheritdoc />
        /// <summary>
        /// 构造方法，暂无特殊说明
        /// </summary>
        /// <param name="repository"></param>
        protected CrudAppService(IRepository<TEntity, int> repository) : base(repository)
        {
        }
    }
    /// <inheritdoc />
    /// <summary>
    /// 按照RestFul规范设计的通用的增删改查的API接口
    /// </summary>
    /// <typeparam name="TEntity">数据库实体</typeparam>
    /// <typeparam name="TEntityDto">实体的DTO,必须能和 <typeparamref name="TEntity" /> 相互映射</typeparam>
    /// <typeparam name="TPrimaryKey">实体的主键</typeparam>
    public abstract class CrudAppService<TEntity, TEntityDto, TPrimaryKey> : CrudAppService<TEntity, TEntityDto, TPrimaryKey, TEntityDto, TEntityDto, PagedAndSortedGetInput>
        where TEntity : class, IEntity<TPrimaryKey>, ICreationAudited
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
        /// <summary>
        /// 构造方法，暂无特殊说明
        /// </summary>
        /// <param name="repository"></param>
        protected CrudAppService(IRepository<TEntity, TPrimaryKey> repository) : base(repository)
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
    /// <typeparam name="TGetInput">获取所有数据时的输入</typeparam>
    public abstract class CrudAppService<TEntity, TEntityDto, TPrimaryKey, TCreateInput, TUpdateInput> : CrudAppService<
        TEntity, TEntityDto, TPrimaryKey, TCreateInput, TUpdateInput, PagedAndSortedGetInput>
        where TEntity : class, IEntity<TPrimaryKey>, ICreationAudited
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TCreateInput : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {
        /// <inheritdoc />
        /// <summary>
        /// 构造方法，暂无特殊说明
        /// </summary>
        /// <param name="repository"></param>
        protected CrudAppService(IRepository<TEntity, TPrimaryKey> repository) : base(repository)
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
    /// <typeparam name="TGetInput">获取所有数据时的输入</typeparam>
    public abstract class CrudAppService<TEntity, TEntityDto, TPrimaryKey, TCreateInput, TUpdateInput, TGetInput> : ApplicationService, ICrudAppService<TEntityDto, TPrimaryKey, TCreateInput, TUpdateInput, TGetInput>
        where TEntity : class, IEntity<TPrimaryKey>, ICreationAudited
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TCreateInput : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {
        private readonly IRepository<TEntity, TPrimaryKey> _repository;
        /// <summary>
        /// 用户没有权限进行当前操作时返回的错误代码
        /// </summary>
        protected int NoPermissionErrorCode { get; set; } = -1;
        /// <summary>
        /// 创建实体的权限名称
        /// </summary>
        protected string CreatePermissionName { get; set; }
        /// <summary>
        /// 更新实体的权限名称
        /// </summary>
        protected string UpdatePermissionName { get; set; }
        /// <summary>
        /// 删除实体的权限名称
        /// </summary>
        protected string DeletePermissionName { get; set; }
        /// <summary>
        /// 获取实体的权限名称
        /// </summary>
        protected string GetPermissionName { get; set; }
        /// <summary>
        /// 批量获取实体的权限名称
        /// </summary>
        protected string GetAllPermissionName { get; set; }

        /// <summary>
        /// 检查当前用户是否有创建实体的权限
        /// </summary>
        /// <returns></returns>
        protected bool CheckCreatePermission() { return CheckPermission(CreatePermissionName); }
        /// <summary>
        /// 检查当前用户是否有更新实体的权限
        /// </summary>
        /// <returns></returns>
        protected bool CheckUpdatePermission() { return CheckPermission(UpdatePermissionName); }
        /// <summary>
        /// 检查当前用户是否有删除实体的权限
        /// </summary>
        /// <returns></returns>
        protected bool CheckDeletePermission() { return CheckPermission(DeletePermissionName); }

        /// <summary>
        /// 检查当前用户是否有获取实体信息的权限
        /// </summary>
        /// <returns></returns>
        protected bool CheckGetPermission() { return CheckPermission(GetPermissionName); }

        /// <summary>
        /// 检查当前用户是否有批量获取实体的权限
        /// </summary>
        /// <returns></returns>
        protected bool CheckGetAllPermission() { return CheckPermission(GetAllPermissionName); }

        /// <summary>
        /// 检查当前用户是否有指定的权限
        /// </summary>
        /// <param name="permissionName"></param>
        /// <returns></returns>
        protected virtual bool CheckPermission(string permissionName)
        {
            if (permissionName.IsNullOrWhiteSpace())
            {
                return true;
            }
            var task = PermissionChecker.IsGrantedAsync(permissionName);
            task.Wait();

            return task.Result;
        }

        /// <inheritdoc />
        /// <summary>
        /// 构造方法，暂无特殊说明
        /// </summary>
        /// <param name="repository"></param>
        protected CrudAppService(IRepository<TEntity, TPrimaryKey> repository)
        {
            _repository = repository;
        }

        /// <inheritdoc />
        /// <summary>
        /// 创建一个指定类型的对象并保存到数据库中
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ApiResult<TEntityDto> Create(TCreateInput input)
        {
            try
            {
                if (CheckCreatePermission() == false)
                {
                    return OperationDenied();
                }

                var entity = ObjectMapper.Map<TEntity>(input);
                var id = _repository.InsertAndGetId(entity);
                var dto = ObjectMapper.Map<TEntityDto>(entity);
                dto.Id = id;
                return new ApiResult<TEntityDto>(true, dto);
            }
            catch (Exception e)
            {
                Logger.Error("未处理的异常", e);
                return new ApiResult<TEntityDto>(false) { Msg = e.ToString() };
            }
        }

        /// <summary>
        /// 从系统中删除所有给定Id的记录
        /// </summary>
        /// <param name="input">需要删除的Id的数组</param>
        /// <returns></returns>
        [HttpDelete]
        public virtual ApiResult Delete(List<TPrimaryKey> input)
        {
            try
            {
                if (CheckDeletePermission() == false)
                {
                    return new ApiResult(false) { ErrorCode = NoPermissionErrorCode, Msg = "No permission to do this operation!" };
                }

                _repository.Delete(entity => input.Contains(entity.Id));
                return new ApiResult(true);
            }
            catch (Exception e)
            {
                Logger.Error("未处理的异常", e);
                return new ApiResult(false) { Msg = e.ToString() };
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 在系统中更新指定的记录（单个）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //TODO：警告：这里的Update和RestFul的Put请求规范并不完全匹配
        [HttpPut]
        public virtual ApiResult<TEntityDto> Update(TUpdateInput input)
        {
            try
            {
                if (CheckUpdatePermission() == false)
                {
                    return OperationDenied();
                }
                var entity = ObjectMapper.Map<TEntity>(input);
                entity = _repository.Update(entity);
                var dto = ObjectMapper.Map<TEntityDto>(entity);
                return new ApiResult<TEntityDto>(true, dto);
            }
            catch (Exception e)
            {
                Logger.Error("未处理的异常", e);
                return new ApiResult<TEntityDto>(false) { Msg = e.ToString() };
            }
        }

        /// <summary>
        /// 由于权限问题,请求已被拒绝!
        /// </summary>
        /// <returns></returns>
        protected virtual ApiResult<TEntityDto> OperationDenied()
        {
            return new ApiResult<TEntityDto>(false, msg: "No permission to do this operation!") { ErrorCode = NoPermissionErrorCode };
        }

        /// <inheritdoc />
        /// <summary>
        /// 从系统中读取满足给定条件的所有的数据记录，有两种工作，一种是
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual PagedApiResult<List<TEntityDto>> GetAll(TGetInput input)
        {
            try
            {

                var query = CreateFilterQuery(input);

                //如果没有查看其他人创建的记录权限,则只能查看到自己创建的记录
                if (CheckGetAllPermission() == false)
                {
                    query = query.Where(entity => entity.CreatorUserId == AbpSession.UserId);
                }

                var totalCount = query.Count();
                var paging = input as IPagedAndSortedGetInput;
                if (paging.IsNotNull())
                {
                    query = Get_Paging(query, paging);
                }

                var aquery = query.ToList();
                var result = aquery.Select(entity => ObjectMapper.Map<TEntityDto>(entity));
                var list = result.ToList();
                return PagedApiResult.Succeed(totalCount, list);
            }
            catch (Exception e)
            {
                Logger.Error("未处理的异常", e);
                return PagedApiResult.Failed<List<TEntityDto>>(e.ToString());
            }
        }

        /// <summary>
        /// 获取指定Id的数据记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ApiResult<TEntityDto> Get(TPrimaryKey id)
        {
            if (CheckGetPermission() == false)
            {
                return new ApiResult<TEntityDto>() { ErrorCode = NoPermissionErrorCode, Msg = "No permission to do this operation!" };
            }
            var entity = _repository.Get(id);
            if (entity.IsNull())
            {
                return ApiResult.Succeed<TEntityDto>();
            }

            var dto = ObjectMapper.Map<TEntityDto>(entity);
            return ApiResult.Succeed(dto);
        }

        /// <summary>
        /// 获取指定Id的数据记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ApiResult<List<TEntityDto>> GetByIds(List<TPrimaryKey> ids)
        {
            if (CheckGetPermission() == false)
            {
                return new ApiResult<List<TEntityDto>>() { ErrorCode = NoPermissionErrorCode, Msg = "No permission to do this operation!" };
            }

            var query = _repository.GetAllIncluding(entity => ids.Contains(entity.Id));
            var result = query.Select(entity => ObjectMapper.Map<TEntityDto>(entity)).ToList();
            return ApiResult.Succeed(result);
        }

        /// <summary>
        /// 根据输入的条件，筛选数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> CreateFilterQuery(TGetInput input)
        {
            return _repository.GetAllIncluding();
        }
        /// <summary>
        /// 查询数据中的部分功能：对查询到的结果进行分页处理
        /// </summary>
        /// <param name="result">需要分页的数据</param>
        /// <param name="paging">分页参数</param>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> Get_Paging(IQueryable<TEntity> result, IPagedAndSortedGetInput paging)
        {
            if (result.IsNull() || paging.IsNull())
            {
                return result;
            }

            if (paging.OrderBy.IsNotNullOrWhiteSpace())
            {
                result = System.Linq.Dynamic.DynamicQueryable.OrderBy(result, $"{paging.OrderBy} {(paging.Asc ? "ASC" : "DESC")}");
            }
            else
            {
                result = paging.Asc ? result.OrderBy(entity => entity.Id) : result.OrderByDescending(entity => entity.Id);
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
    }
}
