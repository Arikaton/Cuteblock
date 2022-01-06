using System.Collections.Generic;

namespace GameScripts.Game
{ 
    [System.Serializable]
    public struct ShapeModel
    {
        public int uid;
        public Vector2IntS rect;
        public List<Vector2IntS> points;
        public Rotation rotation;

        public ShapeModel(Vector2IntS rect, params (int x, int y)[] points)
        {
            this.points = new List<Vector2IntS>(points.Length);
            foreach (var point in points)
            {
                this.points.Add(new Vector2IntS(point.x, point.y));
            }

            uid = 0;
            this.rect = rect;
            rotation = Rotation.Deg0;
        }
    }
}