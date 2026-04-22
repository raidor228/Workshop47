using R3;

namespace Workshop47.Scripts.Game.State.Entities.Upgradeable.Damageable
{
    public class DamageableEntity : UpgradeableEntity
    {
        public readonly ReactiveProperty<float> Health;
        
        public DamageableEntity(DamageableEntityData data) : base(data)
        {
            Health = new ReactiveProperty<float>(data.Health);
            Health.Skip(1).Subscribe(newHealth => data.Health = newHealth);
        }
    }
}