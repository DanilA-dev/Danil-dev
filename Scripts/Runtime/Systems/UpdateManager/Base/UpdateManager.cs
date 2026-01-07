using System.Collections.Generic;
using UnityEngine;

namespace D_Dev.UpdateManager
{
    public class UpdateManager : MonoBehaviour
    {
        #region Fields

        private static readonly Queue<ITickable> _tickables = new();
        private static readonly Queue<ITickable> _pendingAdd = new();
        private static readonly Queue<ITickable> _pendingRemove = new();
        
        private static readonly List<ITickable> _sortedTickables = new();
        private static bool _isSorted = true;

        #endregion

        #region Properties

        public static int Count => _tickables.Count + _pendingAdd.Count;

        #endregion

        #region Monobehavior

        private void Update()
        {
            ProcessPending();
            EnsureSorted();
            
            foreach (var tickable in _sortedTickables)
                tickable?.Tick();
        }

        #endregion

        #region Public

        public static void Add(ITickable tickable)
        {
            if (tickable != null)
                _pendingAdd.Enqueue(tickable);
        }
        
        public static void AddWithPriority(ITickable tickable, int priority)
        {
            if (tickable != null)
            {
                tickable.SetPriority(priority);
                Add(tickable);
            }
        }
        
        public static void Remove(ITickable tickable)
        {
            if (tickable != null)
                _pendingRemove.Enqueue(tickable);
        }
        
        public static void Clear()
        {
            _tickables.Clear();
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
                _tickables.Enqueue(tickable);
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
            if (_isSorted)
                return;
            
            _sortedTickables.Clear();
            _sortedTickables.AddRange(_tickables);
            _sortedTickables.Sort((a, b) => b.GetPriority().CompareTo(a.GetPriority()));
            _isSorted = true;
        }

        #endregion
    }
}
