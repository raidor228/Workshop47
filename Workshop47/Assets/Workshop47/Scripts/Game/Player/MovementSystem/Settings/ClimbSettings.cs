using System;
using Melador.PlayerController.MovementController.Settings.Root;
using UnityEngine;

namespace Melador.PlayerController.MovementController.Settings
{
    [Serializable]
    public class ClimbSettings : ConditionalMechanicSettings
    {
        [Tooltip("Name of the animation trigger used when the player starts climbing")]
        [field: SerializeField] 
        public string ClimbAnimationTrigger { get; private set; } = "IsClimbing";
        
        [Tooltip("The speed at which the player climbs upwards")]
        [field: SerializeField, Range(0f, 15f)]
        public float ClimbUpSpeed { get; private set; } = 4f;
    }
}