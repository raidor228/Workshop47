using UnityEngine;

namespace Melador.PlayerController.GameObjects
{
    public class Water : MonoBehaviour
    {
        [Tooltip("Transform representing the water level.")]
        [SerializeField] private Transform _waterLevel;

        public float WaterLevel => _waterLevel.position.y;
    }
}