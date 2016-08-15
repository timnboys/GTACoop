using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace ServerRemotingClient
{
    /*class Program
    {
        static void Main(string[] args)
        {
            bool showHelp = false;
            string username = null;
            string password = null;
            string command = null;
            string host = "192.168.0.17";
            var port = 4490;
            var arguments = new List<string>();
            var parser = new OptionSet()
            {
                {"u|username=", "The user to run the rcon command as", v => username=v },
                {"p|password=", "The password of the user running the command", v => password=v },
                {"c|command", "The command to run", v => command=v},
                {"a|arg", "An argument to pass to the command.", v => arguments.Add(v)},
                {"port", "Port to connect to server on", v => port = int.Parse(v)},
                {"host", "Host to connect to the server with.", v => host = v},
                {"h|help", "Show help and exit.", v => showHelp = (v != null)}
            };
            parser.Parse(args);
            if (showHelp)
            {
                ShowHelp();
            }
            if (username == null || password == null || command == null)
            {
                Console.WriteLine("You are missing either your username, password, or the command to run");
                Environment.Exit(1);
            }
            Console.WriteLine("Host: " + host);
            Console.WriteLine("Username: " + username);
            Console.WriteLine("Command: " + command);
            RemoteCommandSender.SendCommand(host, port, username, password, command, arguments);
        }

        private static void ShowHelp()
        {
            Console.WriteLine("GTACoOp Server Remoting Client");
            Console.WriteLine("    -u   --username      The user to run the rcon command as.");
            Console.WriteLine("    -p   --password      The password of the user running the command");
            Console.WriteLine("    -c   --command       The command to run");
            Console.WriteLine("    -a   --arg           An argument to pass to the command. Can be used multiple times.");
            Console.WriteLine("         --port          Port to connect to the server with. Default 4490.");
            Console.WriteLine("         --host          Host to connect to the server with. Default localhost.");
            Console.WriteLine("    -h   --help          Show this help and exit.");
            Console.WriteLine("");
            Console.WriteLine("Example: ServerRemotingClient.exe -u admin -p admin -c server.start -a default --port 5000");
            Environment.Exit(0);
        }
    }*/

    class Options
    {
        [Option('u', "username", Required = true,
            HelpText = "User to run the command as.")]
        public string User { get; set; }

        [Option('p', "password", Required = true,
            HelpText = "Password of the user running the command")]
        public string Password { get; set; }

        [Option('c', "command", Required = true,
            HelpText = "Command to run on the server")]
        public string Command { get; set; }

        [Option('o', "port", Required = false, DefaultValue = 4490,
            HelpText = "Port of the server to connect to")]
        public int Port { get; set; }

        [Option('g', "host", Required = false, DefaultValue = "127.0.0.1",
            HelpText = "Host of the server to connect to")]
        public string Host { get; set; }

        [ValueList(typeof(List<string>))]
        public IList<string> ArgList { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var help=HelpText.AutoBuild(this,
                (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
            help.Heading = "ServerRemotingClient - A tool for administrating GTACoOp servers";
            help.Copyright = "Copyright wolfmitchell/Bluscream 2016";
            help.AdditionalNewLineAfterOption = false;
            help.AddPostOptionsLine("Example usage: ");
            help.AddPostOptionsLine("    ServerRemotingClient.exe -g 127.0.0.1 -g 4490 -u admin -p password -c server.start default");
            help.MaximumDisplayWidth = 1200;
            return help;
        }
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            var options = new Options();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                var remoteReply = RemoteCommandSender.SendCommand(options.Host, options.Port, options.User, options.Password, options.Command, (options.ArgList as List<string>));
                Console.WriteLine("Status: " + remoteReply.status);
                Console.WriteLine("Message: " + remoteReply.message);
            }
        }
    }
}
