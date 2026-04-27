using Melador.PlayerController.GameObjects;
using Melador.PlayerController.MovementController.States.Root;
using UnityEngine;

namespace Melador.PlayerController.MovementController.States.Handlers
{
    public class CollisionResponder
    {
        private readonly MovementStateContext _stateContext;

        public CollisionResponder(MovementStateContext stateContext)
        {
            _stateContext = stateContext;
        }

        public void OnTriggerEnter(Collider collider)
        {
            switch (collider.tag)
            {
                case TagsAndLayers.Ladder:
                    HandleLadderEnter(collider);
                    break;
                case TagsAndLayers.Water:
                    HandleWaterEnter(collider);
                    break;
            }
        }

        public void OnTriggerExit(Collider collider)
        {
            switch (collider.tag)
            {
                case TagsAndLayers.Ladder:
                    HandleLadderExit(collider);
                    break;
                case TagsAndLayers.Water:
                    HandleWaterExit(collider);
                    break;
            }
        }

        public void OnControllerColliderHit(ControllerColliderHit hit)
        {
        }

        #region OnTrigger

        private void HandleLadderEnter(Collider collider)
        {
            var box = collider as BoxCollider;
            if (box == null)
            {
                return;
            }
            
            Vector3 halfHeight = new(0, box.size.y / 2, 0);

            _stateContext.ClimbHandler.HasLadderCollision = true;
            var position = collider.transform.position;
            _stateContext.ClimbHandler.ClimbDirection =
                position + halfHeight - (position - halfHeight);
            _stateContext.ClimbHandler.LadderCollider = collider;
        }

        private void HandleLadderExit(Collider collider)
        {
            _stateContext.ClimbHandler.HasLadderCollision = false;
            _stateContext.ClimbHandler.ClimbDirection = Vector3.zero;
        }

        private void HandleWaterEnter(Collider collider)
        {
            if (collider.TryGetComponent(out Water water))
            {
                _stateContext.SwimHandler.IsSwimming = true;
                _stateContext.SwimHandler.CurrentWaterSurfaceLevel = water.WaterLevel;
            }
        }

        private void HandleWaterExit(Collider collider)
        {
            _stateContext.SwimHandler.IsSwimming = false;
        }

        #endregion
    }
}