using UnityEngine;
using Workshop47.Scripts.Game.Gameplay.Services.Events;

namespace Workshop47.Scripts.Game.Gameplay.View.Interactable
{
    public class Interactable : MonoBehaviour, IInteractable
    {
        public Transform RootTransform => transform;
        public int UniqueId => _interactableData.UniqueId;
        public string Title => _interactableData.Name;

        private IInteractableData _interactableData;

        private void Awake()
        {
            _interactableData = GetComponent<IInteractableData>();
        }

        public void Interact()
        {
            var interactionEvent = new InteractionEvent(Title, UniqueId);
            EventsHandler.Send(interactionEvent);
        }
    }
}