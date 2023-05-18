using System.Numerics;
using System.Runtime.InteropServices;

namespace External.Farlight84.Game.Internal.Models
{
    [StructLayout(LayoutKind.Explicit)]
    public struct FTransform
    {
        [FieldOffset(0)]
        public FQuat Rotation;

        [FieldOffset(0x10)]
        public FVector Translation;

        [FieldOffset(0x20)]
        public FVector Scale3D;

        public Matrix4x4 ToMatrixWithScale()
        {
            Matrix4x4 matrix = new()
            {
                M41 = Translation.X,
                M42 = Translation.Y,
                M43 = Translation.Z
            };

            float x2 = Rotation.X + Rotation.X;
            float y2 = Rotation.Y + Rotation.Y;
            float z2 = Rotation.Z + Rotation.Z;

            float xx2 = Rotation.X * x2;
            float yy2 = Rotation.Y * y2;
            float zz2 = Rotation.Z * z2;
            matrix.M11 = (1.0f - (yy2 + zz2)) * Scale3D.X;
            matrix.M22 = (1.0f - (xx2 + zz2)) * Scale3D.Y;
            matrix.M33 = (1.0f - (xx2 + yy2)) * Scale3D.Z;

            float yz2 = Rotation.Y * z2;
            float wx2 = Rotation.W * x2;
            matrix.M32 = (yz2 - wx2) * Scale3D.Z;
            matrix.M23 = (yz2 + wx2) * Scale3D.Y;

            float xy2 = Rotation.X * y2;
            float wz2 = Rotation.W * z2;
            matrix.M21 = (xy2 - wz2) * Scale3D.Y;
            matrix.M12 = (xy2 + wz2) * Scale3D.X;

            float xz2 = Rotation.X * z2;
            float wy2 = Rotation.W * y2;
            matrix.M31 = (xz2 + wy2) * Scale3D.Z;
            matrix.M13 = (xz2 - wy2) * Scale3D.X;

            matrix.M14 = 0.0f;
            matrix.M24 = 0.0f;
            matrix.M34 = 0.0f;
            matrix.M44 = 1.0f;

            return matrix;
        }
    }
}
