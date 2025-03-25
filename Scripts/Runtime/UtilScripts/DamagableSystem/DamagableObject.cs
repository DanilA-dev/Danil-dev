using D_Dev.UtilScripts.ScriptableVaiables;
using D_Dev.UtilScripts.StatsSystem;
using D_Dev.UtilScripts.ValueSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace D_Dev.UtilScripts.DamagableSystem
{
    public class DamagableObject : MonoBehaviour, IDamagable
    {
        #region Fields

        [SerializeField] private StatsContainer _statsContainer;
        [SerializeField] private StringScriptableVariable _healthVariable;
        [Space]
        [FoldoutGroup("Events")]
        public UnityEvent<float> OnDamage;
        [FoldoutGroup("Events")]
        public UnityEvent<DamageInfo> OnDamageWithInfo;
        [FoldoutGroup("Events")]
        public UnityEvent OnDeath;

        private Stat _healthStat;
        private FloatValue _healthValue;
        private Rigidbody _rigidbody;
        
        #endregion
        
        #region Properties

        public bool CanBeDamaged { get; } = true;
        public bool IsDead => _healthValue != null && _healthValue.Value <= 0;
        public GameObject Damagable => this.gameObject;

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            _healthStat = _statsContainer.GetStat(_healthVariable);
            _healthValue = _healthStat.StatValue;
            _rigidbody = GetComponent<Rigidbody>();
        }

        #endregion
        
        #region Public

        public void Damage(DamageInfo damageInfo)
        {
            if(IsDead)
                return;
            
            var damageValue = damageInfo.Damage.ApplyDamage(ref _healthValue);
            if (damageInfo.UseForceOnDamage)
            {
                if (_rigidbody != null)
                {
                    Vector3 dir = Vector3.zero;
                    if(damageInfo.DamageDealer != null)
                        dir = (transform.position - damageInfo.DamageDealer.transform.position).normalized;
                    else
                        dir = transform.position;
                    
                    _rigidbody.AddForce(damageInfo.GetForceDirection(dir), damageInfo.ForceMode);
                }
                    
            }
            OnDamage?.Invoke(damageValue);
            OnDamageWithInfo?.Invoke(damageInfo);

            if (_healthValue.Value <= 0)
                OnDeath?.Invoke();
        }

        #endregion
        
       
    }
}