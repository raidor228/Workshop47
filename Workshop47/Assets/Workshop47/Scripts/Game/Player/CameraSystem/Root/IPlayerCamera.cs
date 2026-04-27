using UnityEngine;

namespace Melador.PlayerController.Root
{
    public interface IPlayerCamera
    {
        public Vector3 GetForward();
        public Vector3 TransformDirection(Vector3 direction);
        public Vector3 GetPosition();
        public bool IsInWater();
    }
}