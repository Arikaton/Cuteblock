using System;

namespace GameScripts.UIManagement
{
    public interface IUIAnimation
    {
        public event Action<bool> AnimationFinished;
        public float Progress { get; }
        public void StartAnimation(float durationPercent = 1f);
        public void StartInstantAnimation();
        public void ForceStopAnimation();
    }
}