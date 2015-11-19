using System;
using System.Collections.Generic;
using System.Data.Unqlite;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
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

            var success = UnqliteDb.ExecuteJx9(
                string.Format(
                    "if(!db_exists('{0}')){{ $rc = db_create('{0}'); }}  $zRec = {1}; $rc = db_store('{0}', $zRec); print $zRec;",
                    Collection, entityJson),
                s => { returnEntity = JsonConvert.DeserializeObject<T>(s); });

            if (success)
                UnqliteDb.Commit();

            return returnEntity;
        }


        public T Get(long id)
        {
            T val = default(T);

            UnqliteDb.ExecuteJx9(string.Format("print db_fetch_by_id('{0}', {1});", Collection, id), s =>
            {
                val = JsonConvert.DeserializeObject<T>(s);
            });

            return val;
        }

        public virtual void Open(string fileName, Unqlite_Open mode)
        {
            UnqliteDb.Open(fileName, mode);
        }

        public virtual IEnumerable<T> GetAll()
        {
            IEnumerable<T> vals = null;

            UnqliteDb.ExecuteJx9(string.Format("print db_fetch_all('{0}');", Collection), s =>
            {
                vals = JsonConvert.DeserializeObject<IEnumerable<T>>(s);
            });

            return vals;
        }

        public void Ensure(T entity, Expression<Func<T, bool>> func)
        {
            BinaryExpression eq = (BinaryExpression)func.Body;

            // Get the left side
            var left = (MemberExpression) eq.Left;
            var leftMember = left.Member;
            var leftName = leftMember.Name;

            MemberExpression right = (MemberExpression)eq.Right;
            MemberExpression rightExpression = (MemberExpression)right.Expression;
            ConstantExpression constExpression = (ConstantExpression)rightExpression.Expression;
            object obj = ((FieldInfo)rightExpression.Member).GetValue(constExpression.Value);
            object val = ((PropertyInfo)right.Member).GetValue(obj, null);
            string x;
            string y;
            string z;
            var getExpression = $"$zCallback = function($rec){{ if($rec.{leftName} == '{val}'){{ return TRUE; }}else{{ return FALSE; }} }}; $data = db_fetch_all('{Collection}', $zCallback); if(count($data) == 0){{ print NULL; }}else{{ print $data[0]; }}";
            if (!UnqliteDb.ExecuteJx9(getExpression, s =>
            {
                Console.WriteLine("Jx9 returned " + s);
                x = s;
                if (s == null)
                {
                    Console.WriteLine("s is null");
                    var insertExpression = $"$entity = {JsonConvert.SerializeObject(entity)}; db_store('{Collection}', $entity); print $entity;";
                    z = insertExpression;
                    if (!UnqliteDb.ExecuteJx9(insertExpression, u =>
                    {
                        Console.WriteLine("Jx9 returned " + u);
                        y = u;
                        entity = JsonConvert.DeserializeObject<T>(u);
                    }))
                    {
                     throw new Exception("Error running Jx9 expression");   
                    }
                }
                else
                {
                    entity = JsonConvert.DeserializeObject<T>(s);
                }
            }))
            {
                throw new Exception("Error running Jx9 expression");
            }
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