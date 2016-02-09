using System.Collections.Generic;
using Mailfunnel.Web.Dto;

namespace Mailfunnel.Web.Managers
{
    public interface IGroupManager
    {
        IEnumerable<Group> GetAllGroups();
    }
}
