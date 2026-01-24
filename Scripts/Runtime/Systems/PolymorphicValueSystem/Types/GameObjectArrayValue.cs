using System;
using D_Dev.ScriptableVaiables;
using UnityEngine;

namespace D_Dev.PolymorphicValueSystem
{
    [System.Serializable]
    public abstract class GameObjectArrayValue : PolymorphicValue<GameObject[]> { }

    [System.Serializable]
    public sealed class GameObjectArrayConstantValue : GameObjectArrayValue
    {
        #region Fields

        [SerializeField] private GameObject[] _value;

        #endregion

        #region Properties

        public override GameObject[] Value
        {
            get => _value;
            set => _value = value;
        }

        #endregion

        #region Cloning

        public override PolymorphicValue<GameObject[]> Clone()
        {
            return new GameObjectArrayConstantValue { _value = _value };
        }

        #endregion
    }

    [System.Serializable]
    public sealed class GameObjectArrayScriptableVariableValue : GameObjectArrayValue
    {
        #region Fields

        [SerializeField] private GameObjectArrayScriptableVariable _variable;

        #endregion

        #region Properties

        public override GameObject[] Value
        {
            get
            {
                return _variable != null ? _variable.Value : default;
            }
            set
            {
                if (_variable != null)
                    _variable.Value = value;
            }
        }

        public GameObjectArrayScriptableVariable Variable => _variable;

        #endregion

        #region Cloning

        public override PolymorphicValue<GameObject[]> Clone()
        {
            return new GameObjectArrayScriptableVariableValue { _variable = _variable };
        }

        #endregion
    }
}
