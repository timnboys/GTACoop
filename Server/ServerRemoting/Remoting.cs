using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTAServer.ServerInstance;
using log4net;
using log4net.Core;
using Lidgren.Network;
using ProtoBuf;

namespace GTAServer.ServerRemoting
{
    class Remoting
    {
        private ILog Log;
        private readonly NetServer _server;
        private readonly InstanceSettings _settings;
        public Dictionary<string, IRemoteCommand> Commands = new Dictionary<string, IRemoteCommand>();

        public Remoting(int port)
        {
            var config = new NetPeerConfiguration("GTAServerRcon") { Port = port };
            _server = new NetServer(config);
            _settings = Program.GlobalSettings;
            Log = LogManager.GetLogger("Remoting");
        }

        /// <summary>
        /// Start remoting server
        /// </summary>
        public void Start()
        {
            Commands.Add("server.start", new ServerStartCommand());
            Commands.Add("server.stop", new ServerStopCommand());
            _server.Start();
        }

        /// <summary>
        /// Deserialize a binary packet
        /// </summary>
        /// <typeparam name="T">Type expected from the packet</typeparam>
        /// <param name="data">Byte array of packet data</param>
        /// <returns>Deserialized object for packet</returns>
        public object DeserializeBinary<T>(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                try
                {
                    return Serializer.Deserialize<T>(stream);
                }
                catch (ProtoException e)
                {
                    Log.Warn("WARN: Deserialization failed: " + e.Message);
                    return null;
                }
            }
        }
        public byte[] SerializeBinary(object data)
        {
            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, data);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Main loop for remoting server.
        /// </summary>
        public void MainLoop()
        {
            while (true) WaitForMessage();
        }
        /// <summary>
        /// Run to check for a message.
        /// </summary>
        public void WaitForMessage()
        {
            NetIncomingMessage msg;
            while ((msg = _server.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                        Log.Debug("NetDebug: " + msg.ReadString());
                        break;
                    case NetIncomingMessageType.WarningMessage:
                        Log.Warn("NetWarn: " + msg.ReadString());
                        break;
                    case NetIncomingMessageType.ErrorMessage:
                        Log.Error("NetError: " + msg.ReadString());
                        break;
                    case NetIncomingMessageType.Data:
                        var len = msg.ReadInt32();
                        var data = (RemotingPacket)DeserializeBinary<RemotingPacket>(msg.ReadBytes(len));
                        RemotingResponse reply;
                        if (data.Username == _settings.RemoteUser && data.Password == _settings.RemotePassword)
                        {
                            if (!Commands.ContainsKey(data.Command))
                            {
                                reply = new RemotingResponse()
                                {
                                    message = "Command not found",
                                    status = false
                                };
                            }
                            else
                            {
                                reply = Commands[data.Command].OnCommandRun(data.CommandArguments);
                            }

                        }
                        else
                        {
                            Log.Warn("Unknown username/password on remoting socket.");
                            reply = new RemotingResponse()
                            {
                                message = "Bad username/password.",
                                status = false
                            };
                        }
                        var serreply = SerializeBinary(reply);
                        var message = _server.CreateMessage();
                        message.Write(serreply.Length);
                        message.Write(serreply);
                        _server.SendMessage(message, msg.SenderConnection, NetDeliveryMethod.ReliableOrdered);
                        break;
                    default:
                        Log.Warn("Unknown message received: " + msg.MessageType);
                        break;
                }
            }
        }
    }
}
