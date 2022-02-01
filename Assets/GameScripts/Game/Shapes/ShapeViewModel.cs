using UniRx;
using UnityEngine;

namespace GameScripts.Game
{
    public class ShapeViewModel
    {
        private ShapeModel _model;
        private IReactiveProperty<Vector2Int> _positionOnGrid;
        private  FieldViewModel _fieldViewModel;
        private IReactiveProperty<bool> _highlighted;

        public ShapeData ShapeData;
        public IReadOnlyReactiveProperty<Vector2Int> PositionOnGrid;
        public IReadOnlyReactiveProperty<Rotation> Rotation;
        public IReadOnlyReactiveProperty<bool> Highlighted;
        public ReactiveProperty<bool> CanBePlaced;
        public ReactiveCommand Destroy;
        public int Uid => _model.Uid;
        public Vector2Int Rect { get; private set; }

        public ShapeViewModel(ShapeModel model, ShapeData shapeData, FieldViewModel fieldViewModel)
        {
            _model = model;
            ShapeData = shapeData;
            _fieldViewModel = fieldViewModel;
            Rect = shapeData.Rect;
            Rotation = _model.Rotation;
            _positionOnGrid = new ReactiveProperty<Vector2Int>(new Vector2Int(-1, -1));
            _highlighted = new ReactiveProperty<bool>(false);
            PositionOnGrid = _positionOnGrid;
            Highlighted = _highlighted;
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

        public void Click()
        {
            _fieldViewModel.ClickShape(this);
        }

        public void RotateClockwise()
        {
            _model.Rotation.Value = _model.Rotation.Value == Game.Rotation.Deg0
                ? Game.Rotation.Deg270
                : _model.Rotation.Value - 1;
        }

        public void EnableHighlighting()
        {
            _highlighted.Value = true;
        }
        
        public void DisableHighlighting()
        {
            _highlighted.Value = false;
        }
    }
}