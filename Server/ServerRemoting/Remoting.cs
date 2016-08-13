using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public Remoting(int port)
        {
            NetPeerConfiguration config = new NetPeerConfiguration("GTAServerRcon") {Port = port};
            _server = new NetServer(config);
            Log = LogManager.GetLogger("Remoting");
        }

        public void Start()
        {
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
        /// <summary>
        /// Main loop for remoting server.
        /// </summary>
        public void MainLoop()
        {
            NetIncomingMessage msg;
            while (msg != null)
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
                        var data = (RemotingPacket) DeserializeBinary<RemotingPacket>(msg.ReadBytes(len));
                        Log.Info("Received a new RemotingPacket, command: " + data.Command);
                        break;
                    default:
                        Log.Warn("Unknown message received: " + msg.MessageType);
                        break;
                }
            }
        }
    }
}
