using System.Collections.Generic;
using ProtoBuf;

namespace GTAServer.ServerPersistance
{
    [ProtoContract]
    public class Server
    {
        /// <summary>
        /// Server name
        /// </summary>
        [ProtoMember(1)] public string Name { get; set; }
        /// <summary>
        /// Server max players
        /// </summary>
        [ProtoMember(2)] public int MaxPlayers { get; set; }
        /// <summary>
        /// Server port
        /// </summary>
        [ProtoMember(3)] public int Port { get; set; }
        /// <summary>
        /// If the server is password protected
        /// </summary>
        [ProtoMember(4)] public bool PasswordProtected { get; set; }
        /// <summary>
        /// Server password
        /// </summary>
        [ProtoMember(5)] public string Password { get; set; }
        /// <summary>
        /// If the server should announce itself
        /// </summary>
        [ProtoMember(6)] public bool Announce { get; set; }
        /// <summary>
        /// Master server address
        /// </summary>
        [ProtoMember(7)] public string MasterServer { get; set; }

        /// <summary>
        /// If the server should allow display names
        /// </summary>
        [ProtoMember(8)] public bool AllowDisplayNames { get; set; }
        /// <summary>
        /// If the server should allow outdated clients
        /// </summary>
        [ProtoMember(9)] public bool AllowOutdatedClients { get; set; }

        /// <summary>
        /// Server gamemode
        /// </summary>
        [ProtoMember(10)] public string Gamemode { get; set; }
        /// <summary>
        /// Filterscripts to load
        /// </summary>
        [ProtoMember(11)] public string[] Filterscripts { get; set; }


        [ProtoMember(12)] public List<Ban> Bans;
        [ProtoMember(13)] public Dictionary<string, User> Users;
        [ProtoMember(14)] public Dictionary<string, Group> Groups;
    }
}
