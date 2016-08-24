using System.Collections.Generic;
using ProtoBuf;

namespace GTAServer.ServerPersistance
{
    [ProtoContract]
    public class PersistanceRoot
    {
        [ProtoMember(1)] public List<Ban> GlobalBans;
        [ProtoMember(2)] public List<Server> Servers;
        [ProtoMember(3)] public int DbVersion; 
    }
}
