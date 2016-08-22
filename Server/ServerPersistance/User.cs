using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAServer.ServerPersistance
{
    public class User
    {
        public string Username;
        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = BCrypt.Net.BCrypt.HashPassword(value); }
        }

        public bool VerifyUserPassword(string passwordAttempt)
        {
            return BCrypt.Net.BCrypt.Verify(passwordAttempt, _password);
        }

        public void SortGroupsByRank()
        {
            Groups.Sort((g1, g2) => g1.GroupRank.CompareTo(g2.GroupRank));
        }
        public string GetChatPrefix()
        {
            return Groups[0]?.ChatPrefix ?? "";
        }

        public string GetChatSuffix()
        {
            return Groups[0]?.ChatSuffix ?? "";
        }

        public List<Group> Groups;
    }
}
