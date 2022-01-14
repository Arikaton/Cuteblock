using DG.Tweening;
using LeTai.TrueShadow;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace GameScripts.UI.Shop
{
    [RequireComponent(typeof(Toggle))]
    public class ShopToggle : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CanvasGroup _resourceIcon;
        [SerializeField] private Image _coverImage;

        [Header("Animations settings")]
        [SerializeField] private float _scaleOnDuration;
        [SerializeField] private AnimationCurve _scaleOnEase;
        [SerializeField] private float _scaleOffDuration;
        [SerializeField] private AnimationCurve _scaleOffEase;
        [SerializeField] private float _resourceOnDuration;
        [SerializeField] private AnimationCurve _resourceOnEase;
        [SerializeField] private float _resourceOffDuration;
        [SerializeField] private AnimationCurve _resorceOffEase;
        
        private Toggle _toggle;
        private Vector2 _resourceIconStartAnchoredPosition;
        private RectTransform _resourceIconRect;
        private Sequence _sequence;

        private void Awake()
        {
            _toggle = GetComponent<Toggle>();
            _resourceIconRect = _resourceIcon.GetComponent<RectTransform>();
            _resourceIconStartAnchoredPosition = _resourceIconRect.anchoredPosition;
            _toggle.OnValueChangedAsObservable().Subscribe(OnToggleValueChanged).AddTo(this);
        }

        private void OnToggleValueChanged(bool isOn)
        {
            if (_sequence.IsActive())
                _sequence.Kill();
            if (isOn)
            {
                ShowOnAnimation();
            }
            else
            {
                ShowOffAnimation();
            }
        }

        private void ShowOnAnimation()
        {
            _sequence = DOTween.Sequence();
            _sequence.Append(_coverImage.transform.DOScaleX(1, _scaleOnDuration).SetEase(_scaleOnEase));
            _sequence.AppendCallback(() => _resourceIcon.alpha = 1);
            _sequence.Append(_resourceIconRect.DOAnchorPos(_resourceIconStartAnchoredPosition, _resourceOnDuration)
                .SetEase(_resourceOnEase));
        }

        private void ShowOffAnimation()
        {
            _sequence = DOTween.Sequence();
            _sequence.Append(_resourceIconRect.DOAnchorPos(Vector2.zero, _resourceOffDuration).SetEase(_resorceOffEase));
            _sequence.AppendCallback(() => _resourceIcon.alpha = 0);
            _sequence.Append(_coverImage.transform.DOScaleX(0, _scaleOffDuration).SetEase(_scaleOffEase));
        }
    }
}