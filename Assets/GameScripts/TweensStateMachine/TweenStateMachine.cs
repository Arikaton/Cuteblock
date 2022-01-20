using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TweensStateMachine.TweenTemplates;

namespace TweensStateMachine
{
    public class TweenStateMachine
    {
        private List<State> _states;
        private Dictionary<string, List<Transition>> _transitions;
        private List<Transition> _currentTransitions;
        private List<Tween> _activeTweens;
        private State _currentState;
        private static readonly List<Transition> EmptyTransitions = new List<Transition>(0);

        public TweenStateMachine()
        {
            _states = new List<State>();
            _transitions = new Dictionary<string, List<Transition>>();
            _currentTransitions = new List<Transition>();
            _activeTweens = new List<Tween>();
        }

        public void Tick()
        {
            if (_currentState == null)
            {
                throw new InvalidOperationException(
                    "You need to set current state before using state machine. (Use SetState() before first Tick())");
            }
            var condition = GetTransition();
            if (condition != null)
                SetState(condition.To);
        }

        public void AddState(string name, params TweenTemplate[] tweenTemplates)
        {
            AddState(new State(name, tweenTemplates));
        }

        public void SetState(string stateName)
        {
            if (_states.All(x => x.StateName != stateName))
                throw new InvalidOperationException(
                    $"State {stateName} doesn't exist. You must add state before seting it active");
        
            var state = _states.Find(x => x.StateName == stateName);
            if(state == _currentState)
                return;

            foreach (var tween in _activeTweens)
                tween?.Kill();
            _activeTweens.Clear();
        
            _currentState = state;
            _currentTransitions = _transitions.TryGetValue(_currentState.StateName, out var transitions) ? transitions : EmptyTransitions;

            var tweenTemplates = _currentState.OnEnter();
            foreach (var tt in tweenTemplates)
            {
                _activeTweens.Add(tt.CreateTween());
            }
        }

        public void AddTransition(string from, string to, Func<bool> condition)
        {
            if (_states.All(x => x.StateName != from))
                throw new InvalidOperationException(
                    $"State {from} doesn't exist. You must add state before adding transition for it");
            if (_states.All(x => x.StateName != to))
                throw new InvalidOperationException(
                    $"State {to} doesn't exist. You must add state before adding transition for it");

            if (_transitions.TryGetValue(from, out List<Transition> transitions))
            {
                _transitions[from].Add(new Transition(to, condition));
            }
            else
            {
                _transitions[from] = new List<Transition>();
                _transitions[from].Add(new Transition(to, condition));
            }
        }

        private void AddState(State state)
        {
            if (_states.Any(x => x.StateName == state.StateName))
            {
                throw new InvalidOperationException(
                    $"State {state.StateName} was already added to the Animator. There can't be 2 states with the same name");
            }
            _states.Add(state);
        }

        private Transition GetTransition()
        {
            foreach (var transition in _currentTransitions)
            {
                if (transition.Condition())
                {
                    return transition;
                }
            }
            return null;
        }
    }
}