using System;

namespace MidiMapper
{
    public class Zone
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public int LowerNote { get; set; }
        public int HigherNote { get; set; }

        /// <summary>
        /// null for Omni
        /// </summary>
        public int? InputChannel { get; set; }

        public int Transpose { get; set; }


    }
}
