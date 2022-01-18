using UniRx;

namespace GameScripts.Game
{
    public class CellViewModel
    {
        private IReactiveProperty<CellStates> _cellState;
        private IReactiveProperty<bool> _shadowed;
        
        public IReadOnlyReactiveProperty<CellStates> CellState;
        public IReadOnlyReactiveProperty<bool> Shadowed;

        public CellViewModel(CellStates cellState)
        {
            _cellState = new ReactiveProperty<CellStates>(cellState);
            _shadowed = new ReactiveProperty<bool>(false);
            CellState = _cellState;
            Shadowed = _shadowed;
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
    }

    public enum CellStates
    {
        Empty = 0,
        Occupied = 1
    }
}