using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RACI.Data
{
    public interface IGenericRepository
    {
        bool IgnoreKeyCase { get; set; }
        IEqualityComparer<string> KeyComparer { get; }

        IQueryable<object> All { get; }
        void Delete(object id);
        void Delete(object[] key);
        object GetById(object id);
        object GetByKey(object[] key);
        void Insert(object entity);
        void Update(object entity);
    }

    public interface IGenericRepository<TEntity>  : IGenericRepository
    where TEntity : class
    {
        void Delete(TEntity entity);
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");
        void Insert(TEntity entity);
        void Update(TEntity entity);

        new IQueryable<TEntity> All { get; }
        new TEntity GetById(object id);
        new TEntity GetByKey(object[] key);
    }
}