using System.Collections.Generic;
using Mailfunnel.Data.Entities;

namespace Mailfunnel.Web.Dto
{
    public class GroupsResponse
    {
        public IEnumerable<GroupEntity> Groups { get; set; } 
    }
}