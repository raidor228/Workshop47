using Melador.PlayerController.CameraController.Settings;
using Melador.PlayerController.MovementController;
using Melador.PlayerController.MovementController.Settings;
using Melador.PlayerController.Root;
using Melador.PlayerInput;
using PlayerController.CameraController;
using Unity.Cinemachine;
using UnityEngine;

namespace Workshop47.Scripts.Game.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Animator _animator;
        [SerializeField] private CinemachineCamera _fpsCamera;
        [SerializeField] private CinemachineCamera _orbitalCamera;
        [SerializeField] private Transform _rootTransform;
        [SerializeField] private PlayerSettings _playerSettings;
        [SerializeField] private CameraSettings _cameraSettings;
        
        public CharacterController CharacterController => _characterController;
        public Animator Animator => _animator;
        public Transform RootTransform => _rootTransform;
        public PlayerSettings PlayerSettings => _playerSettings;
        public CameraSettings CameraSettings => _cameraSettings;
        public PlayerInputProvider PlayerInputProvider => _playerInputProvider;
        
        public Fsm.Fsm MovementFsm => _movementFsm;
        public Fsm.Fsm CameraFsm => _cameraFsm;
        
        private PlayerInputProvider _playerInputProvider;
        
        private PlayerMovementFsm _movementFsm;
        private PlayerCameraFsm _cameraFsm;
        
        public void Initialize(PlayerInputProvider inputProvider)
        {
            _playerInputProvider = inputProvider;

            _cameraFsm = new PlayerCameraFsm(transform, _playerInputProvider.CameraInput, 
                _cameraSettings, _fpsCamera, _orbitalCamera);
            
            _movementFsm = new PlayerMovementFsm(this);
        }

        public void SetRotation(Quaternion rotation)
        {
            Vector3 angles = rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(angles.x, angles.y, angles.z);
        }
        
        public Vector3 GetCameraForward()
        {
            return ((IPlayerCamera)_cameraFsm.CurrentState).GetForward();
        }

        public Vector3 CameraTransformDirection(Vector3 direction)
        {
            return ((IPlayerCamera)_cameraFsm.CurrentState).TransformDirection(direction);
        }

        public Vector3 GetCameraPosition()
        {
            return ((IPlayerCamera)_cameraFsm.CurrentState).GetPosition();
        }

        public bool IsCameraInWater()
        {
            return ((IPlayerCamera)_cameraFsm.CurrentState).IsInWater();
        }
        
        protected virtual void Update()
        {
            _movementFsm?.LogicUpdate();
            _cameraFsm?.LogicUpdate();
        }

        protected virtual void LateUpdate()
        {
            _movementFsm?.LateUpdate();
            _cameraFsm?.LateUpdate();
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            _movementFsm?.OnTriggerEnter(other);
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            _movementFsm?.OnTriggerExit(other);
        }

        protected virtual void OnControllerColliderHit(ControllerColliderHit hit)
        {
            _movementFsm?.OnControllerColliderHit(hit);
        }
    }
}