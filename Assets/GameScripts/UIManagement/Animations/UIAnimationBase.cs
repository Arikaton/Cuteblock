using System;
using DG.Tweening;
using UnityEngine;

namespace GameScripts.UIManagement.Animations
{
    public abstract class UIAnimationBase : UIAnimation
    {
        public override event Action<bool> AnimationFinished;
        public override float Progress => _sequence.ElapsedPercentage();

        private Sequence _sequence;

        public override void StartAnimation(float durationPercent = 1)
        {
            if (_sequence.IsActive())
                return;
            _sequence = DOTween.Sequence();
            _sequence.OnComplete(OnAnimationComplete);
            StartAnimationInternal(_sequence, Mathf.Clamp01(durationPercent));
        }

        public override void ForceStopAnimation()
        {
            _sequence?.Kill();
            AnimationFinished?.Invoke(false);
        }

        public override void StartInstantAnimation()
        {
            if (_sequence.IsActive())
                _sequence.Kill();
            StartInstantAnimationInternal();
        }

        protected abstract void StartAnimationInternal(Sequence sequence, float durationPercent);

        protected abstract void StartInstantAnimationInternal();

        private void OnAnimationComplete()
        {
            _sequence?.Kill();
            AnimationFinished?.Invoke(true);
        }
    }
}