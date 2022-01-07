using System.Collections.Generic;
using UnityEngine;

namespace GameScripts.Game
{ 
    [System.Serializable]
    public struct ShapeModel
    {
        public int uid;
        public Vector2Int rect;
        public List<Vector2Int> points;
        public Rotation rotation;

        public ShapeModel(Vector2Int rect, params (int x, int y)[] points)
        {
            this.points = new List<Vector2Int>(points.Length);
            foreach (var point in points)
            {
                this.points.Add(new Vector2Int(point.x, point.y));
            }

            uid = 0;
            this.rect = rect;
            rotation = Rotation.Deg0;
        }
    }
}