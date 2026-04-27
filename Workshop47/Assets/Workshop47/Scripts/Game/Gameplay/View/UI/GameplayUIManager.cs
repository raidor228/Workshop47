using System.Collections.Generic;
using Melador.PlayerInput;
using R3;
using Workshop47.Scripts.DI;
using Workshop47.Scripts.Game.Common;
using Workshop47.Scripts.Game.Gameplay.Services;
using Workshop47.Scripts.Game.Gameplay.View.Interactable;
using Workshop47.Scripts.Game.Gameplay.View.UI.ScreenControlHub;
using Workshop47.Scripts.Game.Gameplay.View.UI.ScreenGameplay;
using Workshop47.Scripts.MVVM.UI;

namespace Workshop47.Scripts.Game.Gameplay.View.UI
{
    public class GameplayUIManager : UIManager
    {
        private readonly Subject<List<IInteractable>> _onInteractablesOverlap;
        private readonly PlayerInputProvider _playerInputProvider;
        private readonly Subject<Unit> _exitSceneRequest;

        public GameplayUIManager(DIContainer container) : base(container)
        {
            _exitSceneRequest = container.Resolve<Subject<Unit>>(AppConstants.EXIT_SCENE_REQUEST_TAG);
            _playerInputProvider = container.Resolve<PlayerInputProvider>();
            _onInteractablesOverlap = container.Resolve<PlayerService>().Player.CurrentValue.OnInteractablesOverlap;

            var openShopRequest = container.Resolve<Subject<Unit>>(AppConstants.OPEN_SHOP_REQUEST_TAG);
            openShopRequest.Subscribe(e => OpenScreenControlHub());
        }
        
        public ScreenGameplayViewModel OpenScreenGameplay()
        {
            var viewModel = new ScreenGameplayViewModel(this, _playerInputProvider.InteractionsInput, 
                _onInteractablesOverlap);
            var rootUI = Container.Resolve<UIGameplayRootViewModel>();

            rootUI.OpenScreen(viewModel);
            return viewModel;
        }
        
        public ScreenControlHubViewModel OpenScreenControlHub()
        {
            var viewModel = new ScreenControlHubViewModel(this);
            var rootUI = Container.Resolve<UIGameplayRootViewModel>();

            rootUI.OpenScreen(viewModel);
            return viewModel;
        }
    }
}