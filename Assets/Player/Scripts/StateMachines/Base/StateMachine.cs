using System;
using System.Collections.Generic;
using Player.Scripts.StateMachines.Base;

namespace Player.StateMachines.Base
{
    public class StateMachine
    {
        public StateNode CurrentState;
        Dictionary<Type, StateNode> nodes = new();
        HashSet<ITransition> anyTransitions = new ();

        public void Update()
        {
            var transition = GetTransition();
            if(transition != null)
                ChangeState(transition.To);

            CurrentState.State?.OnUpdate();
        }

        public void FixedUpdate()
        {
            CurrentState.State?.OnFixedUpdate();
        }

        public void SetState(IState state)
        {
            CurrentState = nodes[state.GetType()];
            CurrentState.State?.OnEnter();
        }

        private void ChangeState(IState state)
        {
            if(state == CurrentState.State) return;
            
            var previousState = CurrentState.State;
            var newState = nodes[state.GetType()].State;
            
            previousState?.OnExit();
            newState?.OnEnter();
            CurrentState = nodes[state.GetType()];
        }

        private ITransition GetTransition()
        {
            foreach(var transition in anyTransitions)
                if(transition.Condition.Evaluate())
                    return transition;
            
            foreach(var transition in CurrentState.Transitions)
                if(transition.Condition.Evaluate())
                    return transition;
            
            return null;
        }

        public void AddTransition(IState from, IState to, IPredicate condition)
        {
            GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
        }

        public void AddAnyTransition(IState to, IPredicate condition)
        {
            anyTransitions.Add(new Transition(GetOrAddNode(to).State, condition));
        }

        private StateNode GetOrAddNode(IState state)
        {
            var node = nodes.GetValueOrDefault(state.GetType());
            if (node == null)
            {
                node = new StateNode(state);
                nodes.Add(state.GetType(), node);
            }
            
            return node;
        }

        public class StateNode
        {
            public IState State { get; }
            
            public HashSet<ITransition> Transitions { get; }

            public StateNode(IState state)
            {
                State = state;
                Transitions = new HashSet<ITransition>();
            }

            public void AddTransition(IState to, IPredicate condition)
            {
                Transitions.Add(new Transition(to, condition));
            }
        }
    }
}