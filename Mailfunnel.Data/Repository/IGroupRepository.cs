using System.Collections.Generic;
using Mailfunnel.Data.Models;

namespace Mailfunnel.Data.Repository
{
    public interface IGroupRepository
    {
        IEnumerable<Group> GetAllGroups();
    }
}
