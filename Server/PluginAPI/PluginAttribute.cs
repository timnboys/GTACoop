using System;

namespace GTAServer.PluginAPI
{
    [AttributeUsage(AttributeTargets.Class)]
    class PluginAttribute : System.Attribute
    {
        private string _name;
        private string _desc;

        public PluginAttribute(string name, string desc)
        {
            _name = name;
            _desc = desc;
        }
    }
}
