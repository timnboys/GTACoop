using System.Collections.Generic;
using System.Linq;
using ProtoBuf;

namespace GTAServer.ServerPersistance
{
    [ProtoContract]
    public class Group
    {
        [ProtoMember(1)] public string Name;
        [ProtoMember(2)] public List<string> Permissions;

        [ProtoMember(3)] public string ChatPrefix;
        [ProtoMember(4)] public string ChatSuffix;

        [ProtoMember(5)] public int GroupRank;

        public void UpdateUsers(Server groupServer)
        {
            var usersToUpdate = groupServer.Users.Where(u => u.Value.Groups.Contains(Name));
            foreach (var user in usersToUpdate) user.Value.UpdateGroups(groupServer);
        }
    }
}
