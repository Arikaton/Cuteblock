using UnityEngine;

namespace GameScripts.Game
{
    public class FieldViewModel
    {
        private FieldModel _fieldModel;
        private RectInt _rect;

        public FieldViewModel(FieldModel fieldModel)
        {
            _fieldModel = fieldModel;
            _rect = new RectInt(0, 0, 8, 8);
        }

        public bool CanPlaceShape(ShapeViewModel shape, Vector2IntS cell)
        {
            var points = shape.PointsAfterRotation();
            foreach (var point in points)
            {
                var pointPositionOnGrid = cell + point;
                if (!_rect.Contains(pointPositionOnGrid))
                    return false;
                if (_fieldModel.FieldMatrix[pointPositionOnGrid.x, pointPositionOnGrid.y].uid != 0)
                    return false;
            }
            return true;
        }
    }
}