using TweensStateMachine.TweenTemplates;
using UnityEngine;
using UnityEngine.UI;

namespace TweensStateMachine
{
    public static class TweenAnimatorUIExtensions
    {
        public static TweenTemplate TTFade(this Image image, float endValue, float duration)
        {
            return new TweenTemplateColor(() => image.color, value => image.color = value, new Color(image.color.r, image.color.g, image.color.b, endValue), duration);
        }
        
        public static TweenTemplate TTColor(this Image image, Color endValue, float duration)
        {
            return new TweenTemplateColor(() => image.color, value => image.color = value, endValue, duration);
        }
    }
}