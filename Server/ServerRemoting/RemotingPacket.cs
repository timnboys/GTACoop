using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace GTAServer.ServerRemoting
{
    /// <summary>
    /// Packet sent from client to run a remoting command
    /// </summary>
    [ProtoContract]
    class RemotingPacket
    {
        /// <summary>
        /// User to run command as
        /// </summary>
        [ProtoMember(1)] public string Username;
        /// <summary>
        /// Password for the user to run the command as
        /// </summary>
        [ProtoMember(2)] public string Password;
        /// <summary>
        /// Command to run
        /// </summary>
        [ProtoMember(3)] public string Command;
        /// <summary>
        /// Command arguments
        /// </summary>
        [ProtoMember(4)] public List<string> CommandArguments;

        public RemotingPacket(string username, string password, string command, List<string> commandArguments)
        {
            Username = username;
            Password = password;
            Command = command;
            CommandArguments = commandArguments;
        }

        public RemotingPacket()
        {
        }
    }

    /// <summary>
    /// Response from server to client
    /// </summary>
    [ProtoContract]
    class RemotingResponse
    {
        /// <summary>
        /// Command stauts
        /// </summary>
        [ProtoMember(1)] public bool status;
        /// <summary>
        /// Command status message
        /// </summary>
        [ProtoMember(2)] public string message;
    }

}
