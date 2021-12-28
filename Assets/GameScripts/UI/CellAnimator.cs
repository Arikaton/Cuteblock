using DG.Tweening;
using UnityEngine;

namespace GameScripts.UI
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class CellAnimator : MonoBehaviour
    {
        [SerializeField] private Color _highlightedColor = Color.white;
        [SerializeField] private Color _busyColor = Color.white;
        [SerializeField] private float _duration = 0.4f;

        private SpriteRenderer _spriteRenderer;
        private Sequence _sequence;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void AnimateHighlight()
        {
            AnimateInternal(_highlightedColor);
        }

        public void AnimateBusy()
        {
            AnimateInternal(_busyColor);
        }

        public void AnimateClear()
        {
            AnimateInternal(Color.white);
        }

        private void AnimateInternal(Color targetColor)
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence.Append(_spriteRenderer.DOColor(targetColor, _duration));
        }
    }
}