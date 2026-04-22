using System;
using Melador.PlayerController.MovementController.Settings.Root;
using UnityEngine;

namespace Melador.PlayerController.MovementController.Settings
{
    [Serializable]
    public class VaultSettings : ConditionalMechanicSettings
    {
        [Tooltip("The layers that are considered for vaulting")]
        [field: SerializeField]
        public LayerMask VaultLayer { get; private set; }
        
        [Tooltip("Name of the animation trigger used when the player starts vaulting")]
        [field: SerializeField]
        public string VaultAnimationTrigger { get; private set; } = "IsVaulting";

        [Tooltip("Duration of the vault movement in seconds")]
        [field: SerializeField, Range(0f, 3f)]
        public float VaultDuration { get; private set; } = 0.8f;
        
        [Tooltip("Minimum velocity required to perform a long vault instead of a short one")]
        [field: SerializeField, Range(0f, 20f)]
        public float MinVelocityToLongVault { get; private set; } = 7f;
        
        [Tooltip("Velocity range used to determine the type of vault")]
        [field: SerializeField]
        public Vector2 VelocityRange { get; private set; } = new Vector2(5f, 10f);
        
        [Tooltip("Angle range (in degrees) used to determine vault behavior based on approach angle")]
        [field: SerializeField]
        public Vector2 AngleRange { get; private set; } = new Vector2(60f, 20f);
    }
}