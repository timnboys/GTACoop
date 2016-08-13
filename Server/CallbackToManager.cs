using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace GTAServer
{
    public class CallbackToManager : MarshalByRefObject
    {
        public ILog Log;
        public CallbackToManager()
        {
            Log = Program.Log;

        }
        public void StartHook(string serverName)
        {
            Log.Info("Server hooked into database code: " + serverName);
        }
    }
}
