using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace D_Dev.Scripts.Runtime.UtilScripts.StateMachineBehaviour
{
    [System.Serializable]
    public class StateEvent<TStateEnum> where TStateEnum : Enum
    {
        [field : SerializeField] public TStateEnum State { get; private set; }
        [FoldoutGroup("Events")]
        public UnityEvent<TStateEnum> OnStateEnter;
        [FoldoutGroup("Events")]
        public UnityEvent<TStateEnum> OnStateExit;
    }
}