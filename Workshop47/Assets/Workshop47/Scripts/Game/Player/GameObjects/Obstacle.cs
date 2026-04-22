using UnityEngine;

namespace Melador.PlayerController.GameObjects
{
    public class Obstacle : MonoBehaviour
    {
        [Tooltip("Transform representing the obstacle level.")]
        [SerializeField] private Transform _obstacleLevel;

        public float ObstacleLevel => _obstacleLevel.position.y;
    }
}