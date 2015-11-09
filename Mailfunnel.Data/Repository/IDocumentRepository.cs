﻿using System;
using System.Data.Unqlite;

namespace Mailfunnel.Data.Repository
{
    public interface IDocumentRepository<T> : IDisposable where T : IDocumentEntity
    {
        UnqliteDB UnqliteDb { get; }
        string Collection { get; }
        void Open(string fileName, Unqlite_Open mode);
        T Add(T entity);
    }
}