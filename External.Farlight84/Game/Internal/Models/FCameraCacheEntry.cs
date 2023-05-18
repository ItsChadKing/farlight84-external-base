using System.Runtime.InteropServices;

namespace External.Farlight84.Game.Internal.Models
{
    [StructLayout(LayoutKind.Explicit)]
    public struct FCameraCacheEntry
    {
        [FieldOffset(0)]
        public float TimeStamp;

        [FieldOffset(0x10)]
        public FMinimalViewInfo Pov;
    }
}
