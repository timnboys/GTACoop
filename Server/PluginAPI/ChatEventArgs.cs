using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTAServer.ServerInstance;

namespace GTAServer.PluginAPI
{
    public class ChatEventArgs : EventArgs
    {
        public ChatData ChatData;
    }
}
