using System.Collections.Generic;

namespace GTAServer.ServerInstance
{
    public static class ServerServices
    {
        public static Dictionary<string, object> Services = new Dictionary<string, object>();

        public static void DefineService(string name, object serviceObject)
        {
            if (Services.ContainsKey(name)) Services.Remove(name);
            Services.Add(name, serviceObject);
        }
    }
}
