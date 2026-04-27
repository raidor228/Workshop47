using UnityEngine;

namespace Workshop47.Scripts.Game.Gameplay.View.Interactable
{
    public interface IInteractable
    {
        Transform RootTransform { get; }
        int UniqueId { get; }
        string Title { get; }
        
        void Interact();
    }
}