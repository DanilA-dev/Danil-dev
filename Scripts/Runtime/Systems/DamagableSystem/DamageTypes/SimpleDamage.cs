using D_Dev.ValueSystem;
using UnityEngine;

namespace D_Dev.DamagableSystem.DamageTypes
{
    [System.Serializable]
    public class SimpleDamage : IDamage
    {
        #region Fields

        [SerializeField] private FloatValue _damageAmount;

        #endregion

        #region Public

        public float ApplyDamage(ref FloatValue health)
        {
            health.Value -= _damageAmount.Value;
            return _damageAmount.Value;
        }

        #endregion
    }
}
