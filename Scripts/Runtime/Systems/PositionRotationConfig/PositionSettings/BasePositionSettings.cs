using System;
using UnityEngine;

namespace D_Dev.PositionRotationConfig
{
    [Serializable]
    public class BasePositionSettings
    {
        public virtual Vector3 GetPosition() => Vector3.zero;
    }
}