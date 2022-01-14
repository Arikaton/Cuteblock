using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GameScripts.UI
{
    [RequireComponent(typeof(Image))]
    public class CellAnimator : MonoBehaviour
    {
        [SerializeField] private float _duration = 0.4f;
        
        private Color _normalColor = Color.white;
        private Color _shadowedColor = Color.grey;
        private Color _occupiedColor = Color.blue;

        private Image _image;
        private Sequence _sequence;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        public void AnimateNormal()
        {
            AnimateInternal(_normalColor);
        }

        public void AnimateShadow()
        {
            AnimateInternal(_shadowedColor);
        }

        public void AnimateOccupied()
        {
            AnimateInternal(_occupiedColor);
        }

        private void AnimateInternal(Color targetColor)
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence.Append(_image.DOColor(targetColor, _duration));
        }
    }
}