using UnityEngine;

namespace Melador.PlayerController.GameObjects
{
    public class Ledge : MonoBehaviour
    {
        [Tooltip("Transform representing the ledge level.")]
        [SerializeField] private Transform _ledgeLevel;

        public float LedgeLevel => _ledgeLevel.position.y;
    }
}