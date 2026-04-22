using Melador.PlayerController.CameraController.Settings;
using Melador.PlayerInput.Modules;
using Unity.Cinemachine;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PlayerController.CameraController.States.Root
{
    public class CameraStateContext
    {
        public Transform PlayerTransform => _fsm.PlayerTransform;
        public CameraSettings CameraSettings => _fsm.CameraSettings;
        public PlayerCameraInput CameraInput => _fsm.CameraInput;
        
        public Camera Camera { get; }
        public CinemachineBrain CinemachineBrain { get; }

        public readonly CinemachineCamera FpsCamera;
        public readonly CinemachineCamera OrbitalCamera;
        
        private readonly PlayerCameraFsm _fsm;
        
        public CameraStateContext(CinemachineCamera fpsCamera, CinemachineCamera orbitalCamera, 
            PlayerCameraFsm fsm)
        {
            FpsCamera = fpsCamera;
            OrbitalCamera = orbitalCamera;
            
            _fsm = fsm;

            CinemachineBrain = Object.FindFirstObjectByType<CinemachineBrain>();
            Camera = CinemachineBrain.GetComponent<Camera>();
        }
    }
}