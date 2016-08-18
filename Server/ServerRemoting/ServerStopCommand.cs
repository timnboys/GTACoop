using System.Collections.Generic;

namespace GTAServer.ServerRemoting
{
    [RemotingCommand(name: "server.stop", desc: "Stops a server.")]
    public class ServerStopCommand : IRemoteCommand
    {
        public string HelpText => "Stop a server. Requires one argument: server handle";

        public RemotingResponse OnCommandRun(List<string> args)
        {
            var handle = args[0];
            if (!Program.VirtualServers.ContainsKey(handle))
                return new RemotingResponse() {message = "Server not found", status = true};

            Program.StopServer(handle);
            return new RemotingResponse() { message = "Server stopped", status = true};
        }
    }
}
