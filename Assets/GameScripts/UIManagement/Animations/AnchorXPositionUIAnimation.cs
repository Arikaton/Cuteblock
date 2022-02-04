using DG.Tweening;
using UnityEngine;

namespace GameScripts.UIManagement.Animations
{
    public class AnchorXPositionUIAnimation : UIAnimationBase
    {
        [SerializeField] private RectTransform _target;
        [SerializeField] private float _targetValueX;
        [SerializeField] private float _duration;
        [SerializeField] private Ease _ease;
        
        protected override void StartAnimationInternal(Sequence sequence, float durationPercent)
        {
            sequence.Append(_target.DOAnchorPosX(_targetValueX, _duration * durationPercent).SetEase(_ease));
        }

        protected override void StartInstantAnimationInternal()
        {
            _target.anchoredPosition = new Vector2(_targetValueX, _target.anchoredPosition.y);
        }
    }
}