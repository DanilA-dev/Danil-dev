using D_Dev.ValueSystem;

namespace D_Dev.DamagableSystem
{
    public interface IDamage
    {
        public float ApplyDamage(ref FloatValue health);
    }
}
