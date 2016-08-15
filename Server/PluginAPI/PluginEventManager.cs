using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using GTAServer.ServerInstance;

namespace GTAServer.PluginAPI
{
    public class PluginEventManager
    {
        public string PluginName;

        public delegate void IncomingConnectionHandler(object sender, IncomingConnectionEventArgs e);

        public delegate void PlayerConnectionHandler(object sender, PlayerConnectionEventArgs e);

        public delegate void ConnectionRefusedHandler(object sender, ConnectionRefusedEventArgs e);

        public delegate void PlayerDisconnectHandler(object sender, PlayerDisconnectEventArgs e);

        public delegate void PlayerSpawnedHandler(object sender, PlayerSpawnedEventArgs e);

        public delegate void ChatHandler(object sender, ChatEventArgs e);

        public event IncomingConnectionHandler OnIncomingConnection;
        public event PlayerConnectionHandler OnPlayerConnect;
        public event ConnectionRefusedHandler OnConnectionRefused;
        public event PlayerDisconnectHandler OnPlayerDisconnect;
        public event PlayerSpawnedHandler OnPlayerSpawned;
        public event ChatHandler OnChatMessage;

        public PluginEventManager(string pluginName)
        {
            PluginName = pluginName;
        }

    }

}
