using System;
using System.Collections.Generic;
using Melador.PlayerController.MovementController.Settings;
using Melador.PlayerController.MovementController.States.Handlers;
using UnityEngine;
using Workshop47.Scripts.Fsm;
using Workshop47.Scripts.Game.Gameplay.Input;

namespace Melador.PlayerController.MovementController.States.Root
{
    public class MovementStateContext
    {
        public Transform PlayerTransform => _fsm.Controller.transform;
        public Transform PlayerRootTransform => _fsm.Controller.RootTransform;
        public CharacterController CharacterController => _fsm.Controller.CharacterController;
        public PlayerSettings PlayerSettings => _fsm.Controller.PlayerSettings;
        public PlayerMovementInput MovementInput => _fsm.Controller.InputContextManager.MovementInput;

        public MovementHandler MovementHandler { get; }
        public SlopeHandler SlopeHandler { get; }
        public CrouchHandler CrouchHandler { get; }
        public CrawlHandler CrawlHandler { get; }
        public ClimbHandler ClimbHandler { get; }
        public LedgeHandler LedgeHandler { get; }
        public SwimHandler SwimHandler { get; }
        public VaultHandler VaultHandler { get; }
        public WallRunHandler WallRunHandler { get; }
        public RollHandler RollHandler { get; }
        public CollisionResponder CollisionResponder { get; }
        public AnimatorHandler AnimatorHandler { get; }
        
        public Vector3 Velocity { get; private set; }
        public Vector3 HorizontalVelocity => new Vector3(Velocity.x, 0f, Velocity.z);
        public Vector3 DesiredVelocity { get; set; }
        public Vector3 HorizontalDesiredVelocity => new Vector3(DesiredVelocity.x, 0f, DesiredVelocity.z);
        public bool HasJumped { get; set; }
        public int JumpCount { get; set; }
        public float TargetHeight { get; set; }
        public float UpdatingColliderSpeed { get; set; }
        
        public Func<bool> IsJumpingAllowed = () => true;

        public FsmState CurrentState => _fsm.CurrentState;
        
        private readonly PlayerMovementFsm _fsm;
        
        public MovementStateContext(PlayerMovementFsm fsm)
        {
            _fsm = fsm;

            MovementHandler = new MovementHandler(this);
            SlopeHandler = new SlopeHandler(this);
            CrouchHandler = new CrouchHandler(this);
            CrawlHandler = new CrawlHandler(this);
            ClimbHandler = new ClimbHandler(this);
            LedgeHandler = new LedgeHandler(this);
            SwimHandler = new SwimHandler(this);
            VaultHandler = new VaultHandler(this);
            WallRunHandler = new WallRunHandler(this);
            RollHandler = new RollHandler(this);
            CollisionResponder = new CollisionResponder(this);
            AnimatorHandler = new AnimatorHandler(this, _fsm.Controller.Animator);
        }

        public Vector3 GetCameraForward() => _fsm.Controller.GetCameraForward();
        public Vector3 CameraTransformDirection(Vector3 direction) => 
            _fsm.Controller.CameraTransformDirection(direction);
        public Vector3 GetCameraPosition() => _fsm.Controller.GetCameraPosition();
        public bool IsCameraInWater() => _fsm.Controller.IsCameraInWater();
        
        public void LerpVelocity()
        {
            var desiredVelocityMagnitude = HorizontalDesiredVelocity.magnitude;
            var velocityMagnitude = HorizontalVelocity.magnitude;

            Vector3 newVelocity = HorizontalVelocity;
            if (desiredVelocityMagnitude >= velocityMagnitude)
            {
                newVelocity = Vector3.Lerp(newVelocity, HorizontalDesiredVelocity,
                    PlayerSettings.GeneralSettings.MovementAccelerationSpeed * Time.deltaTime);
            }
            else
            {
                newVelocity = Vector3.Lerp(newVelocity, HorizontalDesiredVelocity,
                    PlayerSettings.GeneralSettings.MovementDecelerationSpeed * Time.deltaTime);
            }

            newVelocity.y = DesiredVelocity.y;
            Velocity = newVelocity;
        }
        
        public Vector3 GetMoveDirection()
        {
            Vector2 movementInput = MovementInput.MovementInput;
            Vector3 moveDirection = new Vector3(movementInput.x, 0f, movementInput.y);
            moveDirection = PlayerTransform.TransformDirection(moveDirection);
            return moveDirection;
        }

        public float LinearMap(float value, Vector2 from, Vector2 to)
        {
            return Mathf.Clamp(
                (value - from.x) / (from.y - from.x) * (to.y - to.x) + to.x,
                to.y,
                to.x
            );
        }

        public bool IsGrounded()
        {
            return GetDistanceToGround() < 0.1f;
        }
        
        public float GetDistanceToRoof()
        {
            Vector3 sphereCastOrigin = PlayerTransform.position + CharacterController.center;
            float sphereCastRadius = CharacterController.radius;
            float sphereCastMaxDistance = 4.0f;

            if (Physics.SphereCast(sphereCastOrigin, sphereCastRadius, PlayerTransform.up, 
                    out RaycastHit hit, sphereCastMaxDistance, PlayerSettings.GeneralSettings.RoofMask))
            {
                return hit.distance;
            }
            
            return 0f;
        }
        
        public RaycastHit GetClosestHitAround(LayerMask layerMask)
        {
            List<RaycastHit> raycastHitList = new();

            Vector3 position = PlayerTransform.position;
            Ray ray1 = new Ray(position + PlayerTransform.TransformDirection(new Vector3(0f, 0f, 0.2f)),
                PlayerTransform.forward);
            Ray ray2 = new Ray(position + PlayerTransform.TransformDirection(new Vector3(0.2f, 0f, 0f)),
                PlayerTransform.right);
            Ray ray3 = new Ray(position + PlayerTransform.TransformDirection(new Vector3(0f, 0f, -0.2f)),
                -PlayerTransform.forward);
            Ray ray4 = new Ray(position + PlayerTransform.TransformDirection(new Vector3(-0.2f, 0f, 0f)),
                -PlayerTransform.right);
            Ray ray5 = new Ray(position + PlayerTransform.TransformDirection(new Vector3(0.2f, 0f, 0.2f)),
                Quaternion.AngleAxis(45f, Vector3.up) * PlayerTransform.forward);
            Ray ray6 = new Ray(position + PlayerTransform.TransformDirection(new Vector3(0.2f, 0f, -0.2f)),
                Quaternion.AngleAxis(45f, Vector3.up) * PlayerTransform.right);
            Ray ray7 = new Ray(position + PlayerTransform.TransformDirection(new Vector3(-0.2f, 0f, -0.2f)),
                Quaternion.AngleAxis(45f, Vector3.up) * -PlayerTransform.forward);
            Ray ray8 = new Ray(position + PlayerTransform.TransformDirection(new Vector3(-0.2f, 0f, 0.2f)),
                Quaternion.AngleAxis(45f, Vector3.up) * -PlayerTransform.right);
            
            if (Physics.Raycast(ray1, out var hitInfo1, float.PositiveInfinity, layerMask))
                raycastHitList.Add(hitInfo1);
            if (Physics.Raycast(ray2, out var hitInfo2, float.PositiveInfinity, layerMask))
                raycastHitList.Add(hitInfo2);
            if (Physics.Raycast(ray3, out var hitInfo3, float.PositiveInfinity, layerMask))
                raycastHitList.Add(hitInfo3);
            if (Physics.Raycast(ray4, out var hitInfo4, float.PositiveInfinity, layerMask))
                raycastHitList.Add(hitInfo4);
            if (Physics.Raycast(ray5, out var hitInfo5, float.PositiveInfinity, layerMask))
                raycastHitList.Add(hitInfo5);
            if (Physics.Raycast(ray6, out var hitInfo6, float.PositiveInfinity, layerMask))
                raycastHitList.Add(hitInfo6);
            if (Physics.Raycast(ray7, out var hitInfo7, float.PositiveInfinity, layerMask))
                raycastHitList.Add(hitInfo7);
            if (Physics.Raycast(ray8, out var hitInfo8, float.PositiveInfinity, layerMask))
                raycastHitList.Add(hitInfo8);

            if (raycastHitList.Count == 0)
            {
                return new RaycastHit();
            }

            RaycastHit ray = raycastHitList[0];
            foreach (var hit in raycastHitList)
            {
                if (hit.distance < ray.distance)
                {
                    ray = hit;
                }
            }

            return ray;
        }

        private float GetDistanceToGround()
        {
            if (Physics.Raycast(PlayerTransform.position, Vector3.down, 
                    out var hitInfo, float.PositiveInfinity))
            {
                return hitInfo.distance;
            }

            return 0f;
        }
    }
}