using System.Collections.Generic;
using Melador.PlayerController.CameraController.Settings;
using Melador.PlayerController.MovementController;
using Melador.PlayerController.MovementController.Settings;
using Melador.PlayerController.Root;
using Melador.PlayerInput;
using PlayerController.CameraController;
using R3;
using Unity.Cinemachine;
using UnityEngine;
using Workshop47.Scripts.Game.Gameplay.View.Interactable;
using Workshop47.Scripts.Game.Player.InteractionSystem;
using Workshop47.Scripts.Game.Player.InteractionSystem.Settings;

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
        [SerializeField] private InteractionsSettings _interactionsSettings;
        
        public CharacterController CharacterController => _characterController;
        public Animator Animator => _animator;
        public Transform RootTransform => _rootTransform;
        public PlayerSettings PlayerSettings => _playerSettings;
        public CameraSettings CameraSettings => _cameraSettings;
        public InteractionsSettings InteractionsSettings => _interactionsSettings;
        public PlayerInputProvider PlayerInputProvider => _playerInputProvider;
        
        public Scripts.Fsm.Fsm MovementFsm => _movementFsm;
        public Scripts.Fsm.Fsm CameraFsm => _cameraFsm;
        
        private PlayerInputProvider _playerInputProvider;
        
        private PlayerMovementFsm _movementFsm;
        private PlayerCameraFsm _cameraFsm;
        private InteractionsHandler _interactionsHandler;
        
        public void Initialize(PlayerInputProvider inputProvider, 
            Subject<List<IInteractable>> onInteractablesOverlap)
        {
            _playerInputProvider = inputProvider;

            _cameraFsm = new PlayerCameraFsm(transform, _playerInputProvider.CameraInput, 
                _cameraSettings, _fpsCamera, _orbitalCamera);
            
            _movementFsm = new PlayerMovementFsm(this);

            _interactionsHandler = new InteractionsHandler(_rootTransform, _interactionsSettings, onInteractablesOverlap);
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
            _interactionsHandler?.Update();
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