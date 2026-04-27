using System.Collections.Generic;
using Melador.PlayerInput;
using R3;
using UnityEngine;
using Workshop47.Scripts.Game.Gameplay.Services;
using Workshop47.Scripts.Game.Settings.Gameplay.Entities.Player;
using Workshop47.Scripts.Game.State.Entities.Upgradeable.Player;

namespace Workshop47.Scripts.Game.Gameplay.View.Player
{
    public class PlayerViewModel
    {
        public readonly int EntityId;
        public readonly string ConfigId;
        public readonly string Name;

        public readonly PlayerInputProvider PlayerInputProvider;
        
        public ReadOnlyReactiveProperty<Vector3> Position => _position;
        public ReadOnlyReactiveProperty<Vector3> Rotation => _rotation;
        
        public ReadOnlyReactiveProperty<int> Level { get; }
        public ReadOnlyReactiveProperty<float> Health { get; }

        private readonly ReactiveProperty<Vector3> _position;
        private readonly ReactiveProperty<Vector3> _rotation;
        
        private readonly PlayerService _playerService;
        private readonly PlayerEntity _playerEntity;
        private readonly PlayerSettings _playerSettings;
        private readonly Dictionary<int, PlayerLevelSettings> _levelSettingsMap = new();
        
        public PlayerViewModel(PlayerEntity playerEntity, PlayerSettings playerSettings, 
            PlayerService playerService)
        {
            EntityId = playerEntity.UniqueId;
            ConfigId = playerEntity.ConfigId;
            Name = playerEntity.Name;
            
            Level = playerEntity.Level;
            _position = playerEntity.Position;
            _rotation = playerEntity.Rotation;
            Health = playerEntity.Health;
            
            _playerEntity = playerEntity;
            _playerSettings = playerSettings;
            _playerService = playerService;
            
            PlayerInputProvider = playerService.PlayerInputProvider;
            
            foreach (var playerLevelSettings in playerSettings.Levels)
            {
                _levelSettingsMap[playerLevelSettings.Level] = playerLevelSettings;
            }
        }

        public PlayerLevelSettings GetLevelSettings(int level)
        {
            return _levelSettingsMap[level];
        }

        public void OnPlayerMoved(Vector3 newPosition, Vector3 newRotation)
        {
            _position.Value = newPosition;
            _rotation.Value = newRotation;
        }
    }
}