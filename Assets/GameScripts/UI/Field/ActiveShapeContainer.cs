using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GameScripts.Game
{
    public class ActiveShapeContainer : MonoBehaviour
    {
        public RectTransform containerRect;
        public RectTransform shapeRect;
        [SerializeField] private Image image;

        private Sequence _sequence;
        private Vector2 _initialPosition;

        private void Awake()
        {
            _initialPosition = containerRect.anchoredPosition;
        }

        public void AnimatePopUp()
        {
            _sequence?.Complete();
            _sequence = DOTween.Sequence();
            _sequence.Insert(0f, image.DOFade(1, 0.3f));
        }

        public void AnimateHide()
        {
            _sequence?.Complete();
            _sequence = DOTween.Sequence();
            _sequence.Insert(0f, image.DOFade(0, 0.3f));
        }

        public void ResetAnchoredPosition()
        {
            containerRect.anchoredPosition = _initialPosition;
        }
    }
}