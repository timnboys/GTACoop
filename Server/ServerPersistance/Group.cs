using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAServer.ServerPersistance
{
    public class Group
    {
        public string Name;
        public List<string> Permissions;

        public string ChatPrefix;
        public string ChatSuffix;

        public int GroupRank;
    }
}
