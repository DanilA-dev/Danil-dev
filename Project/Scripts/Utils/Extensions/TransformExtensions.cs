using UnityEngine;

namespace Extensions
{
    public static class TransformExtensions
    {
        public static void RotateTowardsDirection(this Transform owner, Vector3 target, Vector3 axis, float speed)
        {
            var dir = (target - owner.position);
            Quaternion look = Quaternion.LookRotation(dir, axis);
            owner.transform.rotation = Quaternion.RotateTowards(owner.transform.rotation, look, speed);
            
        }
        
        public static void RotateTowards(this Transform owner, Vector3 target, Vector3 axis, float speed)
        {
            Quaternion look = Quaternion.LookRotation(target, axis);
            owner.transform.rotation = Quaternion.RotateTowards(owner.transform.rotation, look, speed);
            
        }
    }
}