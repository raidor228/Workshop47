using System;
using Melador.PlayerController.MovementController.Settings.Root;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Melador.PlayerController.CameraController.Settings
{
    [Serializable]
    public class FreeLookSettings : ConditionalMechanicSettings
    {
        [Tooltip("Action to look around camera")]
        [field: SerializeField]
        public InputActionReference FreeLookAction { get; private set; }
    }
}