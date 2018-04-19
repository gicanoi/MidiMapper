using MidiMapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEMIDI;

namespace MidiMapper.Routing
{
    public class Router
    {
        public int ActiveSetup { get; set; }
        public int ActiveZone { get; set; }
        public SetupCollection SetupCollection { get; set; }

        private CommandReader _commandReader;
        private CommandWriter _commandWriter;

        private void IncreaseActiveSetup()
        {
            if (ActiveSetup < SetupCollection.Setups.Count)
            {
                ActiveSetup++;
            }
        }

        private void DecreaseActiveSetup()
        {
            if (ActiveSetup > 0)
            {
                ActiveSetup--;
            }
        }

        private void RouteCommand(byte[] command)
        {
            var commandType = CommandHelpers.GetCommandType(command);
            switch (commandType)
            {
                case Cons.PROGRAM_CHANGE:
                    if(command[1] == Cons.INCREASE_SETUP_PC)
                    {
                        IncreaseActiveSetup();
                    }
                    else
                    {
                        if(command[1] == Cons.DECREASE_SETUP_PC)
                        {
                            DecreaseActiveSetup();
                        }
                        else
                        {
                            ActiveZone = command[1];
                        }
                    }
                break;
                case Cons.NOTE_ON:

                break;
            }
        }

        public Router(CommandReader commandReader, CommandWriter commandWriter)
        {
            _commandReader = commandReader;
            _commandWriter = commandWriter;

            CommandReader.MessageReceived += CommandReader_MessageReceived;
        }

        private void CommandReader_MessageReceived(object sender, MessageEventArgs e)
        {
            RouteCommand(e.RawCommand);
        }

    }
}
