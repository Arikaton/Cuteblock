using System;

namespace TweensStateMachine
{
    internal class Transition
    {
        public string To { get; }
        public Func<bool> Condition { get; }

        public Transition(string to, Func<bool> condition)
        {
            To = to;
            Condition = condition;
        }
    }
}