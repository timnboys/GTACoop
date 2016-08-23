using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public List<Group> Groups;
        private List<string> _perms;
        private static Dictionary<string, Regex> _regexCache = new Dictionary<string, Regex>();
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


        public void BuildPerms()
        {
            _perms = new List<string>();
            foreach (var group in Groups)
            {
                foreach (var perm in group.Permissions)
                {
                    _perms.Add(perm);
                }
            }
        }
        public bool HasPerm(string neededPerm)
        {
            if (_perms == null) BuildPerms();

            foreach (var perm in _perms)
            {
                if (perm == "*") return true; // fast path, whee
                if (perm == neededPerm) return true; // ok, second fast path... this are slow from here

                var regexPattern = "^" + Regex.Escape(perm)
                                       .Replace(@"\*", ".*?")
                                       .Replace(@"\?", ".")
                                   + "$";
                if (!_regexCache.ContainsKey(regexPattern))
                {
                    _regexCache.Add(regexPattern, new Regex(regexPattern, RegexOptions.Compiled));
                }
                var regex = _regexCache[regexPattern];
                if (regex.Match(neededPerm).Success) return true;
            }
            return false;
        }

    }
}
