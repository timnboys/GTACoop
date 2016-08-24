using System;
using ProtoBuf;

namespace GTAServer.ServerPersistance
{
    [ProtoContract]
    public class Ban
    {
        [ProtoMember(1)] public string BanIp;
        [ProtoMember(2)] public string BanScName;
        [ProtoMember(3)] public string BanAccount;

        [ProtoMember(4)] public DateTime BanCreated;
        [ProtoMember(5)] public DateTime BanExpire;
    }
}
