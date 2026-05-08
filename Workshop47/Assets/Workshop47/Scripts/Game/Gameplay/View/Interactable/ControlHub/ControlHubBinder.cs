using UnityEngine;

namespace Workshop47.Scripts.Game.Gameplay.View.Interactable.ControlHub
{
    public class ControlHubBinder : MonoBehaviour, IInteractable
    {
        public Transform RootTransform => transform;
        public int UniqueId => -4;
        public string Title => "Control Hub";
        
        public void Interact()
        {
            
        }
    }
}