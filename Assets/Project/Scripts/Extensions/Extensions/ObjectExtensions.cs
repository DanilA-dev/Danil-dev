using UnityEngine;

namespace Project.Scripts.Extensions
{
    public static class ObjectExtensions
    {
        public static TComponent GetOrAdd<TComponent>(this GameObject go) where TComponent : Component
        {
            if (go.TryGetComponent(out TComponent comp))
                return comp;

            return go.AddComponent<TComponent>();
        }
    }
}