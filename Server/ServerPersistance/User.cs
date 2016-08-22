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

        public List<Group> Groups;
    }
}
