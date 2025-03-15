using UnityEngine;                    

namespace D_Dev.UtilScripts.DamagableSystem
{
    [System.Serializable]
    public class DamageInfo
    {
        [field: SerializeField] public GameObject DamageDealer { get; set; }
        [field: SerializeReference] public IDamage Damage { get; set; }
    }
}