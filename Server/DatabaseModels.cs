using System;
using GTAServer;
using SQLite;

namespace GTAServer
{
    public enum BanTypes
    {
        /// <summary>
        /// Ban by IP address (Banned at connect)
        /// </summary>
        IP_BAN,
        /// <summary>
        /// Ban by social club name (Banned at connect, but can be spoofed.)
        /// </summary>
        SCNAME_BAN,
        /// <summary>
        /// Ban by account name (banned at login)
        /// </summary>
        ACCOUNT_BAN,
    }
    public class Servers
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public int MaxPlayers { get; set; }
        public int Port { get; set; }
        public bool PasswordProtected { get; set; }
        public string Password { get; set; }
        public bool Announce { get; set; }
        public string MasterServer { get; set; }
        public bool AllowDisplayNames { get; set; }
        public string Gamemode { get; set; }
        public string[] Filterscripts { get; set; }
        public string Handle { get; set; }
    }

    public class Accounts
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public Servers Server { get; set; }
        public int Name { get; set; }
        public int Password { get; set; }
    }

    public class Bans
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int BanType { get; set; }
        public string BannedUser { get; set; }
        public bool IsPermanent { get; set; }
        public DateTime ExpireTime { get; set; }
        public bool IsGlobal { get; set; }
        public Servers Server { get; set; }}
    }

    public class Groups
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string GroupName { get; set; }
        public bool Default { get; set; }
        public Servers Server { get; set; }
    }

    public class Permissions
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public Groups Group { get; set; }
        public string PermissionNode { get; set; }
        public Servers Server { get; set; }
    }


}
