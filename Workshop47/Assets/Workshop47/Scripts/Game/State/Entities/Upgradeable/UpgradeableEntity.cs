using R3;

namespace Workshop47.Scripts.Game.State.Entities.Upgradeable
{
    public abstract class UpgradeableEntity : Entity
    {
        public readonly ReactiveProperty<int> Level;

        protected UpgradeableEntity(UpgradeableEntityData data) : base(data)
        {
            Level = new ReactiveProperty<int>(data.Level);
            Level.Skip(1).Subscribe(newLevel => data.Level = newLevel);
        }
    }
}