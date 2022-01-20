using System;
using UnityEngine;

namespace GameScripts.Game
{
    public static class ExtensionMethods
    {
        public static readonly int[,] rotationMatrix0 = new int[2, 2] {{1, 0}, {0, 1}};
        public static readonly int[,] rotationMatrix90 = new int[2, 2] {{0, -1}, {1, 0}};
        public static readonly int[,] rotationMatrix180 = new int[2, 2] {{-1, 0}, {0, -1}};
        public static readonly int[,] rotationMatrix270 = new int[2, 2] {{0, 1}, {-1, 0}};

        public static int AngleValue(this Rotation rotation)
        {
            switch (rotation)
            {
                case Rotation.Deg0:
                    return 0;
                case Rotation.Deg90:
                    return 90;
                case Rotation.Deg180:
                    return 180;
                case Rotation.Deg270:
                    return 270;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rotation), rotation, null);
            }
        }

        public static Vector2Int RotateBy(this Vector2Int vector, Rotation rotation)
        {
            var rm = GetRotationMatrix(rotation);
            return new Vector2Int(vector.x * rm[0, 0] + vector.y * rm[0, 1], vector.x * rm[1, 0] + vector.y * rm[1, 1]);
        }
        
        public static Vector2 RotateBy(this Vector2 vector, Rotation rotation)
        {
            var rm = GetRotationMatrix(rotation);
            return new Vector2(vector.x * rm[0, 0] + vector.y * rm[0, 1], vector.x * rm[1, 0] + vector.y * rm[1, 1]);
        }

        private static int[,] GetRotationMatrix(Rotation rotation)
        {
            switch (rotation)
            {
                case Rotation.Deg0:
                    return rotationMatrix0;
                case Rotation.Deg90:
                    return rotationMatrix90;
                case Rotation.Deg180:
                    return rotationMatrix180;
                case Rotation.Deg270:
                    return rotationMatrix270;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rotation), rotation, null);
            }
        }
    }
}