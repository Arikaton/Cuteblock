using DG.Tweening;
using UnityEngine;

namespace GameScripts.UIManagement.Animations
{
    public class LocalScaleUIAnimation : UIAnimationBase
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _targetValue;
        [SerializeField] private float _duration;
        [SerializeField] private Ease _ease;
        
        protected override void StartAnimationInternal(Sequence sequence, float durationPercent)
        {
            sequence.Append(_target.DOScale(_targetValue, _duration * durationPercent).SetEase(_ease));
        }

        protected override void StartInstantAnimationInternal()
        {
            _target.localScale = new Vector3(_targetValue, _targetValue, 1);
        }
    }
}