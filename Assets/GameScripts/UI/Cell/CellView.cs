using System;
using GameScripts.Game;
using UniRx;
using UnityEngine;

namespace GameScripts.UI
{
    [RequireComponent(typeof(CellAnimator))]
    public class CellView : MonoBehaviour
    {
        [SerializeField] private CellAnimator cellAnimator;
        
        private CellViewModel _cellViewModel;
        private CompositeDisposable _disposables = new CompositeDisposable();


        public void Bind(CellViewModel cellViewModel)
        {
            _cellViewModel = cellViewModel;
            _cellViewModel.CellState.Subscribe(ChangeCellState).AddTo(_disposables);
            _cellViewModel.Shadowed.Subscribe(AnimateShadow).AddTo(_disposables);
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }

        private void ChangeCellState(CellStates cellState)
        {
            if (cellState == CellStates.Empty)
                AnimateNormal();
            else if (cellState == CellStates.Occupied)
                AnimateOccupied();
            else
                throw new ArgumentOutOfRangeException(nameof(cellState), cellState, null);
        }

        private void AnimateNormal()
        {
            cellAnimator.AnimateNormal();
        }

        private void AnimateOccupied()
        {
            cellAnimator.AnimateOccupied();
        }

        private void AnimateShadow(bool shadow)
        {
            if (_cellViewModel.CellState.Value == CellStates.Occupied)
                return;
            if(shadow)
                cellAnimator.AnimateShadow();
            else
                cellAnimator.AnimateNormal();
        }
    }
}