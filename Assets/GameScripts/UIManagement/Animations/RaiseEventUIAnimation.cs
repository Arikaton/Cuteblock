using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace GameScripts.UIManagement.Animations
{
    public class RaiseEventUIAnimation : UIAnimationBase
    {
        [SerializeField] private UnityEvent onAnimationStart;
        [SerializeField] private UnityEvent onAnimationInstantStart;
        
        protected override void StartAnimationInternal(Sequence sequence, float durationPercent)
        {
            onAnimationStart?.Invoke();
            sequence.onComplete?.Invoke();
        }

        protected override void StartInstantAnimationInternal()
        {
            onAnimationInstantStart?.Invoke();
        }
    }
}