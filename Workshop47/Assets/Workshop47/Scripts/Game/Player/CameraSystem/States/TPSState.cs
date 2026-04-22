using System.Collections;
using PlayerController.CameraController.States.Root;
using Unity.Cinemachine;
using UnityEngine;

namespace PlayerController.CameraController.States
{
    public class TPSState : CameraBaseState
    {
        private CinemachineOrbitalFollow _orbitalFollow;

        private float _addedDegrees;
        private bool _recentering;

        public TPSState(CameraStateContext stateContext, PlayerCameraFsm fsm) :
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

            if (Fsm.PreviousState is FreeLookState)
            {
                _recentering = true;
                _orbitalFollow.StartCoroutine(HandleRecentering());
            }
            else
            {
                ResetRotation();
            }
        }

        public override void LogicUpdate()
        {
            if (!_recentering)
            {
                base.LogicUpdate();
            }

            if (stateContext.CameraInput.PerspectiveAction.WasPerformedThisFrame() &&
                stateContext.CameraSettings.FPSSettings.IsMechanicAllowed)
            {
                Fsm.SetState<FPSState>();
                return;
            }

            if (stateContext.CameraInput.IsFreeLookButtonPressed &&
                stateContext.CameraSettings.FreeLookSettings.IsMechanicAllowed)
            {
                Fsm.SetState<FreeLookState>();
                return;
            }
        }

        public override void LateUpdate()
        {
            Vector3 angles = stateContext.PlayerTransform.rotation.eulerAngles;

            if (_recentering)
            {
                float inputAngle = stateContext.CameraInput.MouseInput.x *
                                   stateContext.CameraSettings.GeneralSettings.MouseSensitivityX * Time.deltaTime;
                
                angles.y += inputAngle;
            }
            else
            {
                angles.y = stateContext.OrbitalCamera.transform.eulerAngles.y;
            }
            
            stateContext.PlayerTransform.rotation = Quaternion.Euler(angles);
        }

        public override void Exit()
        {
            stateContext.OrbitalCamera.Priority.Value = -1;
        }

        public override Vector3 GetForward()
        {
            return stateContext.CinemachineBrain.transform.forward;
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

        protected override void ResetRotation()
        {
            float targetYaw = stateContext.PlayerTransform.eulerAngles.y;
            _orbitalFollow.HorizontalAxis.Value = targetYaw;
            _orbitalFollow.VerticalAxis.Value = 0f;
        }

        protected override void ClampVerticalAxis()
        {
            _orbitalFollow.VerticalAxis.Value = Mathf.Clamp(_orbitalFollow.VerticalAxis.Value,
                _orbitalFollow.VerticalAxis.Range.x, _orbitalFollow.VerticalAxis.Range.y);
        }

        private IEnumerator HandleRecentering()
        {
            float offsetY = stateContext.PlayerTransform.eulerAngles.y;

            float startValue = _orbitalFollow.HorizontalAxis.Value - offsetY;
            float endValue = offsetY;

            startValue %= 360f;
            if (Mathf.Abs(startValue) > 180f)
            {
                if (startValue < 0f)
                {
                    startValue = Mathf.Abs(startValue) - 360f;
                    startValue *= -1f;
                }
                else
                {
                    startValue -= 360f;
                }
            }

            startValue += offsetY;

            float speed = startValue / stateContext.CameraSettings.GeneralSettings.BlendDurationFromFreeLook;
            float delta = Mathf.DeltaAngle(startValue, endValue);

            _addedDegrees = 0f;
            float time = 0;
            while (time < stateContext.CameraSettings.GeneralSettings.BlendDurationFromFreeLook)
            {
                _addedDegrees += stateContext.CameraInput.MouseInput.x *
                                 stateContext.CameraSettings.GeneralSettings.MouseSensitivityX * Time.deltaTime;
                speed = (startValue + _addedDegrees) / 
                        stateContext.CameraSettings.GeneralSettings.BlendDurationFromFreeLook;
                
                time += Time.deltaTime;
                float t = time * speed / (startValue + _addedDegrees);

                float curvedT = stateContext.CameraSettings.GeneralSettings.BlendCurveFromFreeLook.Evaluate(t);

                float current = startValue + _addedDegrees + delta * curvedT;
                _orbitalFollow.HorizontalAxis.Value = current;

                yield return null;
            }

            _orbitalFollow.HorizontalAxis.Value = endValue + _addedDegrees;
            _recentering = false;
        }
    }
}