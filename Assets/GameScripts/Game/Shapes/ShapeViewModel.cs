using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameScripts.Game
{
    public class ShapeViewModel
    {
        private ShapeModel _model;

        public int Uid
        {
            get => _model.uid;
        }

        public Rotation Rotation
        {
            get => _model.rotation;
            set => _model.rotation = value;
        }

        public ShapeViewModel(ShapeModel model)
        {
            _model = model;
        }

        public List<Vector2Int> PointsAfterRotation()
        {
            return _model.points.Select(point => point.RotateBy(_model.rotation)).ToList();
        }
    }
}