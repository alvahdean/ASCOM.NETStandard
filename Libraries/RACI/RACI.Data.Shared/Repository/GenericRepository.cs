using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace RACI.Data
{
    public class GenericRepository<TContext,TEntity> : IGenericRepository<TEntity>
        where TContext : DbContext,new()
        where TEntity : class
    {
        internal TContext context;
        internal DbSet<TEntity> dbSet;
        private bool _ignoreCase;

        /// <summary>
        /// Create a new instance using a new default instance of TContext 
        /// </summary>
        public GenericRepository() : this(new TContext()) { }

        /// <summary>
        /// Creates a new instance using the specified TContext
        /// </summary>
        /// <param name="context">The context which will be used to operate on the underlying data</param>
        public GenericRepository(TContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        /// <summary>
        /// Gets or sets whether string keys will be compared case-insensitively
        /// </summary>
        /// <remarks>Modifying this value will change the KeyComparer used</remarks>
        public bool IgnoreKeyCase
        {
            get => _ignoreCase;
            set
            {
                _ignoreCase = value;
                KeyComparer = _ignoreCase 
                    ? (IEqualityComparer<String>)new CIKeyComparer() 
                    : new CSKeyComparer();
            }
        }

        /// <summary>
        /// Returns the IEqualityComparer<String> used to comparer string keys
        /// </summary>
        public IEqualityComparer<String> KeyComparer { get; private set; }

        //public TContext Context { get => context; }
        public IQueryable<TEntity> All { get => dbSet; }
        /// <summary>
        /// Generic query method with options to filter, order and include related records
        /// </summary>
        /// <param name="filter">A bool expression for filtering results</param>
        /// <param name="orderBy">A function which specifies ordering of results</param>
        /// <param name="includeProperties">A comma separated list of related properties to include</param>
        /// <returns>An enumerable containing the results of the query</returns>
        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        /// <summary>
        /// Returns a single TEntity record for a single-keyed record type
        /// </summary>
        /// <param name="id">The value of the record key to query</param>
        /// <returns>A single entity matching the specified id or null if no record exists</returns>
        public virtual TEntity GetById(object id) => GetByKey(new object[] { id });

        /// <summary>
        /// Returns a single TEntity record for a single or multi keyed record type
        /// </summary>
        /// <param name="key">The values of the record key to query</param>
        /// <returns>A single entity matching the specified key or null if no record exists</returns>
        public virtual TEntity GetByKey(object[] key)
        {
            return dbSet.Find(key);
        }

        /// <summary>
        /// Adds a new record to the context and flags it for creation on Save
        /// </summary>
        /// <param name="entity">The entity to add</param>
        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        /// <summary>
        /// Flags the record matching the specified key for deletion on next Save
        /// </summary>
        /// <param name="id">The id of the record to delete</param>
        public virtual void Delete(object id) => Delete(new object[] { id });

        /// <summary>
        /// Flags the record matching the specified key for deletion on next Save
        /// </summary>
        /// <param name="key">The id of the record to delete</param>
        public virtual void Delete(object[] key)
        {
            TEntity entityToDelete = dbSet.Find(key);
            Delete(entityToDelete);
        }

        /// <summary>
        /// Flags the record matching the specified key for deletion on next Save
        /// </summary>
        /// <param name="key">The id of the record to delete</param>
        public virtual void Delete(TEntity entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }

        /// <summary>
        /// Flags the entity for update on next Save
        /// </summary>
        /// <param name="entity">The entity which has been modified</param>
        public virtual void Update(TEntity entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        IQueryable<object> IGenericRepository.All => All;

        object IGenericRepository.GetById(object id) => GetById(id);

        object IGenericRepository.GetByKey(object[] key) => GetByKey(key);

        public void Insert(object entity) => Insert(entity as TEntity);

        public void Update(object entity) => Update(entity as TEntity);
    }
}
