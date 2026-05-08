using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Workshop47.Input;
using PlayerSettings = Melador.PlayerController.MovementController.Settings.PlayerSettings;

namespace Workshop47.Scripts.Game.Gameplay.Input
{
    public class PlayerMovementInput
    {
        public Vector2 MovementInput => _movementAction.ReadValue<Vector2>();
        public bool IsJumpButtonPressed => _jumpAction.ReadValue<float>() > 0;
        public bool IsCrouchButtonPressed => _crouchAction.ReadValue<float>() > 0;
        public bool IsSlideButtonPressed => _slideAction.ReadValue<float>() > 0;
        public bool IsCrawlButtonPressed => _crawlAction.ReadValue<float>() > 0;
        public bool IsWalkButtonPressed => _walkAction.ReadValue<float>() > 0;
        
        public bool IsSprintButtonPressed { get; private set; }
        
        public event Action OnRollButtonPressed;
        
        public InputAction MovementAction => _movementAction;
        public InputAction JumpAction => _jumpAction;
        public InputAction CrouchAction => _crouchAction;
        public InputAction SlideAction => _slideAction;
        public InputAction CrawlAction => _crawlAction;
        public InputAction WalkAction => _walkAction;
        public InputAction SprintAction => _sprintAction;
        public InputAction RollAction => _rollAction;
        
        private readonly InputAction _movementAction;
        private readonly InputAction _jumpAction;
        private readonly InputAction _crouchAction;
        private readonly InputAction _slideAction;
        private readonly InputAction _crawlAction;
        private readonly InputAction _walkAction;
        private readonly InputAction _sprintAction;
        private readonly InputAction _rollAction;

        private readonly PlayerInputActions _playerInputActions;
        
        public PlayerMovementInput(PlayerInputActions playerInputActions, PlayerSettings playerSettings)
        {
            _playerInputActions = playerInputActions;
            
            _movementAction = _playerInputActions.FindAction(playerSettings.GeneralSettings.MovementAction.name);
            _jumpAction = _playerInputActions.FindAction(playerSettings.JumpSettings.JumpAction.name);
            _crouchAction = _playerInputActions.FindAction(playerSettings.CrouchSettings.CrouchAction.name);
            _slideAction = _playerInputActions.FindAction(playerSettings.SlideSettings.SlideAction.name);
            _crawlAction = _playerInputActions.FindAction(playerSettings.CrawlSettings.CrawlAction.name);
            _walkAction = _playerInputActions.FindAction(playerSettings.WalkSettings.WalkAction.name);
            _sprintAction = _playerInputActions.FindAction(playerSettings.SprintSettings.SprintAction.name);
            _rollAction = _playerInputActions.FindAction(playerSettings.RollSettings.RollAction.name);
        }
        
        public void Enable()
        {
            _playerInputActions.Movement.Enable();
            
            _sprintAction.performed += OnSprintPerformed;
            _sprintAction.canceled += OnSprintCanceled;
            
            _rollAction.performed += OnRollPerformed;
        }

        public void Disable()
        {
            _playerInputActions.Movement.Disable();
            
            _sprintAction.performed -= OnSprintPerformed;
            _sprintAction.canceled -= OnSprintCanceled;
            
            _rollAction.performed -= OnRollPerformed;
        }
        
        private void OnSprintPerformed(InputAction.CallbackContext context)
        {
            IsSprintButtonPressed = true;
        }
        
        private void OnSprintCanceled(InputAction.CallbackContext context)
        {
            IsSprintButtonPressed = false;
        }
        
        private void OnRollPerformed(InputAction.CallbackContext context)
        {
            OnRollButtonPressed?.Invoke();
        }
    }
}