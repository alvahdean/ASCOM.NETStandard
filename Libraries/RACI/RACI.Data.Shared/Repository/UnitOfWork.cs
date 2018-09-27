using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace RACI.Data
{
    public class UnitOfWork<TContext> : IDisposable
        where TContext : RaciModel, new()
    {
        private bool disposed = false;
        private bool _ignoreKeyCase = true;
        private object syncCache = new object();
        private Dictionary<Type, IGenericRepository> repoCache;
        private TContext context;

        protected IGenericRepository<TEntity> Repo<TEntity>()
            where TEntity : class
        {
            Type eType = typeof(TEntity);
            if (!repoCache.ContainsKey(eType))
            {
                lock (syncCache)
                {
                    if (!repoCache.ContainsKey(eType))
                    {
                        //TODO: Verify TContext supports TEntity???
                        repoCache.Add(eType, new GenericRepository<TContext, TEntity>(context) { IgnoreKeyCase = IgnoreKeyCase });
                    }
                }
            }
            return (IGenericRepository<TEntity>)repoCache[eType];
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public UnitOfWork()
        {
            context = new TContext();
            repoCache = new Dictionary<Type, IGenericRepository>();
        }
        public bool IgnoreKeyCase
        {
            get => _ignoreKeyCase;
            set
            {
                _ignoreKeyCase = value;
                foreach (IGenericRepository repo in repoCache.Values)
                    repo.IgnoreKeyCase = IgnoreKeyCase;
            }
        }
        public void Save()
        {

            context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
    public class RaciUnitOfWork : UnitOfWork<RaciModel>
    {
        public IGenericRepository<ProfileNode> Nodes => Repo<ProfileNode>();// as IGenericRepository<IProfileNode>;
        public IGenericRepository<ProfileValue> Values => Repo<ProfileValue>();// as IGenericRepository<IProfileValue>;
        public IGenericRepository<RaciSystem> Systems => NodesOfType<RaciSystem>();// as IGenericRepository<RaciSystem>;
        public IGenericRepository<TNode> NodesOfType<TNode>() where TNode : class, IProfileNode, new() => Repo<TNode>();

    }
}
