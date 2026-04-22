using System;
using Melador.PlayerController.MovementController.Settings.Root;
using UnityEngine;

namespace Melador.PlayerController.CameraController.Settings
{
    [Serializable]
    public class FPSSettings : ConditionalMechanicSettings
    {
        [Tooltip("Culling mask of main camera in FPS mode")] 
        [field: SerializeField] 
        public LayerMask CullingMask { get; private set; }
        
        [Tooltip("Should this state be enabled first by default?")] 
        [field: SerializeField] 
        public bool IsFirstState { get; private set; } = true;
    }
}