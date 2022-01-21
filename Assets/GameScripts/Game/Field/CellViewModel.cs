using UniRx;

namespace GameScripts.Game
{
    public class CellViewModel
    {
        private IReactiveProperty<bool> _occupied;
        private IReactiveProperty<bool> _shadowed;
        private IReactiveProperty<bool> _highlighted;
        
        public IReadOnlyReactiveProperty<bool> Occupied;
        public IReadOnlyReactiveProperty<bool> Shadowed;
        public IReadOnlyReactiveProperty<bool> Highlighted;

        public CellViewModel()
        {
            _occupied = new ReactiveProperty<bool>(false);
            _shadowed = new ReactiveProperty<bool>(false);
            _highlighted = new ReactiveProperty<bool>(false);
            Occupied = _occupied;
            Shadowed = _shadowed;
            Highlighted = _highlighted;
        }

        public void ChangeState(bool occupied)
        {
            _occupied.Value = occupied;
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

    public enum CellStates
    {
        Empty = 0,
        Occupied = 1
    }
}