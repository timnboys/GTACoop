using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAServer.ServerRemoting
{
    interface IRemoteCommand
    {
        string HelpText { get; }
        RemotingResponse OnCommandRun(List<string> args);
    }
}
