using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;

namespace TweensStateMachine.TweenTemplates
{
    public class TweenTemplateColor : TweenTemplate
    {
        public DOGetter<Color> getter;
        public DOSetter<Color> setter;
        public Color endValue;
        public float duration;

        public TweenTemplateColor(DOGetter<Color> getter, DOSetter<Color> setter, Color endValue, float duration)
        {
            this.getter = getter;
            this.setter = setter;
            this.endValue = endValue;
            this.duration = duration;
        }

        internal override Tween CreateTweenWithoutSetting()
        {
            return DOTween.To(getter, setter, endValue, duration);
        }
    }
}