using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAServer.ServerPersistance
{
    public class Ban
    {
        public string BanIp;
        public string BanScName;
        public string BanAccount;

        public DateTime BanCreated;
        public DateTime BanExpire;
    }
}
