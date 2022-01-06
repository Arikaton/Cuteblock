using System;
using UnityEngine;

namespace GameScripts.Game
{
    [System.Serializable]
    public struct Vector2IntS
    {
        public int x;
        public int y;

        public static readonly int[,] rotationMatrix0 = new int[2, 2] {{1, 0}, {0, 1}};
        public static readonly int[,] rotationMatrix90 = new int[2, 2] {{0, -1}, {1, 0}};
        public static readonly int[,] rotationMatrix180 = new int[2, 2] {{-1, 0}, {0, -1}};
        public static readonly int[,] rotationMatrix270 = new int[2, 2] {{0, 1}, {-1, 0}};

        public Vector2IntS(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2IntS RotateBy(Rotation rotation)
        {
            var rm = GetRotationMatrix(rotation);
            return new Vector2IntS(x * rm[0, 0] + y * rm[0, 1], x * rm[1, 0] + y * rm[1, 1]);
        }

        private int[,] GetRotationMatrix(Rotation rotation)
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

        public static Vector2IntS operator +(Vector2IntS vector1, Vector2IntS vector2)
        {
            return new Vector2IntS(vector1.x + vector2.x, vector1.y + vector2.y);
        }

        public static bool operator ==(Vector2IntS vector1, Vector2IntS vector2)
        {
            return vector1.x == vector2.x && vector1.y == vector2.y;
        }

        public static bool operator !=(Vector2IntS vector1, Vector2IntS vector2)
        {
            return !(vector1 == vector2);
        }

        public static implicit operator Vector2Int(Vector2IntS vector)
        {
            return new Vector2Int(vector.x, vector.y);
        }
    }
}