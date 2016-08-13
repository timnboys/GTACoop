using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace GTAServer.ServerRemoting
{
    [ProtoContract]
    class RemotingPacket
    {
        [ProtoMember(1)] public string Username;
        [ProtoMember(2)] public string Password;
        [ProtoMember(3)] public string Command;
        [ProtoMember(4)] public List<string> CommandArguments;

        public RemotingPacket(string username, string password, string command, List<string> commandArguments)
        {
            Username = username;
            Password = password;
            Command = command;
            CommandArguments = commandArguments;
        }
    }
}
