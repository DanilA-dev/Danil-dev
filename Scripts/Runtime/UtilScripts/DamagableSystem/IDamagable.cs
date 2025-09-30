using UnityEngine;

namespace D_Dev.DamagableSystem
{
    public interface IDamagable
    {
        public bool CanBeDamaged { get; }
        public bool IsDead { get; }
        public GameObject Damagable { get; }
        public void Damage(DamageInfo damageInfo);
    }
}
