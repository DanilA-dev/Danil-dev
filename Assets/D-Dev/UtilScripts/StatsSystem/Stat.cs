using D_Dev.UtilScripts.ScriptableVaiables;
using D_Dev.UtilScripts.ValueSystem;
using UnityEngine;

namespace D_Dev.UtilScripts.StatsSystem
{
    [System.Serializable]
    public class Stat
    {
        [field: SerializeField] public StringScriptableVariable StatName { get; private set; }
        [field: SerializeField] public FloatValue StatValue { get; private set; }
    }
}