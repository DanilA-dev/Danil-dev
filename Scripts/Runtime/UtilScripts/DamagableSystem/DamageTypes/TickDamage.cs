using D_Dev.ValueSystem;
using UnityEngine;

namespace D_Dev.DamagableSystem.DamageTypes
{
    [System.Serializable]
    public class TickDamage : IDamage
    {
        [SerializeField] private FloatValue _duration;
        [SerializeField] private FloatValue _damage;
        
        public float ApplyDamage(ref FloatValue health)
        {
            health.Value -= _damage.Value;
            return _damage.Value;
        }
    }
}
