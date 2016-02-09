using System.Collections.Generic;
using Mailfunnel.Data.Infrastructure;
using Mailfunnel.Data.Models;

namespace Mailfunnel.Data.Repository
{
    public class GroupRepository : IGroupRepository
    {
        public IEnumerable<Group> GetAllGroups()
        {
            using (var conn = new DatabaseConnection())
            {
                return null;
            }
        }
    }
}
