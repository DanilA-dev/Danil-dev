using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace D_Dev.DamageableSystem
{
    public abstract class BaseDamageableObject : MonoBehaviour, IDamageable
    {
        #region Fields

        [Title("Damageable Settings")] 
        [SerializeField] private bool _isDamageable = true;
        [PropertyOrder(100)]
        [FoldoutGroup("Events")]
        public UnityEvent<DamageData> OnDamage;
        [FoldoutGroup("Events")]
        [PropertyOrder(100)]
        public UnityEvent OnDeath;
        
        #endregion

        #region Properties

        public bool IsDamageable => _isDamageable;
        public abstract int MaxHealth { get; protected set; }
        public int CurrentHealth { get; protected set; }

        #endregion

        #region Monobehaviour

        private void Start() => Init();

        #endregion
        
        #region IDamageable

        public virtual void TakeDamage(DamageData damageData)
        {
            if(damageData.Damage <= 0)
                return;
            
            if(!_isDamageable)
                return;
            
            CurrentHealth -= damageData.Damage;
            if(CurrentHealth <= 0)
                OnDie();
        }

        #endregion

        #region Virtual
        protected virtual void Init() => CurrentHealth = MaxHealth;

        #endregion
        
        #region Private
        private void OnDie()
        {
            CurrentHealth = 0;
            OnDeath?.Invoke();
        }

        #endregion
    }
}