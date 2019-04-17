using AutoMapper;
using ERBus.Entity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
namespace ERBus.Service.Service
{
    public class DetailInfoServiceBase<TEntity> : EntityServiceBase<TEntity>, IDetailInfoServiceBase<TEntity>
          where TEntity : DetailInfoEntity
    {
        public DetailInfoServiceBase(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            //Mapper.CreateMap<TEntity, TEntity>();
        }
        public virtual IDetailInfoServiceBase<TEntity> Include(Expression<Func<TEntity, object>> include)
        {
            Includes.Add(include);
            ((IQueryable<TEntity>)Repository.DbSet).Include(include);
            return this;
        }
        protected virtual Expression<Func<TEntity, bool>> GetIdFilter(string id)
        {
            return x => x.ID == id;
        }
        protected virtual Expression<Func<TEntity, bool>> GetKeyFilter(TEntity instance)
        {
            return x => x.ID == instance.ID;
        }

        public virtual TEntity Find(TEntity instance, bool notracking = false)
        {
            var result = (notracking ? Repository.DbSet.AsNoTracking() : Repository.DbSet)
                .FirstOrDefault(GetKeyFilter(instance));
            return result;
        }

        public virtual TEntity FindById(string id, bool notracking = false)
        {
            var result = (notracking ? Repository.DbSet.AsNoTracking() : Repository.DbSet)
                .FirstOrDefault(GetIdFilter(id));
            return result;
        }
        public virtual TEntity Insert(TEntity instance)
        {
            var exist = Find(instance, true);
            if (exist != null)
            {
                throw new Exception("Tồn tại bản ghi có cùng mã!");
            }
            var newInstance = Mapper.DynamicMap<TEntity, TEntity>(instance);
            newInstance.ID = Guid.NewGuid().ToString();
            Repository.Insert(newInstance);
            return newInstance;
        }
        public virtual TEntity Update(TEntity instance,
            Action<TEntity, TEntity> updateAction = null,
            Func<TEntity, TEntity, bool> updateCondition = null)
        {
            var entity = Find(instance, false);
            if (entity == null || instance.ID != entity.ID)
            {
                throw new Exception("Bản ghi không tồn tại trong hệ thống");
            }
            var allowUpdate = updateCondition == null || updateCondition(
                instance, entity);
            if (allowUpdate)
            {
                if (updateAction == null)
                {
                    Mapper.DynamicMap(instance, entity);
                }
                else
                {
                    updateAction(instance, entity);
                }
                entity.ObjectState = ObjectState.Modified;
            }
            return entity;
        }

        public virtual TEntity Delete(string id)
        {
            var entity = FindById(id, false);
            if (entity == null)
            {
                throw new Exception("Bản ghi không tồn tại trong hệ thống");
            }
            entity.ObjectState = ObjectState.Deleted;
            return entity;
        }
    }
}
