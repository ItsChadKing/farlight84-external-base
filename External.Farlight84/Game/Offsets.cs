namespace External.Farlight84.Game
{
    internal class Offsets
    {
        public const long GName = 0x60E5F00;
        public const long GObject = 0x60FE750;
        public const long GWorld = 0x6255870;
        public const long ComponentToWorld = 0x250;


        internal class Actor
        {
            public const long RootComponent = 0x138;
            public const long RootComponentStartLocation = 0x11c;
            public const long Mesh = 0x288;
            public const long BoneArray = 0x598;
        }

        internal class LocalActor
        {
            public const long PlayerController = 0x30;
            public const long AcknowledgedPawn = 0x2C0;
            public const long PlayerCameraManager = 0x2D8;
            public const long CameraCacheEntry = 0x1BF0;
        }
    }
}
