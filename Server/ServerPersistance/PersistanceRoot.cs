using System.Collections.Generic;
using System.IO;
using ProtoBuf;

namespace GTAServer.ServerPersistance
{
    [ProtoContract]
    public class PersistanceRoot
    {
        [ProtoMember(1)] public List<Ban> GlobalBans;
        [ProtoMember(2)] public List<Server> Servers;
        [ProtoMember(3)] public int DbVersion;

        /// <summary>
        /// Serialize an object into a byte array
        /// </summary>
        /// <param name="data">Object to serialize</param>
        /// <returns>What the data returns</returns>
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
                try
                {
                    return Serializer.Deserialize<T>(stream);
                }
                catch (ProtoException e)
                {
                    return null;
                }
            }
        }

        public void SaveDb(string fileName)
        {
            using (var stream = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                stream.Write(SerializeBinary(this), 0, int.MaxValue);
                stream.Close();
            }
        }

        public static byte[] ReadFullStream(Stream input)
        {
            using (var ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
        public static PersistanceRoot LoadDb(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return CreateDb(fileName);
            }
            using (var stream = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                return (PersistanceRoot) DeserializeBinary<PersistanceRoot>(ReadFullStream(stream));
            }
        }
        private static PersistanceRoot CreateDb(string fileName)
        {
            var db = new PersistanceRoot
            {
                GlobalBans = new List<Ban>(),
                Servers = new List<Server>
                {
                    new Server("GTACoop Server", 16, 4499, false, "", true, "https://gtamaster.nofla.me", true, false,
                        "freeroam", new string[] {})
                },
                DbVersion = 1
            };

            db.SaveDb(fileName);
            return db;
        }
    }
}
