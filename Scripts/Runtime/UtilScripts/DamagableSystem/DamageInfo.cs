using Sirenix.OdinInspector;
using UnityEngine;                    

namespace D_Dev.UtilScripts.DamagableSystem
{
    [System.Serializable]
    public class DamageInfo
    {
        [field: SerializeField] public GameObject DamageDealer { get; set; }
        [field: SerializeReference] public IDamage Damage { get; set; }
        [field: SerializeReference] public bool UseForceOnDamage { get; set; }
        [field: SerializeReference, ShowIf(nameof(UseForceOnDamage))] public ForceMode ForceMode { get; set; }
        [field: SerializeReference, ShowIf(nameof(UseForceOnDamage))] public Vector3 ForceDirectionOffset { get; set; }
        
        public Vector3 GetForceDirection(Vector3 directionToTarget) =>
        new(directionToTarget.x + ForceDirectionOffset.x, directionToTarget.y + ForceDirectionOffset.y, directionToTarget.z + ForceDirectionOffset.z);
    }
}