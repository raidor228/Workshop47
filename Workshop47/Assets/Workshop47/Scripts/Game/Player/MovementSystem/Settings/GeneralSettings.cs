using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Melador.PlayerController.MovementController.Settings
{
    [Serializable]
    public class GeneralSettings
    {
        [Tooltip("The layers that represent the roof")]
        [field: SerializeField] 
        public LayerMask RoofMask { get; private set; }
        
        [Tooltip("The layers that are considered for bounce while sliding")]
        [field: SerializeField]
        public LayerMask WallBounceLayer { get; private set; }
        
        [Tooltip("Action used to move the player")]
        [field: SerializeField]
        public InputActionReference MovementAction { get; private set; }
        
        [Tooltip("Animation parameter name for movement along the X axis")]
        [field: SerializeField] 
        public string MovementXAnimationParameter { get; private set; } = "MoveX";
        
        [Tooltip("Animation parameter name for movement along the Y axis")]
        [field: SerializeField] 
        public string MovementYAnimationParameter { get; private set; } = "MoveY";
        
        [Tooltip("Animation parameter name for movement along the Z axis")]
        [field: SerializeField]
        public string MovementZAnimationParameter { get; private set; } = "MoveZ";
        
        [Tooltip("Animation trigger used when the player is idle")]
        [field: SerializeField]
        public string IdleAnimationTrigger { get; private set; } = "IsIdling";
        
        [Tooltip("Animation trigger used when the player is falling")]
        [field: SerializeField]
        public string FallAnimationTrigger { get; private set; } = "IsFalling";
        
        [Tooltip("The gravitational force applied to the player while in the air")]
        [field: SerializeField, Range(-1f, -100f)]
        public float Gravity { get; private set; } = -40f;
        
        [Tooltip("The friction applied to the player's movement")] 
        [field: SerializeField, Min(0)] 
        public float Friction { get; private set; } = 7f;

        [Tooltip("Amount of velocity preserved after hitting a wall. 0 = full stop, 1 = no slowdown")] 
        [field: SerializeField, Range(0f, 1f)] 
        public float EnergyLossFactor { get; private set; } = 0.3f;
        
        [Tooltip("The maximum velocity the player can reach")] 
        [field: SerializeField, Range(0f, 300f)] 
        public float MaxVelocity { get; private set; } = 20f;
        
        [Tooltip("The height of the player while standing")]
        [field: SerializeField]
        public float StandingHeight { get; private set; } = 2f;
        
        [Tooltip("The maximum velocity the player can reach while in the air")]
        [field: SerializeField, Range(0f, 300f)]
        public float MaxAirVelocity { get; private set; } = 10f;

        [Tooltip("The minimum Y-axis velocity the player can reach (fall speed limit)")]
        [field: SerializeField, Range(-100f, -2f)]
        public float MinVerticalVelocity { get; private set; } = -30f;
        
        [Tooltip("The acceleration applied to the player while in the air")]
        [field: SerializeField, Range(0f, 200f)]
        public float AirAcceleration { get; private set; } = 3f;
        
        [Tooltip("Amount of speed lost over time while in the air")]
        [field: SerializeField, Range(0f, 100f)]
        public float AirLossFactor { get; private set; } = 20f;
        
        [Tooltip("Multiplier applied to falling speed")]
        [field: SerializeField, Range(1f, 20f)]
        public float FallFactor { get; private set; } = 2f;
        
        [Tooltip("The maximum angle of a slope that the character can walk up")]
        [field: SerializeField, Range(0f, 90f)]
        public float SlopeMaxAngle { get; private set; } = 45f;
        
        [Tooltip("The speed at which velocity interpolates when idle")]
        [field: SerializeField, Range(0f, 50f)]
        public float MovementLerpSpeed { get; private set; } = 20f;
        
        [Tooltip("The acceleration speed when starting to move")]
        [field: SerializeField, Range(0f, 50f)]
        public float MovementAccelerationSpeed { get; private set; } = 40f;
        
        [Tooltip("The deceleration speed when stopping movement")]
        [field: SerializeField, Range(0f, 50f)]
        public float MovementDecelerationSpeed { get; private set; } = 40f;
    }
}