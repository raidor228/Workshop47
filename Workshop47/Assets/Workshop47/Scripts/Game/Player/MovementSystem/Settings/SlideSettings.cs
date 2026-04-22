using System;
using Melador.PlayerController.MovementController.Settings.Root;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Melador.PlayerController.MovementController.Settings
{
    [Serializable]
    public class SlideSettings : ConditionalMechanicSettings
    {
        [Tooltip("The layers that are considered for sliding")]
        [field: SerializeField]
        public LayerMask SlidingLayer { get; private set; }
        
        [Tooltip("Action to enter the sliding state")]
        [field: SerializeField]
        public InputActionReference SlideAction { get; private set; }
        
        [Tooltip("Name of the animation trigger used when the player starts sliding")]
        [field: SerializeField]
        public string SlideAnimationTrigger { get; private set; } = "IsSliding";
        
        [Tooltip("The minimum velocity required to initiate a slide")]
        [field: SerializeField, Range(1f, 10f)]
        public float SlideVelocityThreshold { get; private set; } = 2f;
        
        [Tooltip("The boost applied to the player's velocity while sliding")]
        [field: SerializeField, Range(1f, 10f)]
        public float SlideBoostForce { get; private set; } = 5f;

        [Tooltip("The maximum speed cap while boosting during a slide")]
        [field: SerializeField, Range(1f, 30f)]
        public float BoostSpeedCap { get; private set; } = 20f;

        [Tooltip("The side acceleration applied during a slide")]
        [field: SerializeField, Range(0f, 10f)]
        public float SlidingSideAcceleration { get; private set; } = 1f;

        [Tooltip("Factor by which speed is reduced during a slide. Closer to 1 means slower deceleration")]
        [field: SerializeField, Range(1f, 1.1f)]
        public float SlidingSlowdownFactor { get; private set; } = 1.005f;
        
        [Tooltip("The duration of the slide effect")]
        [field: SerializeField, Range(0f, 5f)]
        public float SlideTime { get; private set; } = 1.2f;

        [Tooltip("The blend time between slopes and flat surfaces during a slide")]
        [field: SerializeField, Range(0f, 1f)]
        public float SlideBlendTime { get; private set; } = 0.15f;
        
        [Tooltip("The cooldown time between slides")]
        [field: SerializeField, Range(1f, 10f)]
        public float BoostCooldown { get; private set; } = 2f;
        
        [Tooltip("Multiplier applied to falling speed. Higher values make falls faster")]
        [field: SerializeField, Range(1f, 20f)]
        public float FallFactor { get; private set; } = 14f;
    }
}