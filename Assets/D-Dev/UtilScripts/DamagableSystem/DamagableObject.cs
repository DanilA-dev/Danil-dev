using UnityEngine;

namespace D_Dev.UtilScripts.DamagableSystem
{
    public class DamagableObject : MonoBehaviour, IDamagable
    {
        #region Properties

        public bool CanBeDamaged { get; } = true;
        public bool IsDead { get; }
        public GameObject Damagable => this.gameObject;

        #endregion

        #region Public

        public void Damage(DamageInfo damageInfo)
        {
            
        }

        #endregion
        
       
    }
}