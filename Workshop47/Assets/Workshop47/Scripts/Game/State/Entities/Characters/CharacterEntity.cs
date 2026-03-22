using R3;

namespace Workshop47.Scripts.Game.State.Entities.Characters
{
    public class CharacterEntity : Entity
    {
        public string Name => _origin.Name;
        
        public readonly ReactiveProperty<int> Level;
        
        private readonly CharacterEntityData _origin;
        
        public CharacterEntity(CharacterEntityData data) : base(data)
        {
            _origin = data;

            Level = new ReactiveProperty<int>(data.Level);
            Level.Skip(1).Subscribe(newLevel => data.Level = newLevel);
        }
    }
}