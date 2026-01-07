namespace D_Dev.DamageableSystem
{
    public interface IDamageable
    {
        public bool IsDamageable { get; }
        public void TakeDamage(DamageData damageData);
    }
}