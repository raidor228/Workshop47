using System;
using Melador.PlayerController.CameraController.Settings;
using Melador.PlayerController.MovementController.Settings;
using UnityEngine;
using Workshop47.Input;
using Workshop47.Scripts.Game.Player.InteractionSystem.Settings;

namespace Workshop47.Scripts.Game.Gameplay.Input
{
    public class InputContextManager : IDisposable
    {
        public readonly InteractionsInput InteractionsInput;
        public readonly PlayerMovementInput MovementInput;
        public readonly PlayerCameraInput CameraInput;
        public readonly PlayerRtsCameraInput RtsCameraInput;
        
        private readonly PlayerInputActions _playerInputActions;

        public InputContextManager(PlayerSettings playerSettings, CameraSettings cameraSettings, 
            InteractionsSettings interactionsSettings)
        {
            _playerInputActions = new PlayerInputActions();

            MovementInput = new PlayerMovementInput(_playerInputActions, playerSettings);
            CameraInput = new PlayerCameraInput(_playerInputActions, cameraSettings);
            RtsCameraInput = new PlayerRtsCameraInput(_playerInputActions, cameraSettings);
            InteractionsInput = new InteractionsInput(_playerInputActions, interactionsSettings);
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
            if ((inputModules & InputModuleType.RtsCamera) != 0)
            {
                RtsCameraInput.Disable();
            }
            if ((inputModules & InputModuleType.Interactions) != 0)
            {
                InteractionsInput.Disable();
            }
        }
        
        public void DisableOnly(InputModuleType inputModules)
        {
            Enable(InputModuleType.All);
            Disable(inputModules);
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
            if ((inputModules & InputModuleType.RtsCamera) != 0)
            {
                RtsCameraInput.Enable();
            }
            if ((inputModules & InputModuleType.Interactions) != 0)
            {
                InteractionsInput.Enable();
            }
        }

        public void EnableOnly(InputModuleType inputModules)
        {
            Disable(InputModuleType.All);
            Enable(inputModules);
        }
        
        public void Dispose()
        {
            Disable(InputModuleType.All);
            _playerInputActions?.Dispose();
        }
    }
}