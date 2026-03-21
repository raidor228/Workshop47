using R3;
using UnityEngine;

namespace Workshop47.Scripts.Game.MainMenu.Root.View
{
    public class WorldMainMenuRootBinder : MonoBehaviour
    {
        private readonly CompositeDisposable _disposables = new();

        private WorldMainMenuRootViewModel _viewModel;
        
        public void Bind(WorldMainMenuRootViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}