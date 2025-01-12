using D_Dev.UtilScripts.ValueSystem;
using UnityEngine;

namespace D_Dev.UtilScripts.DamagableSystem.DamageTypes
{
    [System.Serializable]
    public class RandomDamage : IDamage
    {
        #region Fields

        [SerializeField] private FloatValue _minDamageAmount;
        [SerializeField] private FloatValue _maxDamageAmount;

        #endregion

        #region Properties

        public float ResultDamage => Random.Range(_minDamageAmount.Value, _maxDamageAmount.Value);

        #endregion
        
        #region Public

        public float ApplyDamage(ref FloatValue health)
        {
            health.Value -= ResultDamage;
            return ResultDamage;
        }

        #endregion
    }
}