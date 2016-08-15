using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lidgren.Network;
using ProtoBuf;

namespace ServerRemotingClient
{
    public static class RemoteCommandSender
    {
        public static byte[] SerializeBinary(object data)
        {
            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, data);
                return stream.ToArray();
            }
        }
        public static object DeserializeBinary<T>(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                return Serializer.Deserialize<T>(stream);
            }
        }
        public static RemotingResponse SendCommand(string host, int port, string user, string pass, string command, List<string> args)
        { 
            var config = new NetPeerConfiguration("GTAServerRcon");
            var client = new NetClient(config);
            client.Start();
            client.Connect(host, port);
            var data = SerializeBinary(new RemotingPacket(user, pass, command, args));
            var message = client.CreateMessage();
            message.Write(data.Length);
            message.Write(data);
            client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
            while (true)
            {
                NetIncomingMessage msg;
                while ((msg = client.ReadMessage()) != null)
                {
                    if (msg.MessageType != NetIncomingMessageType.Data) continue;
                    var len = msg.ReadInt32();
                    var recvData = (RemotingResponse) DeserializeBinary<RemotingResponse>(msg.ReadBytes(len));
                    return recvData;
                }
            }
        }
    }
}
