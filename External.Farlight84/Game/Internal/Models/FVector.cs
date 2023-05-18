using System.Numerics;

namespace External.Farlight84.Game.Internal.Models
{
    public struct FVector
    {
        public float X;
        public float Y;
        public float Z;

        public Vector3 ToVector3()
        {
            return new Vector3(X, Y, Z);
        }

        public float DistTo(Vector3 entity)
        {
            return (ToVector3() - entity).Length();
        }
    }
}
