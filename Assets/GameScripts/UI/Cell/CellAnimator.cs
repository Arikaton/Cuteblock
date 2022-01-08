using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GameScripts.UI
{
    [RequireComponent(typeof(Image))]
    public class CellAnimator : MonoBehaviour
    {
        [SerializeField] private Color _highlightedColor = Color.grey;
        [SerializeField] private Color _busyColor = Color.blue;
        [SerializeField] private float _duration = 0.4f;

        private Image _image;
        private Sequence _sequence;

        private void Awake()
        {
            _image = GetComponent<Image>();
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
            _sequence.Append(_image.DOColor(targetColor, _duration));
        }
    }
}