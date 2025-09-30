using D_Dev.ColliderEvents;
using D_Dev.DamagableSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace D_Dev.DamageCollider
{
    public class DamageCollider : MonoBehaviour
    {
        #region Fields

        [SerializeReference] private DamageInfo _damageInfo;
        [SerializeField] private BaseColliderEvents _colliderEvents;
        [FoldoutGroup("Events")] 
        public UnityEvent<DamageInfo> OnDamaged;

        #endregion

        #region Properties

        public BaseColliderEvents ColliderEvents
        {
            get => _colliderEvents;
            set => _colliderEvents = value;
        }

        public DamageInfo DamageInfo
        {
            get => _damageInfo;
            set => _damageInfo = value;
        }

        #endregion

        #region Monobehaviour

        private void OnEnable() => _colliderEvents.OnEnter.AddListener(ApplyDamage);
        private void OnDisable() => _colliderEvents.OnEnter.RemoveListener(ApplyDamage);

        #endregion

        #region Listeners
        private void ApplyDamage(Collider coll)
        {
            if (coll.TryGetComponent(out IDamagable damagable) 
                && damagable.CanBeDamaged)
            {
                damagable.Damage(DamageInfo);
                OnDamaged?.Invoke(DamageInfo);
            }
        }

        #endregion
    }
}
