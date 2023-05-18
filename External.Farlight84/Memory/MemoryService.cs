using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using static External.Farlight84.Memory.NativeImport;

namespace External.Farlight84.Memory
{
    internal unsafe class MemoryService
    {
        // Address which identifies the beginning of a process.
        public static nint BaseAddress;

        // The handle that the operating system assigned to the associated process when the process was started.
        public static nint ProcessHandle;

        // The window handle that the game assigned to the process when the process was started.
        public static nint MainWindowHandle;

        public static uint ProcessId;

        public static void Initialize(int processId)
        {
            var process = Process.GetProcessById(processId);
            ProcessId = (uint)process.Id;

            BaseAddress = process.MainModule!.BaseAddress;

            ProcessHandle = OpenProcess(0x0010 | 0x0020 | 0x0008, false, processId);
            MainWindowHandle = process.MainWindowHandle;
        }

        public static T Read<T>(long address) where T : struct
        {
            var size = Marshal.SizeOf<T>();

            var bufferPtr = stackalloc byte[size];
            var buffer = ReadBytes(address, bufferPtr, size, out var success);

            return success ? MemoryMarshal.Read<T>(buffer) : default!;
        }
        public static string ReadString(long address, int size, Encoding encoding)
        {
            if (address == 0)
            {
                return string.Empty;
            }

            var bufferPtr = stackalloc byte[size];
            var buffer = ReadBytes(address, bufferPtr, size, out var success);

            return success ? encoding.GetString(buffer) : string.Empty;
        }

        private static ReadOnlySpan<byte> ReadBytes(long address, byte* bufferPtr, int size, out bool success)
        {
            // Read the data from the target process
            success = ReadProcessMemory(ProcessHandle, (nint)address, bufferPtr, size, out var bytesRead) && size == bytesRead;

            return new ReadOnlySpan<byte>(bufferPtr, size);
        }
    }
}
