using System.Collections.Generic;
using Workshop47.Scripts.Game.State.Entities.Upgradeable.Characters;
using Workshop47.Scripts.Game.Settings.Settlement.Entities.Characters;
using UnityEngine;
using R3;
using Workshop47.Scripts.Game.Settlement.Services;

namespace Workshop47.Scripts.Game.Settlement.View.Characters
{
    public class CharacterViewModel
    {
        private readonly CharacterEntity _characterEntity;
        private readonly CharacterSettings _characterSettings;
        private readonly CharactersService _charactersService;
        private readonly Dictionary<int, CharacterLevelSettings> _levelSettingsMap = new();
        
        public readonly int EntityId;
        public readonly string ConfigId;
        
        public ReadOnlyReactiveProperty<Vector3> Position { get; }
        public ReadOnlyReactiveProperty<Vector3> Rotation { get; }
        public ReadOnlyReactiveProperty<int> Level { get; }
        
        public CharacterViewModel(CharacterEntity characterEntity, 
            CharacterSettings characterSettings, CharactersService charactersService)
        {
            ConfigId = characterEntity.ConfigId;
            EntityId = characterEntity.UniqueId;
            Level = characterEntity.Level;
        
            _characterEntity = characterEntity;
            _characterSettings = characterSettings;
            _charactersService = charactersService;
            
            foreach (var buildingLevelSettings in characterSettings.Levels)
            {
                _levelSettingsMap[buildingLevelSettings.Level] = buildingLevelSettings;
            }
        
            Position = characterEntity.Position;
            Rotation = characterEntity.Rotation;
        }

        public CharacterLevelSettings GetLevelSettings(int level)
        {
            return _levelSettingsMap[level];
        }
    }
}