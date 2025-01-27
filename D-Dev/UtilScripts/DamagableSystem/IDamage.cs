using D_Dev.UtilScripts.ValueSystem;

namespace D_Dev.UtilScripts.DamagableSystem
{
    public interface IDamage
    {
        public float ApplyDamage(ref FloatValue health);
    }
}