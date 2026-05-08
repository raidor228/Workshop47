using System;
using Melador.PlayerController.MovementController.Settings.Root;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Melador.PlayerController.CameraController.Settings
{
    [Serializable]
    public class RTSSettings : ConditionalMechanicSettings
    {
        [Tooltip("Action to move camera")] 
        [field: SerializeField] 
        public InputActionReference MovementAction { get; private set; }
        
        [Tooltip("Action to zoom camera")] 
        [field: SerializeField] 
        public InputActionReference ZoomAction { get; private set; }
        
        [Tooltip("Action to rotate left camera")] 
        [field: SerializeField] 
        public InputActionReference RotateLeftAction { get; private set; }
        
        [Tooltip("Action to rotate right camera")] 
        [field: SerializeField] 
        public InputActionReference RotateRightAction { get; private set; }
        
        [Tooltip("Use edge scrolling")] 
        [field: SerializeField] 
        public bool UseEdgeScrolling { get; private set; }
        
        [Tooltip("Edge width")] 
        [field: SerializeField] 
        public float EdgeWidth { get; private set; }
        
        [Tooltip("Camera speed")] 
        [field: SerializeField] 
        public float MoveSpeed { get; private set; }   
        
        [Tooltip("Camera speed smoothness")] 
        [field: SerializeField] 
        public float MovementSmooth { get; private set; }
        
        [Tooltip("Rotation speed")] 
        [field: SerializeField] 
        public float RotationSpeed { get; private set; }
        
        [Tooltip("Max zoom limit")] 
        [field: SerializeField] 
        public float MaxZoom { get; private set; }
        
        [Tooltip("Min zoom limit")] 
        [field: SerializeField] 
        public float MinZoom { get; private set; }
        
        [Tooltip("Speed of zoom")] 
        [field: SerializeField] 
        public float ZoomSpeed { get; private set; }
        
        [Tooltip("Smooth of zoom")] 
        [field: SerializeField] 
        public float ZoomSmooth  { get; private set; }
        
        [Tooltip("Bounds for x")] 
        [field: SerializeField] 
        public Vector2 XLimits { get; private set; }
        
        [Tooltip("Bounds for z")] 
        [field: SerializeField] 
        public Vector2 ZLimits { get; private set; }
    }
}