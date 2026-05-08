using UnityEngine;

namespace Workshop47.Scripts.Game.Gameplay.View.Player
{
    public class PlayerBinder : MonoBehaviour
    {
        [SerializeField] private Game.Player.PlayerController _playerController;

        private PlayerViewModel _viewModel;
        
        public void Bind(PlayerViewModel viewModel)
        {
            _viewModel = viewModel;
            _playerController.Initialize(viewModel.InputContextManager, 
                viewModel.OnInteractablesOverlap, viewModel.OnSwitchRtsMode);
            
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