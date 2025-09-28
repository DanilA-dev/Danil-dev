﻿using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace D_dev.ValueUtils
{
    public class BoolInverterEvent : MonoBehaviour
    {
        #region Fields

        [SerializeField, ReadOnly] private bool _value;
        [Space]
        [FoldoutGroup("Events")]
        public UnityEvent OnTrue;
        [FoldoutGroup("Events")]
        public UnityEvent OnFalse;

        #endregion

        #region Public

        public void Invert()
        {
            _value = !_value;
            if (_value)
                OnTrue.Invoke();
            else
                OnFalse.Invoke();
        }

        #endregion
    }
}