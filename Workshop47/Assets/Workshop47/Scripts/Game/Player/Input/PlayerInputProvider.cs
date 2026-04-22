using System;
using Melador.PlayerController.CameraController.Settings;
using Melador.PlayerController.MovementController.Settings;
using Melador.PlayerInput.Modules;
using UnityEngine;
using Workshop47.Input;

namespace Melador.PlayerInput
{
    public class PlayerInputProvider : IDisposable
    {
        public readonly PlayerMovementInput MovementInput;
        public readonly PlayerCameraInput CameraInput;
        
        private readonly PlayerInputActions _playerInputActions;

        public PlayerInputProvider(PlayerSettings playerSettings, CameraSettings cameraSettings)
        {
            _playerInputActions = new PlayerInputActions();

            MovementInput = new PlayerMovementInput(_playerInputActions, playerSettings);
            CameraInput = new PlayerCameraInput(_playerInputActions, cameraSettings);
        }

        public void ShowCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void HideCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        public void Disable(InputModuleType inputModules)
        {
            if ((inputModules & InputModuleType.Movement) != 0)
            {
                MovementInput.Disable();
            }
            if ((inputModules & InputModuleType.Camera) != 0)
            {
                CameraInput.Disable();
            }
        }
        
        public void Enable(InputModuleType inputModules)
        {
            if ((inputModules & InputModuleType.Movement) != 0)
            {
                MovementInput.Enable();
            }
            if ((inputModules & InputModuleType.Camera) != 0)
            {
                CameraInput.Enable();
            }
        }
        
        public void Dispose()
        {
            Disable(InputModuleType.All);
            _playerInputActions?.Dispose();
        }
    }
}