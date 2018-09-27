using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RACI.Data
{
    public interface IRepository<TContext,TRecord> : IDisposable,IValidation<TRecord>
        where TContext: DbContext
        where TRecord: class
    {

        Type ModelType { get; }
        Type RecordType { get; }
        string ModelTypeName { get; }
        string RecordTypeName { get; }
        string RepositoryTypeName { get; }
        bool IgnoreKeyCase { get; set; }

        IQueryable<TRecord> All { get; }
        IEnumerable<TRecord> GetAll();
        bool ValidateOnSave { get; set; }

        bool IsKeySet(TRecord record);
        bool IsRecorded(TRecord record);
        bool IsDirty(TRecord record);
        bool IsPendingAdd(TRecord record);
        bool IsPendingDelete(TRecord record);
        bool IsAttached(TRecord record);

        object[] DbKey(TRecord rec);
        TRecord Get(object[] key);
        void Insert(TRecord rec);
        void Delete(object[] key);
        void Delete(TRecord rec);
        void Update(TRecord rec);
        void Save();
    }

}
