using PlayerController.CameraController.States.Root;
using Unity.Cinemachine;
using UnityEngine;

namespace PlayerController.CameraController.States
{
    public class FreeLookState : CameraBaseState
    {
        private CinemachineOrbitalFollow _orbitalFollow;

        public FreeLookState(CameraStateContext stateContext, PlayerCameraFsm fsm) : 
            base(stateContext, fsm)
        {
        }

        public override void Enter()
        {
            stateContext.OrbitalCamera.Priority.Value = 0;

            if (_orbitalFollow == null)
            {
                _orbitalFollow = stateContext.OrbitalCamera.GetComponent<CinemachineOrbitalFollow>();
            }
            
            ResetRotation();
        }
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (stateContext.CameraInput.PerspectiveAction.WasPerformedThisFrame())
            {
                Fsm.SetState<FPSState>();
                return;
            }

            if (!stateContext.CameraInput.IsFreeLookButtonPressed)
            {
                Fsm.SetState<TPSState>();
                return;
            }
        }
        
        public override void Exit()
        {
            stateContext.OrbitalCamera.Priority.Value = -1;
        }
        
        public override Vector3 GetForward()
        {
            return stateContext.PlayerTransform.forward;
        }

        protected override void HandleInput()
        {
            _orbitalFollow.VerticalAxis.Value -= 
                stateContext.CameraInput.MouseInput.y * 
                stateContext.CameraSettings.GeneralSettings.MouseSensitivityY * Time.deltaTime;
            
            _orbitalFollow.HorizontalAxis.Value += 
                stateContext.CameraInput.MouseInput.x * 
                stateContext.CameraSettings.GeneralSettings.MouseSensitivityX * Time.deltaTime;
        }
        
        protected override void ClampVerticalAxis()
        {
            _orbitalFollow.VerticalAxis.Value = Mathf.Clamp(_orbitalFollow.VerticalAxis.Value,
                _orbitalFollow.VerticalAxis.Range.x, _orbitalFollow.VerticalAxis.Range.y);
        }
    }
}