using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Melador.PlayerController.CameraController.Settings
{
    [Serializable]
    public class GeneralSettings
    {
        [Tooltip("Action to rotate camera")]
        [field: SerializeField]
        public InputActionReference MouseLookAction { get; private set; }
        
        [Tooltip("Action to change perspective of camera")]
        [field: SerializeField]
        public InputActionReference PerspectiveAction { get; private set; }

        [Tooltip("Culling mask of main camera in by default")] 
        [field: SerializeField] 
        public LayerMask DefaultCullingMask { get; private set; }
        
        [Tooltip("Horizontal mouse sensitivity for camera rotation")]
        [field: SerializeField] 
        public float MouseSensitivityX { get; private set; } = 1f;
        
        [Tooltip("Vertical mouse sensitivity for camera rotation")]
        [field: SerializeField] 
        public float MouseSensitivityY { get; private set; } = 1f;
        
        [Tooltip("Duration of the camera transition when switching from Free Look mode")]
        [field: SerializeField, Range(0f, 2f)] 
        public float BlendDurationFromFreeLook { get; private set; } = 0.5f;
        
        [Tooltip("Curve controlling the smoothness of the transition from Free Look mode")]
        [field: SerializeField] 
        public AnimationCurve BlendCurveFromFreeLook { get; private set; }
    }
}