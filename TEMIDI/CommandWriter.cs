using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEMIDI
{
    public class CommandWriter
    {
        private TeVirtualMIDI[] _ports;
        private static CommandWriter _instance;
        private CommandWriter(TeVirtualMIDI[] ports)
        {
            _ports = ports;
        }

        public static CommandWriter Initialize(TeVirtualMIDI[] ports)
        {
            if(_instance == null)
            {
                _instance = new CommandWriter(ports);
                return _instance;
            }
            else
            {
                throw new InvalidOperationException("Command Writer is already initialized");
            }
        }

        public static CommandWriter GetInstance()
        {
            if (_instance == null)
                throw new InvalidOperationException("Command Writer must be initialized first");
            return _instance;
        }

        ~CommandWriter()
        {
            if(_ports != null && _ports.Length > 0)
            {
                foreach(var p in _ports)
                {
                    p.shutdown();
                }
            }
        }
    }
}
