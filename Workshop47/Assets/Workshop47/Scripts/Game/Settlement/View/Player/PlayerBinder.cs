using System;
using UnityEngine;
using Workshop47.Scripts.Game.Settlement.View.Player;

namespace Workshop47.Scripts.Game.Player
{
    public class PlayerBinder : MonoBehaviour
    {
        [SerializeField] private PlayerController _playerController;

        private PlayerViewModel _viewModel;
        
        public void Bind(PlayerViewModel viewModel)
        {
            _viewModel = viewModel;
            _playerController.Initialize(viewModel.PlayerInputProvider);
            
            _playerController.transform.position = _viewModel.Position.CurrentValue;
            _playerController.transform.rotation = Quaternion.Euler(_viewModel.Rotation.CurrentValue);
        }

        private void Update()
        {
            _viewModel.OnPlayerMoved(_playerController.transform.position, 
                _playerController.transform.rotation.eulerAngles);
        }
    }
}