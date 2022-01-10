using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameScripts.Game
{ 
    [System.Serializable]
    public class ShapeData
    {
        public int uid;
        public Vector2Int rect;
        public List<Vector2Int> points;

        public ShapeData(int uid, Vector2Int rect, params (int x, int y)[] points)
        {
            this.points = new List<Vector2Int>(points.Length);
            foreach (var point in points)
            {
                this.points.Add(new Vector2Int(point.x, point.y));
            }

            this.uid = 0;
            this.rect = rect;
        }
        
        public List<Vector2Int> PointsAfterRotation(Rotation rotation)
        {
            return points.Select(point => point.RotateBy(rotation)).ToList();
        }
        
    }
}