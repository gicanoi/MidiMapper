using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEMIDI
{
    [Serializable()]
    public class TeVirtualMIDIException : System.Exception
    {

        /* defines of specific WIN32-error-codes that the native teVirtualMIDI-driver
		 * is using to communicate specific problems to the application */
        private const int ERROR_PATH_NOT_FOUND = 3;
        private const int ERROR_INVALID_HANDLE = 6;
        private const int ERROR_TOO_MANY_CMDS = 56;
        private const int ERROR_TOO_MANY_SESS = 69;
        private const int ERROR_INVALID_NAME = 123;
        private const int ERROR_MOD_NOT_FOUND = 126;
        private const int ERROR_BAD_ARGUMENTS = 160;
        private const int ERROR_ALREADY_EXISTS = 183;
        private const int ERROR_OLD_WIN_VERSION = 1150;
        private const int ERROR_REVISION_MISMATCH = 1306;
        private const int ERROR_ALIAS_EXISTS = 1379;

        public TeVirtualMIDIException() : base()
        {
        }

        public TeVirtualMIDIException(string message) : base(message)
        {
        }

        public TeVirtualMIDIException(string message, System.Exception inner) : base(message, inner)
        {
        }

        protected TeVirtualMIDIException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
        }

        public int reasonCode
        {

            get
            {

                return this.fReasonCode;
            }

            set
            {

                this.fReasonCode = value;

            }

        }

        private int fReasonCode;

        private static string reasonCodeToString(int reasonCode)
        {

            switch (reasonCode)
            {

                case ERROR_OLD_WIN_VERSION:
                    return "Your Windows-version is too old for dynamic MIDI-port creation.";

                case ERROR_INVALID_NAME:
                    return "You need to specify at least 1 character as MIDI-portname!";

                case ERROR_ALREADY_EXISTS:
                    return "The name for the MIDI-port you specified is already in use!";

                case ERROR_ALIAS_EXISTS:
                    return "The name for the MIDI-port you specified is already in use!";

                case ERROR_PATH_NOT_FOUND:
                    return "Possibly the teVirtualMIDI-driver has not been installed!";

                case ERROR_MOD_NOT_FOUND:
                    return "The teVirtualMIDIxx.dll could not be loaded!";

                case ERROR_REVISION_MISMATCH:
                    return "The teVirtualMIDIxx.dll and teVirtualMIDI.sys driver differ in version!";

                case ERROR_TOO_MANY_SESS:
                    return "Maximum number of ports reached";

                case ERROR_INVALID_HANDLE:
                    return "Port not enabled";

                case ERROR_TOO_MANY_CMDS:
                    return "MIDI-command too large";

                case ERROR_BAD_ARGUMENTS:
                    return "Invalid flags specified";

                default:
                    return "Unspecified virtualMIDI-error: " + reasonCode;
            }
        }


        public static void ThrowExceptionForReasonCode(int reasonCode)
        {

            TeVirtualMIDIException exception = new TeVirtualMIDIException(reasonCodeToString(reasonCode));

            exception.reasonCode = reasonCode;

            throw exception;

        }

    }
}
