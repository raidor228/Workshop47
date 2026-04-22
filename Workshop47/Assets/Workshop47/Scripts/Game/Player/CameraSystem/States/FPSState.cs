using PlayerController.CameraController.States.Root;
using Unity.Cinemachine;
using UnityEngine;

namespace PlayerController.CameraController.States
{
    public class FPSState : CameraBaseState
    {
        private CinemachinePanTilt _panTilt;
        
        public FPSState(CameraStateContext stateContext, PlayerCameraFsm fsm) : 
            base(stateContext, fsm)
        {
        }

        public override void Enter()
        {
            stateContext.FpsCamera.Priority.Value = 0;
            stateContext.Camera.cullingMask = stateContext.CameraSettings.FPSSettings.CullingMask;
            
            if (_panTilt == null)
            {
                _panTilt = stateContext.FpsCamera.GetComponent<CinemachinePanTilt>();
            }
            
            ResetRotation();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (stateContext.CameraInput.PerspectiveAction.WasPerformedThisFrame() &&
                stateContext.CameraSettings.TPSSettings.IsMechanicAllowed)
            {
                Fsm.SetState<TPSState>();
            }
        }
        
        public override void LateUpdate()
        {
            Vector3 angles = stateContext.PlayerTransform.rotation.eulerAngles;
            angles.y = stateContext.FpsCamera.transform.eulerAngles.y;
            stateContext.PlayerTransform.rotation = Quaternion.Euler(angles);
        }

        public override void Exit()
        {
            stateContext.FpsCamera.Priority.Value = -1;
            stateContext.Camera.cullingMask = stateContext.CameraSettings.GeneralSettings.DefaultCullingMask;
        }

        public override Vector3 GetForward()
        {
            return stateContext.CinemachineBrain.transform.forward;
        }

        protected override void HandleInput()
        {
            _panTilt.TiltAxis.Value -= 
                stateContext.CameraInput.MouseInput.y * 
                stateContext.CameraSettings.GeneralSettings.MouseSensitivityY * Time.deltaTime;
            
            _panTilt.PanAxis.Value += 
                stateContext.CameraInput.MouseInput.x * 
                stateContext.CameraSettings.GeneralSettings.MouseSensitivityX * Time.deltaTime;
        }

        protected override void ResetRotation()
        {
            float targetYaw = stateContext.PlayerTransform.eulerAngles.y;
            _panTilt.PanAxis.Value = targetYaw;
            _panTilt.TiltAxis.Value = 0f;
        }
        
        protected override void ClampVerticalAxis()
        {
            _panTilt.TiltAxis.Value = Mathf.Clamp(_panTilt.TiltAxis.Value,
                _panTilt.TiltAxis.Range.x, _panTilt.TiltAxis.Range.y);
        }
    }
}