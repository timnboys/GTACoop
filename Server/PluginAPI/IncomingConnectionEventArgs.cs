using System;
using GTAServer.ServerInstance;
namespace GTAServer.PluginAPI
{
    public class IncomingConnectionEventArgs : EventArgs
    {
        public Client Player;
    }
}