using System;
using System.Collections.Generic;
using CustomFSM.Preicate;
using CustomFSM.State;
using CustomFSM.Transition;

namespace CustomFSM.StateMachine
{
    public partial class StateMachine
    {
        private StateNode _currentStateNode;
        private Dictionary<Type, StateNode> _stateDic = new Dictionary<Type, StateNode>();
        private HashSet<ITransition> _anyTransitions = new HashSet<ITransition>();

        public void AddTransition(IState from, IState to, IPredicate condition)
        {
            GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
        }
        
        public void AddAnyTransition(IState to, IPredicate condition)
        {
            _anyTransitions.Add(new Transition.Transition(GetOrAddNode(to).State, condition));
        }
        
        public void OnUpdate()
        {
            ITransition transition = GetTransition();
            if (transition != null)
                ChangeState(transition.To);
            
            _currentStateNode?.State.OnUpdate();
        }

        public void OnFixedUpdate()
        {
            _currentStateNode?.State.OnFixedUpdate();
        }
        
        
        public void SetState(IState state)
        {
            _currentStateNode = _stateDic[state.GetType()];
            _currentStateNode?.State.OnEnter();
        }

        private void ChangeState(IState state)
        {
            if(_currentStateNode.State == state)
                return;

            var prevState = _currentStateNode.State;
            var nextState = _stateDic[state.GetType()];
            prevState?.OnExit();
            nextState.State?.OnEnter();
            _currentStateNode = nextState;
        }

        private ITransition GetTransition()
        {
            foreach (var transition in _anyTransitions)
            {
                if (transition.Condition.Evaluate())
                    return transition;
            }
            
            foreach (var transition in _currentStateNode.Transitions)
            {
                if (transition.Condition.Evaluate())
                    return transition;
            }

            return null;
        }

        

        private StateNode GetOrAddNode(IState state)
        {
            var statenode = _stateDic.GetValueOrDefault(state.GetType());
            if (statenode == null)
            {
                statenode = new StateNode(state);
                _stateDic.Add(state.GetType(), statenode);
            }

            return statenode;
        }

    }
}
