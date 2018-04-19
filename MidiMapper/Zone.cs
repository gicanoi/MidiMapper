using System;

namespace MidiMapper.Model
{
    public class Zone
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public int ActiveOnProgramChange { get; set; }
        public int LowerNote { get; set; }
        public int HigherNote { get; set; }
        public int Transpose { get; set; }

        public int OutputChannel { get; set; }
        public int OutputDevice { get; set; }
        
    }
}
