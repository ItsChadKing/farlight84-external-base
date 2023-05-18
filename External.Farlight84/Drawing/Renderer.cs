using System.Numerics;
using External.Farlight84.Game.Internal.Models;

namespace External.Farlight84.Drawing
{
    internal class Renderer
    {
        public static Vector3 WorldToScreenX(Vector3 worldLocation, FCameraCacheEntry cameraCacheEntry)
        {
            var screenLocation = new Vector3();

            var loc = cameraCacheEntry.Pov.Location;
            var rot = cameraCacheEntry.Pov.Rotation;

            Vector3 cameraLocation = new(loc.X, loc.Y, loc.Z);
            Vector3 cameraRotation = new(rot.Pitch, rot.Yaw, rot.Roll);
            var tempMatrix = GetMatrix(cameraRotation, cameraLocation);

            Vector3 vAxisX = new(tempMatrix.M11, tempMatrix.M12, tempMatrix.M13);
            Vector3 vAxisY = new(tempMatrix.M21, tempMatrix.M22, tempMatrix.M23);
            Vector3 vAxisZ = new(tempMatrix.M31, tempMatrix.M32, tempMatrix.M33);

            var vDelta = worldLocation - cameraLocation;
            Vector3 vTransformed = new(
                Vector3.Dot(vDelta, vAxisY),
                Vector3.Dot(vDelta, vAxisZ),
                Vector3.Dot(vDelta, vAxisX)
            );

            if (vTransformed.Z < 1f)
            {
                vTransformed.Z = 1f;
            }

            var fovAngle = cameraCacheEntry.Pov.Fov;
            var screenCenterX = GameWindowDrawing.Width / 2;
            var screenCenterY = GameWindowDrawing.Height / 2;

            screenLocation.X = screenCenterX + vTransformed.X * (screenCenterX / (float)Math.Tan(fovAngle * (float)Math.PI / 360f)) / vTransformed.Z;
            screenLocation.Y = screenCenterY - vTransformed.Y * (screenCenterX / (float)Math.Tan(fovAngle * (float)Math.PI / 360f)) / vTransformed.Z;


            return screenLocation;
        }

        public static Matrix4x4 GetMatrix(Vector3 rot, Vector3 origin)
        {
            float[] radAngles = {
                rot.X * (float)Math.PI / 180.0f,
                rot.Y * (float)Math.PI / 180.0f,
                rot.Z * (float)Math.PI / 180.0f
            };

            float[] sinValues = {
                (float)Math.Sin(radAngles[0]),
                (float)Math.Sin(radAngles[1]),
                (float)Math.Sin(radAngles[2])
            };

            float[] cosValues = {
                (float)Math.Cos(radAngles[0]),
                (float)Math.Cos(radAngles[1]),
                (float)Math.Cos(radAngles[2])
            };

            return new Matrix4x4()
            {
                M11 = cosValues[0] * cosValues[1],
                M12 = cosValues[0] * sinValues[1],
                M13 = sinValues[0],
                M14 = 0.0f,
                M21 = sinValues[2] * sinValues[0] * cosValues[1] - cosValues[2] * sinValues[1],
                M22 = sinValues[2] * sinValues[0] * sinValues[1] + cosValues[2] * cosValues[1],
                M23 = -sinValues[2] * cosValues[0],
                M24 = 0.0f,
                M31 = -(cosValues[2] * sinValues[0] * cosValues[1] + sinValues[2] * sinValues[1]),
                M32 = cosValues[1] * sinValues[2] - cosValues[2] * sinValues[0] * sinValues[1],
                M33 = cosValues[2] * cosValues[0],
                M34 = 0.0f,
                M41 = origin.X,
                M42 = origin.Y,
                M43 = origin.Z,
                M44 = 1.0f
            };
        }

        public static Matrix4x4 MatrixMultiplication(Matrix4x4 matrixOne, Matrix4x4 matrixTwo)
        {
            return new Matrix4x4
            {
                M11 = matrixOne.M11 * matrixTwo.M11 + matrixOne.M12 * matrixTwo.M21 + matrixOne.M13 * matrixTwo.M31 + matrixOne.M14 * matrixTwo.M41,
                M12 = matrixOne.M11 * matrixTwo.M12 + matrixOne.M12 * matrixTwo.M22 + matrixOne.M13 * matrixTwo.M32 + matrixOne.M14 * matrixTwo.M42,
                M13 = matrixOne.M11 * matrixTwo.M13 + matrixOne.M12 * matrixTwo.M23 + matrixOne.M13 * matrixTwo.M33 + matrixOne.M14 * matrixTwo.M43,
                M14 = matrixOne.M11 * matrixTwo.M14 + matrixOne.M12 * matrixTwo.M24 + matrixOne.M13 * matrixTwo.M34 + matrixOne.M14 * matrixTwo.M44,
                M21 = matrixOne.M21 * matrixTwo.M11 + matrixOne.M22 * matrixTwo.M21 + matrixOne.M23 * matrixTwo.M31 + matrixOne.M24 * matrixTwo.M41,
                M22 = matrixOne.M21 * matrixTwo.M12 + matrixOne.M22 * matrixTwo.M22 + matrixOne.M23 * matrixTwo.M32 + matrixOne.M24 * matrixTwo.M42,
                M23 = matrixOne.M21 * matrixTwo.M13 + matrixOne.M22 * matrixTwo.M23 + matrixOne.M23 * matrixTwo.M33 + matrixOne.M24 * matrixTwo.M43,
                M24 = matrixOne.M21 * matrixTwo.M14 + matrixOne.M22 * matrixTwo.M24 + matrixOne.M23 * matrixTwo.M34 + matrixOne.M24 * matrixTwo.M44,
                M31 = matrixOne.M31 * matrixTwo.M11 + matrixOne.M32 * matrixTwo.M21 + matrixOne.M33 * matrixTwo.M31 + matrixOne.M34 * matrixTwo.M41,
                M32 = matrixOne.M31 * matrixTwo.M12 + matrixOne.M32 * matrixTwo.M22 + matrixOne.M33 * matrixTwo.M32 + matrixOne.M34 * matrixTwo.M42,
                M33 = matrixOne.M31 * matrixTwo.M13 + matrixOne.M32 * matrixTwo.M23 + matrixOne.M33 * matrixTwo.M33 + matrixOne.M34 * matrixTwo.M43,
                M34 = matrixOne.M31 * matrixTwo.M14 + matrixOne.M32 * matrixTwo.M24 + matrixOne.M33 * matrixTwo.M34 + matrixOne.M34 * matrixTwo.M44,
                M41 = matrixOne.M41 * matrixTwo.M11 + matrixOne.M42 * matrixTwo.M21 + matrixOne.M43 * matrixTwo.M31 + matrixOne.M44 * matrixTwo.M41,
                M42 = matrixOne.M41 * matrixTwo.M12 + matrixOne.M42 * matrixTwo.M22 + matrixOne.M43 * matrixTwo.M32 + matrixOne.M44 * matrixTwo.M42,
                M43 = matrixOne.M41 * matrixTwo.M13 + matrixOne.M42 * matrixTwo.M23 + matrixOne.M43 * matrixTwo.M33 + matrixOne.M44 * matrixTwo.M43,
                M44 = matrixOne.M41 * matrixTwo.M14 + matrixOne.M42 * matrixTwo.M24 + matrixOne.M43 * matrixTwo.M34 + matrixOne.M44 * matrixTwo.M44
            };
        }
    }
}
