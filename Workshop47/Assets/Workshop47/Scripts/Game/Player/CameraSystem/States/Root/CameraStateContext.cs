using Melador.PlayerController.CameraController.Settings;
using Unity.Cinemachine;
using UnityEngine;
using Workshop47.Scripts.Game.Gameplay.Input;
using Object = UnityEngine.Object;

namespace PlayerController.CameraController.States.Root
{
    public class CameraStateContext
    {
        public Transform PlayerTransform => _fsm.PlayerTransform;
        public CameraSettings CameraSettings => _fsm.CameraSettings;
        public PlayerCameraInput CameraInput => _fsm.CameraInput;
        public PlayerRtsCameraInput RtsCameraInput => _fsm.RtsCameraInput;
        
        public Camera Camera { get; }
        public CinemachineBrain CinemachineBrain { get; }
        public readonly Transform RtsCameraRig;

        public readonly CinemachineCamera FpsCamera;
        public readonly CinemachineCamera OrbitalCamera;
        public readonly CinemachineCamera RtsCamera;
        
        private readonly PlayerCameraFsm _fsm;
        
        public CameraStateContext(CinemachineCamera fpsCamera, CinemachineCamera orbitalCamera, 
            CinemachineCamera rtsCamera, Transform rtsCameraRig, PlayerCameraFsm fsm)
        {
            FpsCamera = fpsCamera;
            OrbitalCamera = orbitalCamera;
            RtsCamera = rtsCamera;
            RtsCameraRig = rtsCameraRig;
            
            _fsm = fsm;

            CinemachineBrain = Object.FindFirstObjectByType<CinemachineBrain>();
            Camera = CinemachineBrain.GetComponent<Camera>();
        }
    }
}