using D_Dev.DamageableSystem;
using D_Dev.EntityVariable;
using D_Dev.EntityVariable.Types;
using D_Dev.ScriptableVaiables;
using Sirenix.OdinInspector;
using UnityEngine;

namespace D_Dev.Scripts.DamageableSystem.Extensions
{
    public class EntityDamageableObject : BaseDamageableObject
    {
        #region Fields

        [Title("Entity Settings")]
        [SerializeField] private StringScriptableVariable _currentHealthVariableID;
        [SerializeField] private StringScriptableVariable _maxHealthVariableID;
        [SerializeField] private RuntimeEntityVariablesContainer _runtimeEntityVariablesContainer;

        private IntEntityVariable _currentHealthVariable;
        private IntEntityVariable _maxHealthVariable;
            
        #endregion

        #region Properties

        public override int MaxHealth { get; protected set; }

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            _runtimeEntityVariablesContainer.OnInitialized += OnVariablesInitialized;
        }

        private void OnDestroy()
        {
            _runtimeEntityVariablesContainer.OnInitialized -= OnVariablesInitialized;
        }


        #endregion

        #region Listeners

        private void OnVariablesInitialized()
        {
            _maxHealthVariable = _runtimeEntityVariablesContainer.GetVariable<IntEntityVariable>(_maxHealthVariableID);
            _currentHealthVariable = _runtimeEntityVariablesContainer.GetVariable<IntEntityVariable>(_currentHealthVariableID);
            
            if(_maxHealthVariable != null)
                MaxHealth = _maxHealthVariable.Value;
           
            Init();
        }

        #endregion
        
        #region Overrides

        public override void TakeDamage(DamageData damageData)
        {
            base.TakeDamage(damageData);
            
            if(_currentHealthVariable != null)
                _currentHealthVariable.Value = CurrentHealth;
        }

        #endregion
    }
}