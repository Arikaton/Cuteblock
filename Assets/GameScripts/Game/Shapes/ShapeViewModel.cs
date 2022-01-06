using System.Collections.Generic;
using System.Linq;

namespace GameScripts.Game
{
    public class ShapeViewModel
    {
        private ShapeModel _model;

        public ShapeViewModel(ShapeModel model)
        {
            _model = model;
        }
        
        public List<Vector2IntS> PointsAfterRotation()
        {
            return _model.points.Select(point => point.RotateBy(_model.rotation)).ToList();
        }

        public void SetRotation(Rotation rotation)
        {
            _model.rotation = rotation;
        }
    }
}