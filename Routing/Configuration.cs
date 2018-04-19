using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiMapper.Routing
{
    public static class Cons
    {
        public static readonly byte INCREASE_SETUP_PC = 127;
        public static readonly byte DECREASE_SETUP_PC = 126;
        public const byte PROGRAM_CHANGE = 0xC;
        public const byte NOTE_ON = 0x9;
        public const byte NOTE_OFF = 0x8;
        public const byte CONTROL_CHANGE = 0xB;
    }
}
