using DG.Tweening;

namespace TweensStateMachine.TweenTemplates
{
    public abstract class TweenTemplate
    {
        internal Ease ease = Ease.Linear;

        public Tween CreateTween()
        {
            return CreateTweenWithoutSetting().SetEase(ease);
        }

        internal abstract Tween CreateTweenWithoutSetting();
    }
}