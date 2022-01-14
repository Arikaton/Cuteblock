using System;
using UnityEngine;

namespace GameScripts.UIManagement.Animations
{
    public abstract class UIAnimation : MonoBehaviour, IUIAnimation
    {
        public abstract event Action<bool> AnimationFinished;
        public abstract float Progress { get; }
        public abstract void StartAnimation(float durationPercent = 1);
        public abstract void StartInstantAnimation();
        public abstract void ForceStopAnimation();
    }
}