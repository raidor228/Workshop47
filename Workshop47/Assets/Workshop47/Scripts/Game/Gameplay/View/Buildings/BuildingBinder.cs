using UnityEngine;
using Workshop47.Scripts.Game.Gameplay.View.Interactable;

namespace Workshop47.Scripts.Game.Gameplay.View.Buildings
{
    public class BuildingBinder : MonoBehaviour, IInteractableData
    {
        [SerializeField] private BuildingRenderer _buildingRenderer;
        
        public int UniqueId => _viewModel.EntityId;
        public string Name => _viewModel.ConfigId;
        
        private BuildingViewModel _viewModel;

        public void Bind(BuildingViewModel viewModel)
        {
            _viewModel = viewModel;
            if (_buildingRenderer != null)
            {
                _buildingRenderer.Initialize(viewModel);
            }
            
            transform.position = viewModel.Position.CurrentValue;
            transform.rotation = Quaternion.Euler(viewModel.Rotation.CurrentValue);
        }
    }
}