using Workshop47.Scripts.Game.State.Entities.Upgradeable.Damageable;

namespace Workshop47.Scripts.Game.State.Entities.Upgradeable.Characters
{
    public class CharacterEntity : DamageableEntity
    {
        public string Name => _origin.Name;
        
        private readonly CharacterEntityData _origin;
        
        public CharacterEntity(CharacterEntityData data) : base(data)
        {
            _origin = data;
        }
    }
}