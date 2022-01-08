using UnityEngine;

namespace GameScripts.Game
{
    [System.Serializable]
    public class Cell
    {
        public int uid;
        public int hp;
        public Vector2Int positionInShape;
        public Rotation shapeRotation;

        public Cell()
        {
            hp = 1;
        }
    }
}