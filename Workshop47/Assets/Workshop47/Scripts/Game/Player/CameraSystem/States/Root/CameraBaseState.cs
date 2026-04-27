using Melador.PlayerController;
using Melador.PlayerController.Root;
using UnityEngine;
using Workshop47.Scripts.Fsm;

namespace PlayerController.CameraController.States.Root
{
    public class CameraBaseState : FsmState, IPlayerCamera
    {
        protected readonly CameraStateContext stateContext;

        private readonly PlayerCameraFsm _fsm;

        protected CameraBaseState(CameraStateContext stateContext, PlayerCameraFsm fsm) : base(fsm)
        {
            this.stateContext = stateContext;
            _fsm = fsm;
        }

        public override void LogicUpdate()
        {
            HandleInput();
            ClampVerticalAxis();
        }

        public virtual Vector3 GetForward()
        {
            return Vector3.zero;
        }

        public Vector3 TransformDirection(Vector3 direction)
        {
            if (_fsm.CurrentState is FreeLookState)
            {
                return stateContext.PlayerTransform.TransformDirection(direction);
            }
            
            return stateContext.CinemachineBrain.transform.TransformDirection(direction);
        }

        public Vector3 GetPosition()
        {
            return stateContext.CinemachineBrain.transform.position;
        }

        public bool IsInWater()
        {
            Vector3 camPosition = stateContext.CinemachineBrain.transform.position;
            Collider[] hitColliders = Physics.OverlapSphere(camPosition, 0.3f);
            foreach (var collider in hitColliders)
            {
                if (collider.CompareTag(TagsAndLayers.Water))
                {
                    return true;
                }
            }

            return false;
        }

        protected virtual void HandleInput()
        {
        }

        protected virtual void ResetRotation()
        {
        }

        protected virtual void ClampVerticalAxis()
        {
        }
    }
}