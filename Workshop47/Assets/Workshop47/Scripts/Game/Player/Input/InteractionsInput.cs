using Melador.PlayerController.CameraController.Settings;
using UnityEngine;
using UnityEngine.InputSystem;
using Workshop47.Input;
using Workshop47.Scripts.Game.Player.InteractionSystem.Settings;

namespace Melador.PlayerInput.Modules
{
    public class InteractionsInput
    {
        public bool IsInteractButtonPressed => _interactAction.ReadValue<float>() > 0;
        public bool IsNextInteractionButtonPressed => _nextInteractionAction.ReadValue<float>() > 0;
        public bool IsPreviousInteractionButtonPressed => _previousInteractionAction.ReadValue<float>() > 0;
        
        public InputAction InteractAction => _interactAction;
        public InputAction NextInteractionAction => _nextInteractionAction;
        public InputAction PreviousInteractionAction => _previousInteractionAction;
        
        private readonly InputAction _interactAction;
        private readonly InputAction _nextInteractionAction;
        private readonly InputAction _previousInteractionAction;
        
        private readonly PlayerInputActions _playerInputActions;

        public InteractionsInput(PlayerInputActions playerInputActions, InteractionsSettings interactionsSettings)
        {
            _playerInputActions = playerInputActions;

            _interactAction = _playerInputActions.FindAction(interactionsSettings.InteractAction.name);
            _nextInteractionAction = _playerInputActions.FindAction(interactionsSettings.NextInteractionAction.name);
            _previousInteractionAction = _playerInputActions.FindAction(interactionsSettings.PreviousInteractionAction.name);
        }
        
        public void Enable()
        {
            _playerInputActions.Interactions.Enable();
        }

        public void Disable()
        {
            _playerInputActions.Interactions.Disable();
        }
    }
}