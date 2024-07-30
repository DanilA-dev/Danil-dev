using UnityEngine;

namespace Extensions
{
    public static class GameObjectExtensions
    {
        public static bool TryGetComponentInAny<T>(this GameObject gameObject, out T component)
        {
            if (gameObject.TryGetComponent(out component))
                return true;

            var root = gameObject.transform.root;
            if (root.TryGetComponent(out component))
                return true;

            var childInRoot = root.GetComponentInChildren<T>();
            if (childInRoot != null)
            {
                component = childInRoot;
                return true;
            }
            
            return false;
        }

        public static T GetOrAdd<T>(this GameObject go) where T : Component
        {
            if (go.TryGetComponent(out T comp))
                return comp;

            return go.AddComponent<T>();
        }
        
    }
}
