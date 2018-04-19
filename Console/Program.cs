using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TEMIDI;

namespace MidiConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            TeVirtualMIDI.logging(TeVirtualMIDI.TE_VM_LOGGING_MISC | TeVirtualMIDI.TE_VM_LOGGING_RX | TeVirtualMIDI.TE_VM_LOGGING_TX);

            //Guid manufacturer = new Guid("aa4e075f-3504-4aab-9b06-9a4104a91cf0");
            //Guid product = new Guid("bb4e075f-3504-4aab-9b06-9a4104a91cf0");

            //CommandReader.Initialize(new TeVirtualMIDI("C# loopback", 65535, TeVirtualMIDI.TE_VM_FLAGS_PARSE_RX, ref manufacturer, ref product));

            //alernatively: simple instantiation without any "special" stuff:
            var port = new TeVirtualMIDI("eeee el pueeeerto");
            CommandReader.Initialize(port);

            Thread thread = new Thread(new ThreadStart(CommandReader.WorkThreadFunction));
            CommandReader.MessageReceived += CommandReader_MessageReceived;
            thread.Start();
            Console.WriteLine("Virtual port created - press enter to close port again");
            Console.ReadKey();
            //CommandReader.port.shutdown();
            CommandReader.ShutDown();
            Console.WriteLine("Virtual port closed - press enter to terminate application");
            Console.ReadKey();
        }

        private static void CommandReader_MessageReceived(object sender, MessageEventArgs e)
        {
            Console.WriteLine(e.Command);
        }
    }
}
