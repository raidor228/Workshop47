using UnityEngine;
using Workshop47.Scripts.Game.Gameplay.View.Interactable;

namespace Workshop47.Scripts.Game.Gameplay.View.Characters
{
    public class CharacterBinder : MonoBehaviour, IInteractable
    {
        [SerializeField] private CharacterController _characterController;
        
        public Transform RootTransform => transform;
        public int UniqueId => _viewModel.EntityId;
        public string Title => _viewModel.Name;
        
        private CharacterViewModel _viewModel;
        
        public void Bind(CharacterViewModel viewModel)
        {
            _viewModel = viewModel;
            
            var position = viewModel.Position.CurrentValue;
            transform.position = position;
            
            var rotation = viewModel.Rotation.CurrentValue;
            transform.rotation = Quaternion.Euler(rotation);
        }

        public void Move(Vector3 direction)
        {
            _characterController.Move(direction * _viewModel.Speed * Time.deltaTime);
            _viewModel.Moved.OnNext(transform.position);
        }
        
        public void Interact()
        {
            _viewModel.OnRequestInteract();
        }
    }
}