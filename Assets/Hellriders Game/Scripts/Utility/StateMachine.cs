using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private IState _currentState;

    private Dictionary<Type, List<Transition>> _transitions = new Dictionary<Type, List<Transition>>(); //storage for all possible transition lines
    private List<Transition> _currentTransitions = new List<Transition>();
    private List<Transition> _anyTransitions = new List<Transition>(); // transitions that can be triggered from any state,
    private static List<Transition> EmptyTransitions = new List<Transition>();

    public IState GetCurrentState() { return _currentState; }

    public void Tick()
    {
        var transition = GetTransition();
        if (transition != null)
            SetState(transition.To);

        Debug.Log("StateMachine is currently ticking: " + _currentState);
        _currentState?.Tick();
    }

    public void SetState(IState state)
    {
        if (state == _currentState)
            return;

        _currentState?.OnExit(); //allow for clean up
        _currentState = state; // sets new state


        _transitions.TryGetValue(_currentState.GetType(), out _currentTransitions);
        if (_currentTransitions == null)
            _currentTransitions = EmptyTransitions;

        _currentState.OnEnter();

    }
    public void AddTransition(IState from, IState to, Func<bool> predicate)
    {
        if (_transitions.TryGetValue(from.GetType(), out var transitions) == false)
        {
            transitions = new List<Transition>();
            _transitions[from.GetType()] = transitions;
        }

        transitions.Add(new Transition(to, predicate));
    }

    public void AddAnyTransition(IState to, Func<bool> predicate)
    {
        _anyTransitions.Add(new Transition(to, predicate));
    }

    private class Transition
    {
        public Func<bool> Condition { get; }

        public IState To { get; }

        public Transition(IState to, Func<bool> condition)
        {
            To = to;
            Condition = condition;
        }
    }

    private Transition GetTransition()
    {
        foreach (var transition in _anyTransitions)
        {
            if (transition.Condition())
                return transition;
        }

        foreach (var transition in _currentTransitions)
        {
            if (transition.Condition())
                return transition;
        }
        return null;
    }
}
