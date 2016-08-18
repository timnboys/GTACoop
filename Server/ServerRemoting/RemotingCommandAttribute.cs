using System;

namespace GTAServer.ServerRemoting
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RemotingCommandAttribute : System.Attribute
    {
        public string Name;
        public string Desc;

        public RemotingCommandAttribute(string name, string desc)
        {
            Name = name;
            Desc = name;
        }
    }
}
