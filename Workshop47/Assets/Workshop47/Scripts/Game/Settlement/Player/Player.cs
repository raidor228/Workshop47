using System;
using UnityEngine;
using Workshop47.Input;
using R3;

namespace Workshop47.Scripts.Game.Settlement.Player
{
    public class Player : IDisposable
    {
        public ReadOnlyReactiveProperty<IControllable> ControllableCharacter => _controllableCharacter;

        private readonly PlayerInputActions _playerInputActions;
        private readonly ReactiveProperty<IControllable> _controllableCharacter;

        public Player(PlayerInputActions playerInputActions, IControllable characterViewModel = null)
        {
            _playerInputActions = playerInputActions;
            _controllableCharacter = new ReactiveProperty<IControllable>(characterViewModel);

            EnableInput(true);
        }

        public void EnableInput(bool enable)
        {
            if (enable)
            {
                _playerInputActions.Enable();
            }
            else
            {
                _playerInputActions.Disable();
            }
        }
        
        public void SetCharacter(IControllable newCharacter)
        {
            _controllableCharacter.Value = newCharacter;
        }

        public void Update()
        {
            HandleMovement();
        }
        
        private void HandleMovement()
        {
            if (_controllableCharacter.CurrentValue == null)
            {
                return;
            }

            Vector2 input = _playerInputActions.Movement.Movement.ReadValue<Vector2>();
            if (input == Vector2.zero)
            {
                return;
            }
            
            Vector3 direction = new Vector3(input.x, 0, input.y);
            _controllableCharacter.CurrentValue.Move(direction);
        }

        public void Dispose()
        {
            _playerInputActions?.Dispose();
            _controllableCharacter?.Dispose();
        }
    }
}