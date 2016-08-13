using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NDesk.Options;

namespace ServerRemotingClient
{
    class Program
    {
        static void Main(string[] args)
        {
            bool showHelp = false;
            string username;
            string password;
            string command;
            var port = 4490;
            var arguments = new List<string>();
            var parser = new OptionSet()
            {
                {"u|username=", "The user to run the rcon command as", v => username=v },
                {"p|password=", "The password of the user running the command", v => password=v },
                {"c|command", "The command to run", v => command=v},
                {"a|arg", "An argument to pass to the command.", v => arguments.Add(v)},
                {"port", "Port to connect to server on", v => port = int.Parse(v)},
                {"h|help", "Show help and exit.", v => showHelp = (v != null)}
            };
            parser.Parse(args);
            if (showHelp)
            {
                ShowHelp();
            }
        }

        private static void ShowHelp()
        {
            Console.WriteLine("GTACoOp Server Remoting Client");
            Console.WriteLine("    -u   --username      The user to run the rcon command as.");
            Console.WriteLine("    -p   --password      The password of the user running the command");
            Console.WriteLine("    -c   --command       The command to run");
            Console.WriteLine("    -a   --arg           An argument to pass to the command. Can be used multiple times.");
            Console.WriteLine("         --port          Port to connect to the server with");
            Console.WriteLine("    -h   --help          Show this help and exit.");
            Console.WriteLine("");
            Console.WriteLine("Example: ServerRemotingClient.exe -u admin -p admin -c server.start -a default --port 5000");
            Environment.Exit(0);
        }
    }
}
