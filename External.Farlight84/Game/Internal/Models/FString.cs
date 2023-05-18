using System.Runtime.InteropServices;

namespace External.Farlight84.Game.Internal.Models
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FString
    {
        public IntPtr pBuffer;
        public int length;
    }
}
