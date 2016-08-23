using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAServer.ServerPersistance
{
    public class Server
    {
        /// <summary>
        /// Server name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Server max players
        /// </summary>
        public int MaxPlayers { get; set; }
        /// <summary>
        /// Server port
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// If the server is password protected
        /// </summary>
        public bool PasswordProtected { get; set; }
        /// <summary>
        /// Server password
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// If the server should announce itself
        /// </summary>
        public bool Announce { get; set; }
        /// <summary>
        /// Master server address
        /// </summary>
        public string MasterServer { get; set; }

        /// <summary>
        /// If the server should allow display names
        /// </summary>
        public bool AllowDisplayNames { get; set; }
        /// <summary>
        /// If the server should allow outdated clients
        /// </summary>
        public bool AllowOutdatedClients { get; set; }

        /// <summary>
        /// Server gamemode
        /// </summary>
        public string Gamemode { get; set; }
        /// <summary>
        /// Filterscripts to load
        /// </summary>
        public string[] Filterscripts { get; set; }


        public List<Ban> Bans;
        public Dictionary<string, User> Users;
        public Dictionary<string, Group> Groups;
    }
}
