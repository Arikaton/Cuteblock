using UnityEngine;

namespace GameScripts.UI
{
    [RequireComponent(typeof(CellAnimator))]
    public class CellView : MonoBehaviour
    {
        private CellAnimator _cellAnimator;

        private void Awake()
        {
            _cellAnimator = GetComponent<CellAnimator>();
        }

        public void AnimateNormal()
        {
            _cellAnimator.AnimateNormal();
        }

        public void AnimateShadow()
        {
            _cellAnimator.AnimateShadow();
        }

        public void AnimateOccupied()
        {
            _cellAnimator.AnimateOccupied();
        }
    }
}