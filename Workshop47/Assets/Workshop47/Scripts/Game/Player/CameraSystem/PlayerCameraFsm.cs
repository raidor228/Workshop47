using Melador.PlayerController.CameraController.Settings;
using Melador.PlayerInput.Modules;
using PlayerController.CameraController.States;
using PlayerController.CameraController.States.Root;
using Unity.Cinemachine;
using UnityEngine;
using Workshop47.Scripts.Game.Fsm;

namespace PlayerController.CameraController
{
    public class PlayerCameraFsm : Fsm
    {
        public readonly Transform PlayerTransform;
        public readonly CameraSettings CameraSettings;
        public readonly PlayerCameraInput CameraInput;

        public PlayerCameraFsm(Transform playerTransform, PlayerCameraInput cameraInput, 
            CameraSettings cameraSettings, CinemachineCamera fpsCamera, CinemachineCamera orbitalCamera)
        {
            PlayerTransform = playerTransform;
            CameraSettings = cameraSettings;
            CameraInput = cameraInput;
            
            CameraStateContext stateContext = new CameraStateContext(fpsCamera, orbitalCamera, this);
            
            AddState(new FPSState(stateContext, this));
            AddState(new TPSState(stateContext, this));
            AddState(new FreeLookState(stateContext, this));

            if (cameraSettings.FPSSettings.IsMechanicAllowed 
                && cameraSettings.FPSSettings.IsFirstState)
            {
                SetState<FPSState>();
            }
            else
            {
                SetState<TPSState>();
            }
        }
    }
}