using UnityEngine;

namespace Workshop47.Scripts.Game.Settlement.View.Characters
{
    public class CharacterBinder : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;

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
    }
}