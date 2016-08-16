using System;
using GTAServer.PluginAPI;
using GTAServer.ServerInstance;

namespace TestPlugin
{
    [Plugin(name: "TestPlugin", desc: "Test plugin")]
    public class TestPlugin : IPlugin
    {
        public void Start(GameServer server, PluginEventManager eventManager) => Console.WriteLine("Test plugin loaded!");

        public void Stop() => Console.WriteLine("Test plugin unloaded!");
    }
}
