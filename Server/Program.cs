using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Security.Policy;
using System.Threading;
using System.Xml.Serialization;
using GTAServer.ServerInstance;
using log4net;
using log4net.Config;
using SharpRaven;
using SharpRaven.Data;

namespace GTAServer 
{
    public static class Program
    {
        /// <summary>
        /// Location of the virtual server host
        /// </summary>
        public static string ServerHostLocation => AppDomain.CurrentDomain.BaseDirectory;
        /// <summary>
        /// Dictionary of all the virtual servers currently running
        /// </summary>
        public static Dictionary<string, AppDomain> VirtualServerDomains = new Dictionary<string, AppDomain>();
        /// <summary>
        /// Dictionary of all the virtual server handles.
        /// </summary>
        public static Dictionary<string, ObjectHandle> VirtualServerHandles = new Dictionary<string, ObjectHandle>();
        /// <summary>
        /// Dictionary of all the virtual servers
        /// </summary>
        public static Dictionary<string, GameServer> VirtualServers = new Dictionary<string, GameServer>();
        /// <summary>
        /// Dictionary of threads for the virtual servers.
        /// </summary>
        public static Dictionary<string, Thread> VirtualServerThreads = new Dictionary<string, Thread>();
        /// <summary>
        /// Server debug mode
        /// </summary>
        public static bool Debug = false;
        /// <summary>
        /// If the server should allow the config option to allow old clients
        /// </summary>
        public static bool AllowOutdatedClients = false;

        public static InstanceSettings Settings;
        /// <summary>
        /// Delete a file
        /// </summary>
        /// <param name="name">File to delete</param>
        /// <returns>If the file was deleted</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteFile(string name);

        public static readonly ILog Log = LogManager.GetLogger("ServerManager");
        /// <summary>
        /// Read server settings from XML
        /// </summary>
        /// <param name="path">Server settings path</param>
        /// <returns>ServerSettings instance</returns>
        private static InstanceSettings ReadSettings(string path)
        {
            var ser = new XmlSerializer(typeof(InstanceSettings));
            Log.Warn("WARNING! Debug build, sandboxing not enabled!");
            Log.Warn("It is advised to only run a single server instance!");
            if (File.Exists(path))
            {
                using (var stream = File.OpenRead(path)) Settings = (InstanceSettings)ser.Deserialize(stream);
                using (var stream = new FileStream(path, File.Exists(path) ? FileMode.Truncate : FileMode.Create, FileAccess.ReadWrite)) ser.Serialize(stream, Settings);
            }
            else
            {
                Log.Debug("No settings.xml found, creating a new one.");
                using (var stream = File.OpenWrite(path)) ser.Serialize(stream, Settings = new InstanceSettings());
            }
            //LogToConsole(1, false, "FILE", "Settings loaded from " + path);
            return Settings;
        }

        public static InstanceSettings GlobalSettings;
        /// <summary>
        /// Master server list
        /// </summary>
        public class MasterServerList
        {
            public List<string> List { get; set; }
        }

        public static void Main(string[] args)
        {
            try
            {
                XmlConfigurator.Configure(new System.IO.FileInfo("logging.xml"));
                Log.Debug("Loading settings");
                GlobalSettings =
                    ReadSettings(ServerHostLocation + ((args.Length > 0) ? args[0] : "Settings.xml"));
                var remoting = new ServerRemoting.Remoting(4490);
                remoting.Start();
                var remotingThread = new Thread(remoting.MainLoop);
                remotingThread.Start();

                foreach (var server in GlobalSettings.Servers)
                {
                    StartServer(server);
                }
                foreach (var thread in VirtualServerThreads)
                {
                    thread.Value.Join(100);
                    remotingThread.Join(10);
                }
            }
#if !DEBUG
            catch (Exception e)
            {
                SentryErrorSender(e);
                throw;
            }
#endif
        }

        private static void SentryErrorSender(Exception e)
        {
            if (GlobalSettings == null)
            {
                if (Log != null)
                {
                    Log.Fatal("Crash before settings were readable, reporting...");
                }
                else
                {
                    Console.WriteLine("Crash before logging was setup and settings were readable! Reporting...");
                }
            }
            else
            {
                if (!GlobalSettings.ReportErrors) return;
            }
            var ravenClient = new RavenClient("https://fb40b3620a5446a2a35eb8b835a67680:b5cbe6d46fb24decbe96957f54a7fa3b@sentry.nofla.me/2");
            var ravenEvent = new SentryEvent(e);
#if DEBUG
            ravenEvent.Tags.Add("DebugBuild", "true");
#else
            ravenEvent.Tags.Add("DebugBuild", "false");
#endif
            ravenClient.Capture(ravenEvent);
        }

        public static void StartServer(ServerSettings settings)
        {
            Log.Info("Creating new server instance: ");
            Log.Info("  - Handle: " + settings.Handle);
            Log.Info("  - Name: " + settings.Name);
            Log.Info("  - Player Limit: " + settings.MaxPlayers);
            if (settings.AllowOutdatedClients && !AllowOutdatedClients)
            {
                Log.Warn("Server config for " + settings.Handle + " is set to allow outdated clients, yet it has been disabled on the master server.");
                settings.AllowOutdatedClients = false;
            }

            VirtualServerDomains[settings.Handle]=AppDomain.CreateDomain(settings.Handle);
            var domain = VirtualServerDomains[settings.Handle];
#if DEBUG
            // Have to do this because ReSharper didn't pick up on the #if/#else/#endif...
            // ReSharper disable once UseObjectOrCollectionInitializer
            var curServer = new GameServer();
#else
            VirtualServerHandles[settings.Handle] = domain.CreateInstanceFrom(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath,
                "GTAServer.ServerInstance.GameServer");
            var handle = VirtualServerHandles[settings.Handle];
            VirtualServers[settings.Handle] = (GameServer)handle.Unwrap();
            var curServer = VirtualServers[settings.Handle];
#endif
            curServer.Name = settings.Name;
            curServer.MaxPlayers = settings.MaxPlayers;
            curServer.Port = settings.Port;
            curServer.PasswordProtected = settings.PasswordProtected;
            curServer.Password = settings.Password;
            curServer.AnnounceSelf = settings.Announce;
            curServer.MasterServer = settings.MasterServer;
            curServer.AllowNickNames = settings.AllowDisplayNames;
            curServer.AllowOutdatedClients = settings.AllowOutdatedClients;
            curServer.GamemodeName = settings.Gamemode;
            curServer.InternalName = settings.Handle;
            curServer.ConfigureServer();
            curServer.SetupLogger();
            curServer.LoadPlugin("TestPlugin");
            Log.Info("Finished configuring server: " + settings.Handle + ", starting.");
            VirtualServerThreads[settings.Handle] = new Thread(curServer.StartAndRunMainLoop);
            VirtualServerThreads[settings.Handle].Start();
            Log.Debug("Server started, injecting filterscripts into " + settings.Handle);
            foreach (var script in settings.Filterscripts)
            {
                if (string.IsNullOrEmpty(script) || string.IsNullOrWhiteSpace(script)) continue;
                Log.Info("Filterscript " + script + " loading into server instance " + settings.Handle);
                curServer.LoadFilterscript(script);
            }
            Log.Info("Server " + settings.Handle + " is now finished starting.");
        }

        public static void StopServer(string handle)
        {
            Log.Info("Stopping server " + handle);
            if (!VirtualServers.ContainsKey(handle)) return;
            VirtualServers[handle].Stop();
            VirtualServerThreads[handle].Abort();
#if DEBUG
            Log.Debug("Debug build, no appdomain to unload.");
#else
            AppDomain.Unload(VirtualServerDomains[handle]);
            VirtualServerDomains.Remove(handle);
#endif
            VirtualServerHandles.Remove(handle);
            VirtualServerThreads.Remove(handle);
            VirtualServers.Remove(handle);
            Log.Info("Server stopped: " + handle);
        }
    }
}