using UnityEngine;

namespace D_Dev.UtilScripts.DamagableSystem
{
    public class DamageInfo
    {
        public GameObject DamageDealer { get; set; }
        public IDamage Damage { get; set; }
        
        public DamageInfo(GameObject damageDealer, IDamage damage)
        {
            DamageDealer = damageDealer;
            Damage = damage;
        }

    }
}