using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAServer.PluginAPI
{
    public interface IPlugin
    {
        void Start(GameServer server);
    }
}
