using System;
using Melador.PlayerController.MovementController.Settings.Root;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Melador.PlayerController.MovementController.Settings
{
    [Serializable]
    public class CrawlSettings : ConditionalMechanicSettings
    {
        [Tooltip("Action to enter the crawling state")]
        [field: SerializeField]
        public InputActionReference CrawlAction { get; private set; }
        
        [Tooltip("Name of the animation trigger used when the player starts crawling")]
        [field: SerializeField]
        public string CrawlAnimationTrigger { get; private set; } = "IsCrawling";
        
        [Tooltip("The height of the player while crawling")]
        [field: SerializeField]
        public float CrawlingHeight { get; private set; } = 0.5f;
        
        [Tooltip("The speed at which the player crawls")]
        [field: SerializeField, Range(0f, 10f)]
        public float CrawlingSpeed { get; private set; } = 6f;

        [Tooltip("The acceleration applied to the player while crawling")]
        [field: SerializeField, Range(1f, 300f)]
        public float CrawlAcceleration { get; private set; } = 8f;
    }
}