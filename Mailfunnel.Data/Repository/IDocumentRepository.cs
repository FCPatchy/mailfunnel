using System;
using System.Collections.Generic;
using System.Data.Unqlite;
using System.Linq.Expressions;

namespace Mailfunnel.Data.Repository
{
    public interface IDocumentRepository<T> : IDisposable where T : IDocumentEntity
    {
        UnqliteDB UnqliteDb { get; }
        string Collection { get; }
        void Open(string fileName, Unqlite_Open mode);
        T Add(T entity);
        T Get(long id);
        IEnumerable<T> GetAll();
        void Ensure(T entity, Expression<Func<T, bool>> func);
    }
}