using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameScripts.Game
{ 
    [System.Serializable]
    public class ShapeData
    {
        [SerializeField] private int uid;
        [SerializeField] private Vector2Int rect;
        [SerializeField] private List<Vector2Int> points;

        public int Uid => uid;
        public Vector2Int Rect => rect;
        private List<Vector2Int> Points => points;

        public ShapeData(int uid, Vector2Int rect, params (int x, int y)[] points)
        {
            this.points = new List<Vector2Int>(points.Length);
            foreach (var point in points)
            {
                this.points.Add(new Vector2Int(point.x, point.y));
            }

            this.uid = uid;
            this.rect = rect;
        }
        
        public List<Vector2Int> PointsAfterRotation(Rotation rot)
        {
            return points.Select(point => point.RotateBy(rot)).ToList();
        }

        public Vector2 OriginCenterToShapeCenterDistanceNormalized(Rotation rotation)
        {
            return (new Vector2(rect.x * 0.5f, rect.y * 0.5f) - new Vector2(0.5f, 0.5f)).RotateBy(rotation);
        }
    }
}