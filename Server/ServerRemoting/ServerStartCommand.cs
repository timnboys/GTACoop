using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTAServer.ServerInstance;

namespace GTAServer.ServerRemoting
{
    [RemotingCommand(name: "server.start", desc: "Start a server.")]
    public class ServerStartCommand : IRemoteCommand
    {
        public string HelpText => "Start a server. Requires one argument: server handle";

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
