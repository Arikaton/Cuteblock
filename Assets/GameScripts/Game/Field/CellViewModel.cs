using UniRx;

namespace GameScripts.Game
{
    public class CellViewModel
    {
        private IReactiveProperty<CellStates> _cellState;
        private IReactiveProperty<bool> _shadowed;
        private IReactiveProperty<bool> _highlighted;
        
        public IReadOnlyReactiveProperty<CellStates> CellState;
        public IReadOnlyReactiveProperty<bool> Shadowed;
        public IReadOnlyReactiveProperty<bool> Highlighted;

        public CellViewModel(CellStates cellState)
        {
            _cellState = new ReactiveProperty<CellStates>(cellState);
            _shadowed = new ReactiveProperty<bool>(false);
            _highlighted = new ReactiveProperty<bool>(false);
            CellState = _cellState;
            Shadowed = _shadowed;
            Highlighted = _highlighted;
        }

        public void ChangeState(CellStates cellState)
        {
            _cellState.Value = cellState;
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