using UnityEngine;
using R3;

namespace Workshop47.Scripts.Game.Settlement.Root.View
{
    public class WorldSettlementRootBinder : MonoBehaviour
    {
        private readonly CompositeDisposable _disposables = new();

        private WorldSettlementRootViewModel _viewModel;
        
        public void Bind(WorldSettlementRootViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}