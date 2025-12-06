namespace D_Dev.ValueSystem.RandomMethods
{
    [System.Serializable]
    public abstract class BaseRandomValueMethod<T>
    {
        public abstract T GetRandomValue();
    }
}