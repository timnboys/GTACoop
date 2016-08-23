using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAServer.ServerPersistance
{
    public class Group
    {
        public string Name;
        public List<string> Permissions;

        public string ChatPrefix;
        public string ChatSuffix;

        public int GroupRank;

        public void UpdateUsers(Server groupServer)
        {
            var usersToUpdate = groupServer.Users.Where(u => u.Value.Groups.Contains(Name));
            foreach (var user in usersToUpdate) user.Value.UpdateGroups(groupServer);
        }
    }
}
