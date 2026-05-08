using Melador.PlayerController.CameraController.Settings;
using UnityEngine;
using UnityEngine.InputSystem;
using Workshop47.Input;

namespace Workshop47.Scripts.Game.Gameplay.Input
{
    public class PlayerCameraInput
    {
        public Vector2 MouseInput => _mouseAction.ReadValue<Vector2>();
        public bool IsPerspectiveButtonPressed => _perspectiveAction.ReadValue<float>() > 0;
        public bool IsFreeLookButtonPressed => _freeLookAction.ReadValue<float>() > 0;
        
        public InputAction MouseAction => _mouseAction;
        public InputAction PerspectiveAction => _perspectiveAction;
        public InputAction FreeLookAction => _freeLookAction;
        
        private readonly InputAction _mouseAction;
        private readonly InputAction _perspectiveAction;
        private readonly InputAction _freeLookAction;
        
        private readonly PlayerInputActions _playerInputActions;

        public PlayerCameraInput(PlayerInputActions playerInputActions, CameraSettings cameraSettings)
        {
            _playerInputActions = playerInputActions;

            _mouseAction = _playerInputActions.FindAction(cameraSettings.GeneralSettings.MouseLookAction.name);
            _perspectiveAction = _playerInputActions.FindAction(cameraSettings.GeneralSettings.PerspectiveAction.name);
            _freeLookAction = _playerInputActions.FindAction(cameraSettings.FreeLookSettings.FreeLookAction.name);
        }
        
        public void Enable()
        {
            _playerInputActions.Camera.Enable();
        }

        public void Disable()
        {
            _playerInputActions.Camera.Disable();
        }
    }
}