using DG.Tweening;
using TweensStateMachine.TweenTemplates;

namespace TweensStateMachine
{
    public static class TweenAnimatorSettingsExtensions
    {
        public static T SetEase<T>(this T tt, Ease ease) where T : TweenTemplate
        {
            tt.ease = ease;
            return tt;
        }
    }
}