using System;
using System.Collections.Generic;
using ObservableCollections;
using Workshop47.Scripts.Game.Settings.Settlement.Entities;
using Workshop47.Scripts.Game.Settings.Settlement.Entities.Characters;
using Workshop47.Scripts.Game.Settlement.View.Characters;
using Workshop47.Scripts.Game.State.Commands;
using Workshop47.Scripts.Game.State.Entities;
using Workshop47.Scripts.Game.State.Entities.Upgradeable.Characters;
using Workshop47.Scripts.Game.Settlement.Commands;
using UnityEngine;
using R3;

namespace Workshop47.Scripts.Game.Settlement.Services
{
    public class CharactersService
    {
        public IObservableCollection<CharacterViewModel> AllCharacters => _allCharacters;
        
        private readonly ObservableList<CharacterViewModel> _allCharacters = new();
        private readonly Dictionary<int, CharacterViewModel> _charactersMap = new();
        private readonly Dictionary<string, CharacterSettings> _characterSettingsMap = new();
        private readonly ICommandProcessor _cmd;

        public CharactersService(IObservableCollection<Entity> entities, 
            EntitiesSettings entitiesSettings, ICommandProcessor cmd)
        {
            _cmd = cmd;
            
            foreach (var characterSettings in entitiesSettings.Characters)
            {
                _characterSettingsMap[characterSettings.ConfigId] = characterSettings;
            }

            foreach (var entity in entities)
            {
                if (entity is CharacterEntity characterEntity)
                {
                    CreateCharacterViewModel(characterEntity);
                }
            }

            entities.ObserveAdd().Subscribe(e =>
            {
                var entity = e.Value;
                if (entity is CharacterEntity characterEntity)
                {
                    CreateCharacterViewModel(characterEntity);
                }
            });

            entities.ObserveRemove().Subscribe(e =>
            {
                var entity = e.Value;

                if (entity is CharacterEntity characterEntity)
                {
                    RemoveCharacterViewModel(characterEntity);
                }
            });
        }

        public bool PlaceCharacter(string characterConfigId, Vector3 position, Vector3 rotation)
        {
            var command = new CmdPlaceEntity(EntityType.Character, characterConfigId, position, rotation);
            var result = _cmd.Process(command);

            return result;
        }
        
        public bool MoveCharacter(int characterEntityId, Vector3 newPosition)
        {
            throw new NotImplementedException();
        }

        public bool DeleteCharacter(int characterEntityId)
        {
            throw new NotImplementedException();
        }

        private void CreateCharacterViewModel(CharacterEntity characterEntity)
        {
            var characterSettings = _characterSettingsMap[characterEntity.ConfigId];
            var characterViewModel = new CharacterViewModel(characterEntity, characterSettings, this);
            
            _allCharacters.Add(characterViewModel);
            _charactersMap[characterEntity.UniqueId] = characterViewModel;
        }

        private void RemoveCharacterViewModel(CharacterEntity characterEntity)
        {
            if (_charactersMap.TryGetValue(characterEntity.UniqueId, out var characterViewModel))
            {
                _allCharacters.Remove(characterViewModel);
                _charactersMap.Remove(characterEntity.UniqueId);
            }
        }
    }
}