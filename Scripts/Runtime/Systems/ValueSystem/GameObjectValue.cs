using D_Dev.ScriptableVaiables;
using D_Dev.ValueSystem.RandomMethods;
using UnityEngine;

namespace D_Dev.ValueSystem
{
    [System.Serializable]
    public class GameObjectValue : BaseValue<GameObject, GameObjectScriptableVariable,ArrayRandomValueMethod<GameObject>> { }
}