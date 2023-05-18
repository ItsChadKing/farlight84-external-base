using External.Farlight84.Memory;
using System.Text;

namespace External.Farlight84.Game
{
    internal class GameUtilities
    {
        public static string GetNameFromFName(long gNameOffset, int key)
        {
            var blockOffset = key >> 16;
            var nameOffset = (ushort)(key & 65535);

            var fNamePool = MemoryService.BaseAddress + gNameOffset + 0x10;
            var fNamePoolChunk = MemoryService.Read<long>(fNamePool + blockOffset * 0x8);
            var fNameEntry = fNamePoolChunk + (0x2 * nameOffset);

            var fNameEntryHeader = MemoryService.Read<short>(fNameEntry);
            var fNamePtr = fNameEntry + 0x2;
            var fNameLength = fNameEntryHeader >> 0x6;

            return fNameLength >= 0
                ? MemoryService.ReadString(fNamePtr, fNameLength, Encoding.UTF8)
                : string.Empty;
        }

        public static float ToMeters(float x)
        {
            return x / 39.62f;
        }
    }
}