using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace D_Dev.StateMachineBehaviour
{
    [System.Serializable]
    public class StateEvent
    {
        [field : SerializeField] public string State { get; private set; }
        [FoldoutGroup("Events")]
        public UnityEvent<string> OnStateEnter;
        [FoldoutGroup("Events")]
        public UnityEvent<string> OnStateExit;
    }
}
