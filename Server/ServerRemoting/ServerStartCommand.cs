using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTAServer.ServerInstance;

namespace GTAServer.ServerRemoting
{
    class ServerStartCommand : IRemoteCommand
    {
        public string HelpText => "Start a server. Requires one argument: server name";

        public RemotingResponse OnCommandRun(List<string> args)
        {
            var servers = new List<ServerSettings>(Program.GlobalSettings.Servers);
            var server = servers.Find(s => s.Handle == args[0]);
            if (server == null) return new RemotingResponse() {message = "Server not found", status = false};
            Program.StartServer(server);
            return new RemotingResponse() {message = "Server started", status = true};
        }
    }
}
