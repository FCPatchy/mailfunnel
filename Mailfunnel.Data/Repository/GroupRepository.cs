using System;
using System.Data.Unqlite;
using Mailfunnel.Data.Entities;

namespace Mailfunnel.Data.Repository
{
    public class GroupRepository : DocumentRepositoryBase<GroupEntity>
    {
        public GroupRepository()
        {
            if (!UnqliteDb.Open("Mailfunnel.db", Unqlite_Open.CREATE))
                throw new Exception("Unable to open database");
        }

        public override string Collection
        {
            get { return "Groups"; }
        }
    }
}
