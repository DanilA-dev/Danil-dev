using System.Collections.Generic;
using UnityEngine;

namespace D_Dev.UpdateManager
{
    public class FixedUpdateManager : MonoBehaviour
    {
        #region Fields

        private static readonly Queue<IFixedTickable> _fixedTickables = new();
        private static readonly Queue<IFixedTickable> _pendingAdd = new();
        private static readonly Queue<IFixedTickable> _pendingRemove = new();
        
        private static readonly List<IFixedTickable> _sortedTickables = new();
        private static bool _isSorted = true;

        #endregion

        #region Properties

        public static int Count => _fixedTickables.Count + _pendingAdd.Count;

        #endregion

        #region Monobehavior

        private void FixedUpdate()
        {
            ProcessPending();
            EnsureSorted();
            
            foreach (var tickable in _sortedTickables)
            {
                tickable?.FixedTick();
            }
        }

        #endregion

        #region Public

        public static void Add(IFixedTickable tickable)
        {
            if (tickable != null)
                _pendingAdd.Enqueue(tickable);
        }
        
        public static void AddWithPriority(IFixedTickable tickable, int priority)
        {
            if (tickable != null)
            {
                tickable.SetPriority(priority);
                Add(tickable);
            }
        }
        
        public static void Remove(IFixedTickable tickable)
        {
            if (tickable != null)
                _pendingRemove.Enqueue(tickable);
        }
        
        public static void Clear()
        {
            _fixedTickables.Clear();
            _pendingAdd.Clear();
            _pendingRemove.Clear();
            _sortedTickables.Clear();
            _isSorted = true;
        }

        #endregion

        #region Private

        private static void ProcessPending()
        {
            while (_pendingAdd.Count > 0)
            {
                var tickable = _pendingAdd.Dequeue();
                _fixedTickables.Enqueue(tickable);
                _isSorted = false;
            }
            
            while (_pendingRemove.Count > 0)
            {
                var tickable = _pendingRemove.Dequeue();
                _sortedTickables.Remove(tickable);
            }
        }
        
        private static void EnsureSorted()
        {
            if (_isSorted) return;
            
            _sortedTickables.Clear();
            _sortedTickables.AddRange(_fixedTickables);
            _sortedTickables.Sort((a, b) => b.GetPriority().CompareTo(a.GetPriority()));
            _isSorted = true;
        }

        #endregion
    }
}
