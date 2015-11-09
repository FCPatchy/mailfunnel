using System;
using System.Data.Unqlite;

namespace Mailfunnel.Data.Repository
{
    public interface IDataRepository<T, in TKey> : IDisposable where T : IDataEntity<TKey>
    {
        void Open(string fileName, Unqlite_Open mode);
        T Add(T entity);
    }
}