using System.Collections.Generic;
using System.Linq;
using D_Dev.UtilScripts.Extensions;
using D_Dev.UtilScripts.ScriptableVaiables;
using UnityEngine;

namespace D_Dev.UtilScripts.StatsSystem
{
    public class StatsContainer : MonoBehaviour
    {
        #region Fields

        [SerializeField] private List<Stat> _stats;

        #endregion

        #region Properties

        public List<Stat> Stats => _stats;

        #endregion

        #region Public

        public void AddStat(Stat newStat)
        {
            if(!_stats.Contains(newStat))
                _stats.Add(newStat);
        }

        public void SetStats(List<Stat> newStats) => _stats = newStats;
        
        public void RemoveStat(Stat stat)
        {
            _stats.TryRemove(stat);
        }

        public void RemoveStat(StringScriptableVariable statName)
        {
            var stat = _stats.FirstOrDefault(s => s.StatName == statName);
            if(stat == null)
                return;
            
            _stats.TryRemove(stat);
        }

        public Stat GetStat(StringScriptableVariable statName)
        {
            var stat = _stats.FirstOrDefault(s => s.StatName == statName);
            return stat;
        }
        
        #endregion
    }
}