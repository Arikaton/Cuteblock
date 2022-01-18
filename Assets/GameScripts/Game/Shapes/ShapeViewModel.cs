using UniRx;
using UnityEngine;

namespace GameScripts.Game
{
    public class ShapeViewModel
    {
        private ShapeModel _model;
        private IReactiveProperty<Vector2Int> _positionOnGrid;

        public IReadOnlyReactiveProperty<Vector2Int> PositionOnGrid;
        public IReadOnlyReactiveProperty<Rotation> Rotation;
        public int Uid => _model.Uid;
        public Vector2Int Rect { get; private set; }

        public ShapeViewModel(ShapeModel model, Vector2Int rect)
        {
            _model = model;
            Rect = rect;
            Rotation = _model.Rotation;
            _positionOnGrid = new ReactiveProperty<Vector2Int>(new Vector2Int(-1, -1));
            PositionOnGrid = _positionOnGrid;
        }

        public void PlaceShapeAt(Vector2Int cell)
        {
            _positionOnGrid.Value = cell;
        }
    }
}