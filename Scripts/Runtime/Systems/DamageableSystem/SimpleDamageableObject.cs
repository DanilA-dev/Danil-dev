using D_Dev.DamageableSystem;
using UnityEngine;

namespace D_Dev.Scripts.DamageableSystem.Types
{
    public class SimpleDamageableObject : BaseDamageableObject
    {
        #region Fields

        [SerializeField] private int _maxHealth;

        #endregion

        #region Properties

        public override int MaxHealth
        {
            get => _maxHealth;
            protected set => _maxHealth = value;
        }
        #endregion
    }
}