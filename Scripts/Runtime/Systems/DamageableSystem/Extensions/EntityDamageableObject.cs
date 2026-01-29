using D_Dev.DamageableSystem;
using D_Dev.PolymorphicValueSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace D_Dev.Scripts.DamageableSystem.Extensions
{
    public class EntityDamageableObject : BaseDamageableObject
    {
        #region Fields

        [Title("Entity Settings")]
        [SerializeReference] private PolymorphicValue<float> _currentHealthValue;
        [SerializeReference] private PolymorphicValue<float> _maxHealthValue;
            
        #endregion

        #region Properties

        public override float MaxHealth { get; protected set; }

        #endregion

        
        #region Overrides

        protected override void Init()
        {
            base.Init();
            MaxHealth = _maxHealthValue.Value;
        }

        public override void TakeDamage(DamageData damageData)
        {
            base.TakeDamage(damageData);
            _currentHealthValue.Value = CurrentHealth;
        }

        #endregion
    }
}