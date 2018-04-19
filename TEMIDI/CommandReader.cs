using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEMIDI
{
    public class MessageEventArgs : EventArgs
    {
        public byte[] RawCommand { get; set; }
        public string Command { get; set; }
        public MessageEventArgs(byte[] command)
        {
            RawCommand = command;
            Command = CommandReader.byteArrayToString(command);
        }
    }


    public class CommandReader
    {
        public static void Initialize(TeVirtualMIDI port)
        {
            CommandReader.port = port;
        }

        private static TeVirtualMIDI port;
        public static event EventHandler<MessageEventArgs> MessageReceived;

        public static void WorkThreadFunction()
        {
            byte[] command;
            EventHandler<MessageEventArgs> handler = MessageReceived;
            try
            {
                while (true)
                {
                    command = port.getCommand();

                    if (handler != null)
                    {
                        handler(null, new MessageEventArgs(command));
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO: Remove console logging
                Console.WriteLine("thread aborting: " + ex.Message);
            }

        }

        public static void ShutDown()
        {
            port.shutdown();
        }

        public static string byteArrayToString(byte[] ba)
        {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", ":");
        }
    }
}
