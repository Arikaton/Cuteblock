using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameScripts.Game
{ 
    [System.Serializable]
    public class ShapeData
    {
        public readonly int uid;
        public readonly Vector2Int rect;
        public readonly List<Vector2Int> points;
        public readonly Rotation rotation;

        public ShapeData(int uid, Vector2Int rect, params (int x, int y)[] points)
        {
            this.points = new List<Vector2Int>(points.Length);
            foreach (var point in points)
            {
                this.points.Add(new Vector2Int(point.x, point.y));
            }

            this.uid = uid;
            this.rect = rect;
            rotation = Rotation.Deg0;
        }
        
        public List<Vector2Int> PointsAfterRotation(Rotation rot)
        {
            return points.Select(point => point.RotateBy(rot)).ToList();
        }
    }
}