using System;
using System.Linq;
using D_Dev.Base;
using D_Dev.PolymorphicValueSystem;
using D_Dev.StateMachine;
using UnityEngine;

namespace D_Dev.StateMachineBehaviour
{
    public abstract class BaseComponentState : MonoBehaviour, IState
    {
        #region Classes

        [Serializable]
        public class TransitionData
        {
            #region Fields

            [SerializeField] private PolymorphicValue<string>[] _fromStates;
            [SerializeReference] private ICondition[] _conditions;

            #endregion

            #region Properties

            public string[] FromStates => _fromStates.Select(x => x.Value).ToArray();

            public ICondition[] Conditions => _conditions;

            #endregion
        }

        #endregion

        #region Fields

        [SerializeField] private PolymorphicValue<string> _stateName;
        [SerializeField] private TransitionData[] _transitions;

        #endregion
        
        #region Properties

        public float ExitTime { get; }
        public string StateName => _stateName.Value;
        public TransitionData[] Transitions => _transitions;

        #endregion

        #region IState

        public virtual void OnEnter(){}

        public virtual void OnUpdate(){}

        public virtual void OnFixedUpdate(){}

        public virtual void OnExit(){}


        #endregion
    }
}