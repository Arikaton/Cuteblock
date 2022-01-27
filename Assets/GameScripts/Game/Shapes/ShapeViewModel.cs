using UniRx;
using UnityEngine;

namespace GameScripts.Game
{
    public class ShapeViewModel
    {
        private ShapeModel _model;
        private IReactiveProperty<Vector2Int> _positionOnGrid;

        public ShapeData ShapeData;
        public IReadOnlyReactiveProperty<Vector2Int> PositionOnGrid;
        public IReadOnlyReactiveProperty<Rotation> Rotation;
        public ReactiveProperty<bool> CanBePlaced;
        public ReactiveCommand Destroy;
        public int Uid => _model.Uid;
        public Vector2Int Rect { get; private set; }

        public ShapeViewModel(ShapeModel model, ShapeData shapeData)
        {
            _model = model;
            ShapeData = shapeData;
            Rect = shapeData.Rect;
            Rotation = _model.Rotation;
            _positionOnGrid = new ReactiveProperty<Vector2Int>(new Vector2Int(-1, -1));
            PositionOnGrid = _positionOnGrid;
            CanBePlaced = new ReactiveProperty<bool>(false);
            Destroy = new ReactiveCommand();
        }

        public void PlaceShapeAt(Vector2Int cell)
        {
            _positionOnGrid.Value = cell;
        }
        
        public Vector2 OriginCenterToShapeCenterDistanceNormalized()
        {
            return ShapeData.OriginCenterToShapeCenterDistanceNormalized(Rotation.Value);
        }
    }
}