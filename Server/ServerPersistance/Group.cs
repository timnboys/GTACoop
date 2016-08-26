using System.Collections.Generic;
using System.Linq;
using ProtoBuf;

namespace GTAServer.ServerPersistance
{
    [ProtoContract]
    public class Group
    {
        [ProtoMember(1)] public string Name;
        [ProtoMember(2)] public List<string> Permissions = new List<string>();

        [ProtoMember(3)] public string ChatPrefix;
        [ProtoMember(4)] public string ChatSuffix;

        [ProtoMember(5)] public int GroupRank;

        [ProtoMember(6)] public bool IsDefault;

        public void UpdateUsers(Server groupServer)
        {
            var usersToUpdate = groupServer.Users.Where(u => u.Value.Groups.Contains(Name));
            foreach (var user in usersToUpdate) user.Value.UpdateGroups(groupServer);
        }

        public Group(string name, string chatPrefix, string chatSuffix, int groupRank, bool isDefault)
        {
            Name = name;
            ChatPrefix = chatPrefix;
            ChatSuffix = chatSuffix;
            GroupRank = groupRank;
            IsDefault = isDefault;
        }
    }
}
