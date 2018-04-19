using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TEMIDI
{
    public class TeVirtualMIDI
    {
        /* default size of sysex-buffer */
        private const UInt32 TE_VM_DEFAULT_SYSEX_SIZE = 65535;

        /* constant for loading of teVirtualMIDI-interface-DLL, either 32 or 64 bit */
        private const string DllName = "teVirtualMIDI.dll";
        /* private const string DllName = "teVirtualMIDI32.dll"; */
        /* private const string DllName = "teVirtualMIDI64.dll"; */

        /* TE_VM_LOGGING_MISC - log internal stuff (port enable, disable...) */
        public const UInt32 TE_VM_LOGGING_MISC = 1;
        /* TE_VM_LOGGING_RX - log data received from the driver */
        public const UInt32 TE_VM_LOGGING_RX = 2;
        /* TE_VM_LOGGING_TX - log data sent to the driver */
        public const UInt32 TE_VM_LOGGING_TX = 4;

        /* TE_VM_FLAGS_PARSE_RX - parse incoming data into single, valid MIDI-commands */
        public const UInt32 TE_VM_FLAGS_PARSE_RX = 1;
        /* TE_VM_FLAGS_PARSE_TX - parse outgoing data into single, valid MIDI-commands */
        public const UInt32 TE_VM_FLAGS_PARSE_TX = 2;
        /* TE_VM_FLAGS_INSTANTIATE_RX_ONLY - Only the "midi-out" part of the port is created */
        public const UInt32 TE_VM_FLAGS_INSTANTIATE_RX_ONLY = 4;
        /* TE_VM_FLAGS_INSTANTIATE_TX_ONLY - Only the "midi-in" part of the port is created */
        public const UInt32 TE_VM_FLAGS_INSTANTIATE_TX_ONLY = 8;
        /* TE_VM_FLAGS_INSTANTIATE_BOTH - a bidirectional port is created */
        public const UInt32 TE_VM_FLAGS_INSTANTIATE_BOTH = 12;


        /* static initializer to retrieve version-info from DLL... */
        static TeVirtualMIDI()
        {

            fVersionString = Marshal.PtrToStringAuto(virtualMIDIGetVersion(ref fVersionMajor, ref fVersionMinor, ref fVersionRelease, ref fVersionBuild));
            fDriverVersionString = Marshal.PtrToStringAuto(virtualMIDIGetDriverVersion(ref fDriverVersionMajor, ref fDriverVersionMinor, ref fDriverVersionRelease, ref fDriverVersionBuild));

        }


        public TeVirtualMIDI(string portName, UInt32 maxSysexLength = TE_VM_DEFAULT_SYSEX_SIZE, UInt32 flags = TE_VM_FLAGS_PARSE_RX)
        {

            fInstance = virtualMIDICreatePortEx2(portName, IntPtr.Zero, IntPtr.Zero, maxSysexLength, flags);

            if (fInstance == IntPtr.Zero)
            {

                int lastError = Marshal.GetLastWin32Error();

                TeVirtualMIDIException.ThrowExceptionForReasonCode(lastError);

            }

            fReadBuffer = new byte[maxSysexLength];
            fReadProcessIds = new UInt64[17];
            fMaxSysexLength = maxSysexLength;

        }

        public TeVirtualMIDI(string portName, UInt32 maxSysexLength, UInt32 flags, ref Guid manufacturer, ref Guid product)
        {

            fInstance = virtualMIDICreatePortEx3(portName, IntPtr.Zero, IntPtr.Zero, maxSysexLength, flags, ref manufacturer, ref product);

            if (fInstance == IntPtr.Zero)
            {

                int lastError = Marshal.GetLastWin32Error();

                TeVirtualMIDIException.ThrowExceptionForReasonCode(lastError);

            }

            fReadBuffer = new byte[maxSysexLength];
            fReadProcessIds = new UInt64[17];
            fMaxSysexLength = maxSysexLength;

        }


        ~TeVirtualMIDI()
        {

            if (fInstance != IntPtr.Zero)
            {

                virtualMIDIClosePort(fInstance);

                fInstance = IntPtr.Zero;

            }
        }


        public static int versionMajor
        {

            get
            {

                return fVersionMajor;

            }

        }


        public static int versionMinor
        {

            get
            {

                return fVersionMinor;

            }

        }


        public static int versionRelease
        {

            get
            {

                return fVersionRelease;

            }

        }


        public static int versionBuild
        {

            get
            {

                return fVersionBuild;

            }

        }


        public static String versionString
        {

            get
            {

                return fVersionString;

            }

        }

        public static int driverVersionMajor
        {

            get
            {

                return fDriverVersionMajor;

            }

        }


        public static int driverVersionMinor
        {

            get
            {

                return fDriverVersionMinor;

            }

        }


        public static int driverVersionRelease
        {

            get
            {

                return fDriverVersionRelease;

            }

        }


        public static int driverVersionBuild
        {

            get
            {

                return fDriverVersionBuild;

            }

        }


        public static String driverVersionString
        {

            get
            {

                return fDriverVersionString;

            }

        }


        public static UInt32 logging(UInt32 loggingMask)
        {

            return virtualMIDILogging(loggingMask);

        }


        public void shutdown()
        {

            if (!virtualMIDIShutdown(fInstance))
            {

                int lastError = Marshal.GetLastWin32Error();

                TeVirtualMIDIException.ThrowExceptionForReasonCode(lastError);

            }

        }


        public void sendCommand(byte[] command)
        {

            if ((command == null) || (command.Length == 0))
            {

                return;

            }

            if (!virtualMIDISendData(fInstance, command, (UInt32)command.Length))
            {

                int lastError = Marshal.GetLastWin32Error();
                TeVirtualMIDIException.ThrowExceptionForReasonCode(lastError);

            }

        }


        public byte[] getCommand()
        {

            UInt32 length = fMaxSysexLength;

            if (!virtualMIDIGetData(fInstance, fReadBuffer, ref length))
            {

                int lastError = Marshal.GetLastWin32Error();
                TeVirtualMIDIException.ThrowExceptionForReasonCode(lastError);

            }

            byte[] outBytes = new byte[length];

            Array.Copy(fReadBuffer, outBytes, length);

            return outBytes;

        }

        public UInt64[] getProcessIds()
        {

            UInt32 length = 17 * sizeof(ulong);
            UInt32 count;

            if (!virtualMIDIGetProcesses(fInstance, fReadProcessIds, ref length))
            {

                int lastError = Marshal.GetLastWin32Error();
                TeVirtualMIDIException.ThrowExceptionForReasonCode(lastError);

            }

            count = length / sizeof(ulong);

            UInt64[] outIds = new UInt64[count];

            Array.Copy(fReadProcessIds, outIds, count);

            return outIds;

        }


        private byte[] fReadBuffer;
        private IntPtr fInstance;
        private UInt32 fMaxSysexLength;
        private UInt64[] fReadProcessIds;
        private static ushort fVersionMajor;
        private static ushort fVersionMinor;
        private static ushort fVersionRelease;
        private static ushort fVersionBuild;
        private static String fVersionString;

        private static ushort fDriverVersionMajor;
        private static ushort fDriverVersionMinor;
        private static ushort fDriverVersionRelease;
        private static ushort fDriverVersionBuild;
        private static String fDriverVersionString;


        [DllImport(DllName, EntryPoint = "virtualMIDICreatePortEx3", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr virtualMIDICreatePortEx3(string portName, IntPtr callback, IntPtr dwCallbackInstance, UInt32 maxSysexLength, UInt32 flags, ref Guid manufacturer, ref Guid product);

        [DllImport(DllName, EntryPoint = "virtualMIDICreatePortEx2", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr virtualMIDICreatePortEx2(string portName, IntPtr callback, IntPtr dwCallbackInstance, UInt32 maxSysexLength, UInt32 flags);

        [DllImport(DllName, EntryPoint = "virtualMIDIClosePort", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern void virtualMIDIClosePort(IntPtr instance);

        [DllImport(DllName, EntryPoint = "virtualMIDIShutdown", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern Boolean virtualMIDIShutdown(IntPtr instance);

        [DllImport(DllName, EntryPoint = "virtualMIDISendData", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern Boolean virtualMIDISendData(IntPtr midiPort, byte[] midiDataBytes, UInt32 length);

        [DllImport(DllName, EntryPoint = "virtualMIDIGetData", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern Boolean virtualMIDIGetData(IntPtr midiPort, [Out] byte[] midiDataBytes, ref UInt32 length);

        [DllImport(DllName, EntryPoint = "virtualMIDIGetProcesses", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern Boolean virtualMIDIGetProcesses(IntPtr midiPort, [Out] UInt64[] processIds, ref UInt32 length);

        [DllImport(DllName, EntryPoint = "virtualMIDIGetVersion", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr virtualMIDIGetVersion(ref ushort major, ref ushort minor, ref ushort release, ref ushort build);

        [DllImport(DllName, EntryPoint = "virtualMIDIGetDriverVersion", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr virtualMIDIGetDriverVersion(ref ushort major, ref ushort minor, ref ushort release, ref ushort build);

        [DllImport(DllName, EntryPoint = "virtualMIDILogging", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern UInt32 virtualMIDILogging(UInt32 loggingMask);
    }
}
