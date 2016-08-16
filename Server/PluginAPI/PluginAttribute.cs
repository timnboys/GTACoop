using System;

namespace GTAServer.PluginAPI
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PluginAttribute : System.Attribute
    {
        public string Name;
        public string Desc;

        public PluginAttribute(string name, string desc)
        {
            Name = name;
            Desc = desc;
        }
    }
}
