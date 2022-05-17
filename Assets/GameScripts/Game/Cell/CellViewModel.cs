using UniRx;
using UnityEngine;

namespace GameScripts.Game
{
    public class CellViewModel
    {
        private CellModel _model;
        
        private IReactiveProperty<bool> _shadowed;
        private IReactiveProperty<bool> _highlighted;
        
        public IReadOnlyReactiveProperty<bool> Occupied;
        public IReadOnlyReactiveProperty<bool> Shadowed;
        public IReadOnlyReactiveProperty<bool> Highlighted;

        public Vector2Int PositionOnField => _model.positionOnField;

        public CellViewModel(CellModel model)
        {
            _model = model;
            _shadowed = new ReactiveProperty<bool>(false);
            _highlighted = new ReactiveProperty<bool>(false);
            Occupied = _model.uid.Select(uid => uid != 0).ToReadOnlyReactiveProperty();
            Shadowed = _shadowed;
            Highlighted = _highlighted;
        }

        public void TurnOnShadow()
        {
            _shadowed.Value = true;
        }

        public void TurnOffShadow()
        {
            _shadowed.Value = false;
        }

        public void TurnOnHighlight()
        {
            _highlighted.Value = true;
        }

        public void TurnOffHighlight()
        {
            _highlighted.Value = false;
        }
    }
}