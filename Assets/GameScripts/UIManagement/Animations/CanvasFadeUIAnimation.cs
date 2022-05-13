using DG.Tweening;
using UnityEngine;

namespace GameScripts.UIManagement.Animations
{
    public class CanvasFadeUIAnimation : UIAnimationBase
    {
        [SerializeField] private CanvasGroup _target;
        [SerializeField] [Range(0, 1)] protected float _targetValue;
        [SerializeField] private float _duration;
        [SerializeField] private Ease _ease;
        [SerializeField] private float _delay;
        
        protected override void StartAnimationInternal(Sequence sequence, float durationPercent)
        {
            if (_delay > 0)
            {
                sequence.AppendInterval(_delay);
            }
            sequence.Append(_target.DOFade(_targetValue, _duration * durationPercent).SetEase(_ease));
        }

        protected override void StartInstantAnimationInternal()
        {
            _target.alpha = _targetValue;
        }
    }
}