using System.Collections.Generic;
using TweensStateMachine.TweenTemplates;

namespace TweensStateMachine
{
    internal class State
    {
        public string StateName { get; }
        private List<TweenTemplate> TweenTemplates;

        public State(string stateName, params TweenTemplate[] tweenTemplates)
        {
            StateName = stateName;
            TweenTemplates = new List<TweenTemplate>();
            foreach (var tt in tweenTemplates)
            {
                TweenTemplates.Add(tt);
            }
        }

        public List<TweenTemplate> OnEnter()
        {
            return TweenTemplates;
        }

        public void AddTweenTemplate(TweenTemplate tweenTemplate)
        {
            TweenTemplates.Add(tweenTemplate);
        }
    }
}