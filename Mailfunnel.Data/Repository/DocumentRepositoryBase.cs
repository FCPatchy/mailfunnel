using System.Collections.Generic;
using System.Data.Unqlite;
using Newtonsoft.Json;

namespace Mailfunnel.Data.Repository
{
    public abstract class DocumentRepositoryBase<T> : IDocumentRepository<T> where T : IDocumentEntity
    {
        protected DocumentRepositoryBase()
        {
            UnqliteDb = UnqliteDB.Create();
        }

        public UnqliteDB UnqliteDb { get; }
        public abstract string Collection { get; }

        public virtual T Add(T entity)
        {
            var returnEntity = default(T);

            var entityJson = JsonConvert.SerializeObject(entity);

            UnqliteDb.ExecuteJx9(
                string.Format(
                    "if(!db_exists('{0}')){{ $rc = db_create('{0}'); }}  $zRec = {1}; $rc = db_store('{0}', $zRec); print $zRec;",
                    Collection, entityJson),
                s => { returnEntity = JsonConvert.DeserializeObject<T>(s); });

            return returnEntity;
        }

        public virtual void Open(string fileName, Unqlite_Open mode)
        {
            UnqliteDb.Open(fileName, mode);
        }

        public virtual IEnumerable<T> GetAll()
        {
            IEnumerable<T> vals = null;

            UnqliteDb.ExecuteJx9(string.Format("print db_fetch_all('{0}');", Collection),
                s => { vals = JsonConvert.DeserializeObject<IEnumerable<T>>(s); });

            return vals;
        }

        #region IDisposable Support

        private bool disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    UnqliteDb.Close();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.

        // ~DataRepository() {

        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.

        //   Dispose(false);

        // }

        // This code added to correctly implement the disposable pattern.

        public virtual void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion
    }
}