using System.Collections.Generic;
using System.Linq;
using D_Dev.Extensions;
using D_Dev.ScriptableVaiables;
using UnityEngine;

namespace D_Dev.StatsSystem
{
    public class StatsContainer : MonoBehaviour
    {
        #region Fields

        [SerializeField] private List<Stat> _stats;

        #endregion

        #region Properties

        public List<Stat> Stats => _stats;

        #endregion

        #region Monobehaviour

        private void Start()
        {
            if(_stats.Count <= 0)
                return;

            foreach (var stat in _stats)
                stat.Init();
        }

        private void OnDisable()
        {
            if(_stats.Count <= 0)
                return;

            foreach (var stat in _stats)
                stat.Dispose();
        }

        #endregion
        
        #region Public

        public void AddStat(Stat newStat)
        {
            if (!_stats.Contains(newStat))
            {
                newStat.Init();
                _stats.Add(newStat);
            }
        }

        public void SetStats(List<Stat> newStats)
        {
            _stats = newStats;
            foreach (var stat in _stats)
                stat.Init();
        }

        public void RemoveStat(Stat stat)
        {
            if(_stats.TryRemove(stat))
                stat.Dispose();
        }

        public void RemoveStat(StringScriptableVariable statName)
        {
            var stat = _stats.FirstOrDefault(s => s.StatID == statName);
            if(stat == null)
                return;
            
            if(_stats.TryRemove(stat))
                stat.Dispose();
        }

        public Stat GetStat(StringScriptableVariable statName)
        {
            var stat = _stats.FirstOrDefault(s => s.StatID == statName);
            return stat;
        }
        
        #endregion
    }
}