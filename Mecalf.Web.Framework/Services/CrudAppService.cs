using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Repositories;
using Abp.Logging;
using Abp.UI;
using Mecalf.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

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
    public abstract class CrudAppService<TEntity, TEntityDto, TPrimaryKey> : CrudAppService<TEntity, TEntityDto, TPrimaryKey, TEntityDto, TEntityDto, PagedAndSortedResultRequestDto>
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
        TEntity, TEntityDto, TPrimaryKey, TCreateInput, TUpdateInput, PagedAndSortedResultRequestDto>
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
        /// <summary>
        /// 存放实体的仓储
        /// </summary>
        protected readonly IRepository<TEntity, TPrimaryKey> _repository;
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
        /// 管理权限的权限名称,只有拥有这项权限的用户能修改其他用户的数据
        /// </summary>
        protected string ManagePermissionName { get; set; }

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
        /// 检查当前用户是否有管理所有实体的权限
        /// </summary>
        /// <returns></returns>
        protected bool CheckManagePermission() { return CheckPermission(ManagePermissionName); }

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

        ///// <summary>
        ///// 获取数据，这里是所有请求都需要有的基础过滤。
        ///// </summary>
        ///// <param name="query"></param>
        ///// <returns></returns>
        //protected virtual IQueryable<TEntity> GetAll(IQueryable<TEntity> query)
        //{
        //    return query;
        //}

        /// <summary>
        /// 将需要的导航的属性包含进来
        /// </summary>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> GetAllIncluding()
        {
            return _repository.GetAllIncluding();
        }

        /// <inheritdoc />
        /// <summary>
        /// 创建一个指定类型的对象并保存到数据库中
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual TEntityDto Create(TCreateInput input)
        {
            try
            {
                if (CheckCreatePermission() == false)
                {
                    throw OperationDenied();
                }

                var entity = ObjectMapper.Map<TEntity>(input);
                var id = _repository.InsertAndGetId(entity);
                var dto = ObjectMapper.Map<TEntityDto>(entity);
                dto.Id = id;
                return dto;
            }
            catch (Exception e)
            {
                throw UnhandledException(e);
            }
        }

        /// <summary>
        /// 从系统中删除所有给定Id的记录
        /// </summary>
        /// <param name="input">需要删除的Id的数组</param>
        /// <returns></returns>
        [HttpDelete]
        public virtual object Delete(List<TPrimaryKey> input)
        {
            try
            {
                if (CheckDeletePermission() == false)
                {
                    throw OperationDenied();
                }

                if (CheckManagePermission())
                {
                    _repository.Delete(entity => input.Contains(entity.Id));
                }
                else
                {
                    _repository.Delete(entity => input.Contains(entity.Id) && entity.CreatorUserId == AbpSession.UserId);
                }

                return "Success";
            }
            catch (Exception e)
            {
                throw UnhandledException(e);
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
        public virtual TEntityDto Update(TUpdateInput input)
        {
            try
            {
                if (CheckUpdatePermission() == false)
                {
                    throw OperationDenied();
                }
                IQueryable<TEntity> query = GetAllIncluding();

                if (CheckManagePermission() == false)
                {
                    OperationDenied();
                    query = query.Where(ent => ent.CreatorUserId == AbpSession.UserId);
                }

                var entity = query.FirstOrDefault(entity1 => entity1.Id.Equals(input.Id));
                if (entity == null)
                {
                    throw NotFoundException(input.Id);
                }
                ObjectMapper.Map(input, entity);
                entity = _repository.Update(entity);
                var dto = ObjectMapper.Map<TEntityDto>(entity);
                return dto;
            }
            catch (Exception e)
            {
                throw UnhandledException(e);
            }
        }

        /// <summary>
        /// 由于权限问题,请求已被拒绝!
        /// </summary>
        /// <returns></returns>
        protected virtual UserFriendlyException OperationDenied()
        {
            return new UserFriendlyException(NoPermissionErrorCode, "No permission to do this operation!") { Severity = LogSeverity.Error };
        }

        /// <summary>
        /// 由于系统出现未处理的异常!
        /// </summary>
        /// <returns></returns>
        protected virtual UserFriendlyException UnhandledException(Exception e)
        {
            Logger.Error("未处理的异常", e);
            return new UserFriendlyException(-1, "未处理的异常！", e.ToString()) { Severity = LogSeverity.Fatal };
        }

        /// <summary>
        /// 找不到给定的记录,操作失败!
        /// </summary>
        /// <returns></returns>
        protected virtual UserFriendlyException NotFoundException(TPrimaryKey id)
        {
            return new UserFriendlyException(-2, $"找不到唯一Id为:{id}的记录,操作失败！") { Severity = LogSeverity.Warn };
        }

        /// <summary>
        /// 由于业务逻辑上不允许当前操作,操作失败!!
        /// </summary>
        /// <returns></returns>
        protected virtual UserFriendlyException Failed(string message, int id)
        {
            return new UserFriendlyException(id, message) { Severity = LogSeverity.Warn };
        }

        /// <inheritdoc />
        /// <summary>
        /// 从系统中读取满足给定条件的所有的数据记录，有两种工作，一种是
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual PagedResultDto<TEntityDto> GetAll(TGetInput input)
        {
            try
            {

                if (CheckGetAllPermission() == false)
                {
                    throw OperationDenied();
                }
                var query = CreateFilterQuery(input);

                //如果没有管理权限,则只能查看到自己创建的记录
                if (CheckManagePermission() == false)
                {
                    query = query.Where(ent => ent.CreatorUserId == AbpSession.UserId);
                }

                var totalCount = query.Count();
                var paging = input as PagedAndSortedResultRequestDto;
                if (paging.IsNotNull())
                {
                    query = Get_Paging(query, paging);
                }

                var result = query.ToList().Select(entity => ObjectMapper.Map<TEntityDto>(entity));
                var list = result.ToList();
                return new PagedResultDto<TEntityDto>(totalCount, list);
            }
            catch (Exception e)
            {
                throw UnhandledException(e);
            }
        }

        /// <summary>
        /// 获取指定Id的数据记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TEntityDto Get(TPrimaryKey id)
        {
            if (CheckGetPermission() == false)
            {
                throw OperationDenied();
            }

            var query = GetAllIncluding();
            if (CheckManagePermission() == false)
            {
                query = query.Where(ent => ent.CreatorUserId == AbpSession.UserId);
            }
            var entity = query.FirstOrDefault(entity1 => entity1.Id.Equals(id));
            if (entity.IsNull())
            {
                return default(TEntityDto);
            }

            var dto = ObjectMapper.Map<TEntityDto>(entity);
            return (dto);
        }

        /// <summary>
        /// 获取指定Id的数据记录
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ListResultDto<TEntityDto> GetByIds(List<TPrimaryKey> ids)
        {
            if (CheckGetPermission() == false)
            {
                throw OperationDenied();
            }
            var query = GetAllIncluding();
            if (CheckManagePermission() == false)
            {
                query = query.Where(ent => ent.CreatorUserId == AbpSession.UserId);
            }
            query = query.Where(entity => ids.Contains(entity.Id));
            var result = query.Select(entity => ObjectMapper.Map<TEntityDto>(entity)).ToList();
            return new ListResultDto<TEntityDto>(result);
        }

        /// <summary>
        /// 根据输入的条件，筛选数据,子类可以在需要的时候重写这个方法来对查询的数据进行过滤
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> CreateFilterQuery(TGetInput input)
        {
            return GetAllIncluding();
        }
        /// <summary>
        /// 查询数据中的部分功能：对查询到的结果进行分页处理
        /// </summary>
        /// <param name="result">需要分页的数据</param>
        /// <param name="paging">分页参数</param>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> Get_Paging(IQueryable<TEntity> result, PagedAndSortedResultRequestDto paging)
        {
            if (result.IsNull() || paging.IsNull())
            {
                return result;
            }

            result = paging.Sorting.IsNotNullOrWhiteSpace() ?
                System.Linq.Dynamic.DynamicQueryable.OrderBy(result, paging.Sorting)
                : result.OrderByDescending(entity => entity.Id);
            result = result.Skip(paging.SkipCount);
            result = result.Take(paging.MaxResultCount);
            return result;
        }
    }
}
