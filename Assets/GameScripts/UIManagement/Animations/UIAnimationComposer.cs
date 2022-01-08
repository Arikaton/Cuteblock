using System;
using System.Linq;
using UnityEngine;

namespace GameScripts.UIManagement.Animations
{
    public class UIAnimationComposer : UIAnimation
    {
        [SerializeField] private UIAnimationBase[] _animations;

        private bool _isWaitingForComplete = false;
        private int _completedAnimationsCounter = 0;
        
        public override event Action<bool> AnimationFinished;
        public override float Progress => _animations.Min(anim => anim.Progress);

        private void Awake()
        {
            foreach (var anim in _animations)
            {
                anim.AnimationFinished += OnAnimationFinished;
            }
        }

        public override void StartAnimation(float durationPercent = 1)
        {
            _isWaitingForComplete = true;
            _completedAnimationsCounter = 0;
            foreach (var anim in _animations)
            {
                anim.StartAnimation(durationPercent);
            }
        }

        public override void StartInstantAnimation()
        {
            foreach (var anim in _animations)
            {
                anim.StartInstantAnimation();
            }
        }

        public override void ForceStopAnimation()
        {
            _isWaitingForComplete = false;
            _completedAnimationsCounter = 0;
            foreach (var anim in _animations)
            {
                anim.ForceStopAnimation();
            }
            AnimationFinished?.Invoke(false);
        }

        private void OnAnimationFinished(bool isFinished)
        {
            if (!isFinished)
                return;
            if (!_isWaitingForComplete)
                return;
            _completedAnimationsCounter++;
            if (_completedAnimationsCounter == _animations.Length)
            {
                _isWaitingForComplete = false;
                _completedAnimationsCounter = 0;
                AnimationFinished?.Invoke(true);
            }
        }
    }
}