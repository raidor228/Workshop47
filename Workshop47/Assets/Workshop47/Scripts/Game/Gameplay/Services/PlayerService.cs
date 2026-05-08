using R3;
using UnityEngine;
using Workshop47.Scripts.Game.Gameplay.Input;
using Workshop47.Scripts.Game.Gameplay.View.Player;
using Workshop47.Scripts.Game.Settings.Gameplay.Entities.Player;
using Workshop47.Scripts.Game.State.Commands;
using Workshop47.Scripts.Game.State.Entities.Upgradeable.Player;

namespace Workshop47.Scripts.Game.Gameplay.Services
{
    public class PlayerService
    {
        public ReadOnlyReactiveProperty<PlayerViewModel> Player => _player;
        public readonly InputContextManager InputContextManager;
        
        private readonly ReactiveProperty<PlayerViewModel> _player = new();
        
        private readonly PlayerSettings _playerSettings;
        private readonly ICommandProcessor _cmd;
        
        public PlayerService(ReadOnlyReactiveProperty<PlayerEntity> playerEntity, 
            PlayerSettings playerSettings, InputContextManager inputContextManager, ICommandProcessor cmd)
        {
            _cmd = cmd;
            InputContextManager = inputContextManager;
            
            _playerSettings = playerSettings;
            CreatePlayerViewModel(playerEntity.CurrentValue);
        }

        public void TeleportPlayer(Vector3 position)
        {
            
        }

        public void SwitchRtsMode()
        {
            _player.CurrentValue.OnSwitchRtsMode.OnNext(Unit.Default);
        }
        
        private void CreatePlayerViewModel(PlayerEntity playerEntity)
        {
            var playerViewModel = new PlayerViewModel(playerEntity, _playerSettings, this);
            _player.Value = playerViewModel;
        }
    }
}