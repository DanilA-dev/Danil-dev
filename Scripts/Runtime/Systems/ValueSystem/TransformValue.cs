using System;
using D_Dev.ScriptableVaiables;
using D_Dev.ValueSystem.RandomMethods;
using UnityEngine;

namespace D_Dev.ValueSystem
{
    [Serializable]
    public class TransformValue : BaseValue<Transform, TransformScriptableVariable, ArrayRandomValueMethod<Transform>> {}
}