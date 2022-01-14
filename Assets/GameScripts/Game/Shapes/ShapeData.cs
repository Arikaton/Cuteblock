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
        [SerializeField] private Rotation rotation;

        public int Uid => uid;
        private Vector2Int Rect => rect;
        private List<Vector2Int> Points => points;
        private Rotation Rotation => rotation;

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