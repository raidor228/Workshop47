using UnityEngine;
using UnityEngine.InputSystem;

namespace Workshop47.Scripts.Game.Player.InteractionSystem.Settings
{
    [CreateAssetMenu(fileName = "New InteractionsSettings", menuName = "Melador/Settings/Interactions Settings")]
    public class InteractionsSettings : ScriptableObject
    {
        [field: SerializeField]
        public InputActionReference InteractAction { get; private set; }
        
        [field: SerializeField]
        public InputActionReference NextInteractionAction { get; private set; }
        
        [field: SerializeField]
        public InputActionReference PreviousInteractionAction { get; private set; }
        
        [field: SerializeField]
        public LayerMask InteractionLayer { get; private set; }
        
        [field: SerializeField]
        public float InteractionsRadius { get; private set; }
        
        [field: SerializeField]
        public float OverlapFrequency { get; private set; }
    }
}