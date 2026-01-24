using System;
using D_Dev.ScriptableVaiables;
using UnityEngine;

namespace D_Dev.PolymorphicValueSystem
{
    [System.Serializable]
    public abstract class GameObjectValue : PolymorphicValue<GameObject> { }

    [System.Serializable]
    public sealed class GameObjectConstantValue : GameObjectValue
    {
        #region Fields

        [SerializeField] private GameObject _value;

        #endregion

        #region Properties

        public override GameObject Value
        {
            get => _value;
            set => _value = value;
        }

        #endregion

        #region Cloning

        public override PolymorphicValue<GameObject> Clone()
        {
            return new GameObjectConstantValue { _value = _value };
        }

        #endregion
    }

    [System.Serializable]
    public sealed class GameObjectScriptableVariableValue : GameObjectValue
    {
        #region Fields

        [SerializeField] private GameObjectScriptableVariable _variable;

        #endregion

        #region Properties

        public override GameObject Value
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

        public GameObjectScriptableVariable Variable => _variable;

        #endregion

        #region Cloning

        public override PolymorphicValue<GameObject> Clone()
        {
            return new GameObjectScriptableVariableValue { _variable = _variable };
        }

        #endregion
    }
}
