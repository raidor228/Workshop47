using Melador.PlayerController.CameraController.Settings;
using UnityEngine;
using UnityEngine.InputSystem;
using Workshop47.Input;

namespace Workshop47.Scripts.Game.Gameplay.Input
{
    public class PlayerRtsCameraInput
    {
        public Vector2 MovementInput => _movementAction.ReadValue<Vector2>();
        public float ZoomInput => _zoomAction.ReadValue<Vector2>().y;
        public bool IsRotateLeftButtonPressed => _rotateLeftAction.ReadValue<float>() > 0;
        public bool IsRotateRightButtonPressed => _rotateRightAction.ReadValue<float>() > 0;

        public InputAction MovementAction => _movementAction;
        public InputAction ZoomAction => _zoomAction;
        public InputAction RotateLeftAction => _rotateLeftAction;
        public InputAction RotateRightAction => _rotateRightAction;
        
        private readonly InputAction _movementAction;
        private readonly InputAction _zoomAction;
        private readonly InputAction _rotateLeftAction;
        private readonly InputAction _rotateRightAction;
        
        private readonly PlayerInputActions _playerInputActions;

        public PlayerRtsCameraInput(PlayerInputActions playerInputActions, CameraSettings cameraSettings)
        {
            _playerInputActions = playerInputActions;
            
            _movementAction = _playerInputActions.FindAction(cameraSettings.RTSSettings.MovementAction.name);
            _zoomAction = _playerInputActions.FindAction(cameraSettings.RTSSettings.ZoomAction.name);
            _rotateLeftAction = _playerInputActions.FindAction(cameraSettings.RTSSettings.RotateLeftAction.name);
            _rotateRightAction = _playerInputActions.FindAction(cameraSettings.RTSSettings.RotateRightAction.name);
        }
        
        public void Enable()
        {
            _playerInputActions.RtsCamera.Enable();
        }

        public void Disable()
        {
            _playerInputActions.RtsCamera.Disable();
        }
    }
}