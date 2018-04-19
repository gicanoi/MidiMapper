using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiMapper.Routing
{
    public class CommandHelpers
    {
        public static byte ChangeChannel(byte command, int newChannel)
        {
            int cmdType = command & 0b1111_0000;
            int cmdChannel = command & 0b0000_1111;
            int cmdNewChannel = cmdType | (newChannel - 1);
            return (byte)cmdNewChannel;
        }

        public static byte GetCommandType(byte[] command)
        {
            return (byte)(command[0] & 0b1100_0000);
        }

    }
}
