using ProtoBuf;

namespace GTAServer.ServerInstance
{
    /// <summary>
    /// Class containing data for each chat message.
    /// </summary>
    [ProtoContract]
    public class ChatData
    {
        /// <summary>
        /// Message ID
        /// </summary>
        [ProtoMember(1)]
        public long Id { get; set; }
        /// <summary>
        /// Message Sender
        /// </summary>
        [ProtoMember(2)]
        public string Sender { get; set; }
        /// <summary>
        /// Message Contents
        /// </summary>
        [ProtoMember(3)]
        public string Message { get; set; }
    }

    public static class ChatFormatting
    {
        public static readonly string Purple = "~p~";
        public static readonly string Pink = "~q~";
        public static readonly string Blue = "~b~";
        public static readonly string Darkblue = "~d~";
        public static readonly string Red = "~r~";
        public static readonly string Green = "~g~";
        public static readonly string White = "~w~";
        public static readonly string Orange = "~o~";
        public static readonly string Grey1 = "~c~";
        public static readonly string Grey2 = "~m~";
        public static readonly string Grey3 = "~t~";
        public static readonly string Black1 = "~l~";
        public static readonly string Black2 = " ~u~";
        public static readonly string Yellow = "~y~";

        public static readonly string Big = "~h~";
        public static readonly string Normal = "~n~";
        public static readonly string Unknown = "~s~";
    }
}