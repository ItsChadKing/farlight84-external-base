using System.Numerics;
using External.Farlight84.Game.Internal.Models;

namespace External.Farlight84.Game.Models
{
    internal class LocalPlayer
    {
        private static readonly Lazy<LocalPlayer> Instance = new(() => new LocalPlayer());
        public static LocalPlayer GetInstance => Instance.Value;

        public long LocalPlayerPointer { get; set; }
        public long PlayerPawnPointer { get; set; }
        public Vector3 Location { get; set; }
        public FCameraCacheEntry CameraCacheEntry { get; set; }

        private LocalPlayer(){}
    }
}
