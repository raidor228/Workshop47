using UnityEngine;

namespace Workshop47.Scripts.Game.Gameplay.View.Buildings
{
    public class BuildingBinder : MonoBehaviour
    {
        private BuildingViewModel _viewModel;

        public void Bind(BuildingViewModel viewModel)
        {
            _viewModel = viewModel;

            transform.position = viewModel.Position.CurrentValue;
            transform.rotation = Quaternion.Euler(viewModel.Rotation.CurrentValue);
        }
    }
}