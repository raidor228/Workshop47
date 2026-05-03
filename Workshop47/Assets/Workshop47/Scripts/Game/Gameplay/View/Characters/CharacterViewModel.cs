using System.Collections.Generic;
using R3;
using UnityEngine;
using Workshop47.Scripts.Game.Gameplay.Services;
using Workshop47.Scripts.Game.Gameplay.Services.Events;
using Workshop47.Scripts.Game.Settings.Gameplay.Entities.Characters;
using Workshop47.Scripts.Game.State.Entities.Upgradeable.Characters;

namespace Workshop47.Scripts.Game.Gameplay.View.Characters
{
    public class CharacterViewModel
    {
        public readonly int EntityId;
        public readonly string ConfigId;
        public readonly string Name;

        public float Speed => _characterSettings.Speed;
        
        public ReadOnlyReactiveProperty<Vector3> Position { get; }
        public ReadOnlyReactiveProperty<Vector3> Rotation { get; }
        public ReadOnlyReactiveProperty<int> Level { get; }
        public ReadOnlyReactiveProperty<float> Health { get; }
        
        public readonly Subject<Vector3> Moved = new();
        
        private readonly CharacterEntity _characterEntity;
        private readonly CharacterSettings _characterSettings;
        private readonly CharactersService _charactersService;
        private readonly Dictionary<int, CharacterLevelSettings> _levelSettingsMap = new();
        
        public CharacterViewModel(CharacterEntity characterEntity, 
            CharacterSettings characterSettings, CharactersService charactersService)
        {
            EntityId = characterEntity.UniqueId;
            ConfigId = characterEntity.ConfigId;
            Name = characterEntity.Name;
            
            Level = characterEntity.Level;
            Position = characterEntity.Position;
            Rotation = characterEntity.Rotation;
            Health = characterEntity.Health;
            
            _characterEntity = characterEntity;
            _characterSettings = characterSettings;
            _charactersService = charactersService;
            
            foreach (var characterLevelSettings in characterSettings.Levels)
            {
                _levelSettingsMap[characterLevelSettings.Level] = characterLevelSettings;
            }
            
            Moved.Subscribe(OnMoved);
        }

        public void OnRequestInteract()
        {
            var interactionEvent = new InteractionEvent(Name, EntityId);
            EventsHandler.Send(interactionEvent);
        }
        
        public CharacterLevelSettings GetLevelSettings(int level)
        {
            return _levelSettingsMap[level];
        }

        private void OnMoved(Vector3 newPosition)
        {
            //_charactersService.MoveCharacter(EntityId, newPosition);
        }
    }
}