using System;
using Melador.PlayerController.MovementController.Settings.Root;
using UnityEngine;

namespace Melador.PlayerController.CameraController.Settings
{
    [Serializable]
    public class TPSSettings : ConditionalMechanicSettings
    {
        [Tooltip("Should this state be enabled first by default?")] 
        [field: SerializeField] 
        public bool IsFirstState { get; private set; } = false;
    }
}