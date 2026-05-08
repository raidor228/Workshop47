using Melador.PlayerController.CameraController.Settings;
using PlayerController.CameraController.States;
using PlayerController.CameraController.States.Root;
using R3;
using Unity.Cinemachine;
using UnityEngine;
using Workshop47.Scripts.Fsm;
using Workshop47.Scripts.Game.Gameplay.Input;

namespace PlayerController.CameraController
{
    public class PlayerCameraFsm : Fsm
    {
        public readonly Transform PlayerTransform;
        public readonly CameraSettings CameraSettings;
        public readonly PlayerCameraInput CameraInput;
        public readonly PlayerRtsCameraInput RtsCameraInput;

        public PlayerCameraFsm(Transform playerTransform, PlayerCameraInput cameraInput, 
            PlayerRtsCameraInput rtsCameraInput, CameraSettings cameraSettings, 
            CinemachineCamera fpsCamera, CinemachineCamera orbitalCamera, 
            CinemachineCamera rtsCamera, Transform rtsCameraRig, Subject<Unit> onSwitchRtsMode)
        {
            PlayerTransform = playerTransform;
            CameraSettings = cameraSettings;
            CameraInput = cameraInput;
            RtsCameraInput = rtsCameraInput;
            
            CameraStateContext stateContext = new CameraStateContext(fpsCamera, orbitalCamera, 
                rtsCamera, rtsCameraRig, this);
            
            AddState(new FPSState(stateContext, this));
            AddState(new TPSState(stateContext, this));
            AddState(new FreeLookState(stateContext, this));
            AddState(new RTSState(stateContext, this));

            if (cameraSettings.FPSSettings.IsMechanicAllowed 
                && cameraSettings.FPSSettings.IsFirstState)
            {
                SetState<FPSState>();
            }
            else
            {
                SetState<TPSState>();
            }

            onSwitchRtsMode.Subscribe(_ =>
            {
                if (CurrentState is RTSState)
                {
                    if (PreviousState is FPSState)
                    {
                        SetState<FPSState>();
                    }
                    else if (PreviousState is TPSState)
                    {
                        SetState<TPSState>();
                    }
                    else
                    {
                        SetState<FPSState>();
                    }
                }
                else
                {
                    SetState<RTSState>();
                }
            });
        }
    }
}