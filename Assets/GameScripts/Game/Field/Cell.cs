using UnityEngine;

namespace GameScripts.Game
{
    [System.Serializable]
    public struct Cell
    {
        public int uid;
        public Vector2Int positionInShape;
        public Rotation shapeRotation;
    }
}