using PlayerController.CameraController.States.Root;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerController.CameraController.States
{
    public class RTSState : CameraBaseState
    {
        private float _targetZoom;
        private Vector3 _targetPosition;
        
        public RTSState(CameraStateContext stateContext, PlayerCameraFsm fsm) :
            base(stateContext, fsm)
        {
            _targetZoom = stateContext.RtsCamera.Lens.OrthographicSize;
            _targetPosition = stateContext.RtsCameraRig.position;
        }

        public override void Enter()
        {
            stateContext.RtsCamera.Priority.Value = 0;
        }

        public override void LateUpdate()
        {
            HandleMovement();
            HandleRotation();
            HandleZoom();

            SmoothZoom();
            SmoothMovement();
        }

        public override void Exit()
        {
            stateContext.RtsCamera.Priority.Value = -1;
        }

        public override Vector3 GetForward()
        {
            return stateContext.CinemachineBrain.transform.forward;
        }

        private void HandleMovement()
        {
            Vector3 forward = stateContext.RtsCameraRig.forward;
            Vector3 right = stateContext.RtsCameraRig.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            var input = stateContext.RtsCameraInput.MovementInput;
            var direction = Vector3.zero;
            
            if (input.y > 0)
                direction += forward;

            if (input.y < 0)
                direction -= forward;

            if (input.x > 0)
                direction += right;

            if (input.x < 0)
                direction -= right;
            
            if (stateContext.CameraSettings.RTSSettings.UseEdgeScrolling)
            {
                Vector2 mousePos = Mouse.current.position.ReadValue();
                var edgeSize = stateContext.CameraSettings.RTSSettings.EdgeWidth;
                
                if (mousePos.x >= Screen.width - edgeSize)
                    direction += right;

                if (mousePos.x <= edgeSize)
                    direction -= right;

                if (mousePos.y >= Screen.height - edgeSize)
                    direction += forward;

                if (mousePos.y <= edgeSize)
                    direction -= forward;
            }

            direction.Normalize();

            float currentSpeed = stateContext.CameraSettings.RTSSettings.MoveSpeed;

            var currentPosition = stateContext.RtsCameraRig.position;
            var targetPosition = currentPosition + direction * currentSpeed * Time.deltaTime;
            var clampedPosition = ClampPosition(targetPosition);
            _targetPosition = clampedPosition;
            _targetPosition.y = stateContext.RtsCameraRig.position.y;
        }

        private void HandleRotation()
        {
            float rotation = 0f;

            if (stateContext.RtsCameraInput.IsRotateLeftButtonPressed)
                rotation -= 1f;

            if (stateContext.RtsCameraInput.IsRotateRightButtonPressed)
                rotation += 1f;

            stateContext.RtsCameraRig.Rotate(Vector3.up, 
                rotation * stateContext.CameraSettings.RTSSettings.RotationSpeed * 
                Time.deltaTime, Space.World);
        }

        private void HandleZoom()
        {
            float scroll = Mouse.current.scroll.ReadValue().y;

            if (Mathf.Abs(scroll) < 0.01f)
                return;

            float size = stateContext.RtsCamera.Lens.OrthographicSize;
            size -= scroll * stateContext.CameraSettings.RTSSettings.ZoomSpeed * Time.deltaTime;
            size = Mathf.Clamp(size, stateContext.CameraSettings.RTSSettings.MinZoom, 
                stateContext.CameraSettings.RTSSettings.MaxZoom);
            _targetZoom = size;
        }

        private void SmoothZoom()
        {
            float current = stateContext.RtsCamera.Lens.OrthographicSize;

            float newZoom = Mathf.Lerp(
                current,
                _targetZoom,
                Time.deltaTime * stateContext.CameraSettings.RTSSettings.ZoomSmooth
            );

            stateContext.RtsCamera.Lens.OrthographicSize = newZoom;
        }
        
        private void SmoothMovement()
        {
            Vector3 current = stateContext.RtsCameraRig.position;

            Vector3 newPosition = Vector3.Lerp(
                current,
                _targetPosition,
                Time.deltaTime * stateContext.CameraSettings.RTSSettings.MovementSmooth
            );
            
            stateContext.RtsCameraRig.position = newPosition;
        }
        
        private Vector3 ClampPosition(Vector3 target)
        {
            float orthoSize = stateContext.RtsCamera.Lens.OrthographicSize;
            float aspect = (float)Screen.width / Screen.height;

            float verticalExtent = orthoSize;
            float horizontalExtent = orthoSize * aspect;

            var xLimits = stateContext.CameraSettings.RTSSettings.XLimits;
            var zLimits = stateContext.CameraSettings.RTSSettings.ZLimits;

            float yaw = stateContext.RtsCameraRig.eulerAngles.y * Mathf.Deg2Rad;

            float rotatedX =
                Mathf.Abs(Mathf.Cos(yaw)) * horizontalExtent +
                Mathf.Abs(Mathf.Sin(yaw)) * verticalExtent;

            float rotatedZ =
                Mathf.Abs(Mathf.Sin(yaw)) * horizontalExtent +
                Mathf.Abs(Mathf.Cos(yaw)) * verticalExtent;

            Vector3 clamped = Vector3.zero;
            
            clamped.x = Mathf.Clamp(
                target.x,
                xLimits.x + rotatedX,
                xLimits.y - rotatedX
            );

            clamped.z = Mathf.Clamp(
                target.z,
                zLimits.x + rotatedZ,
                zLimits.y - rotatedZ
            );

            return clamped;
        }
    }
}