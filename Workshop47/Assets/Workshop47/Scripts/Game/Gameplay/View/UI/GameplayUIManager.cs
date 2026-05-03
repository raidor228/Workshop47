using Melador.PlayerInput;
using Melador.PlayerInput.Modules;
using R3;
using UnityEngine;
using Workshop47.Scripts.DI;
using Workshop47.Scripts.Game.Common;
using Workshop47.Scripts.Game.Gameplay.Services.Events;
using Workshop47.Scripts.Game.Gameplay.View.UI.ScreenControlHub;
using Workshop47.Scripts.MVVM.UI;

namespace Workshop47.Scripts.Game.Gameplay.View.UI
{
    public class GameplayUIManager : UIManager
    {
        private readonly PlayerInputProvider _playerInputProvider;
        private readonly Subject<Unit> _exitSceneRequest;

        private readonly CompositeDisposable _disposable = new();
        
        public GameplayUIManager(DIContainer container) : base(container)
        {
            _exitSceneRequest = container.Resolve<Subject<Unit>>(AppConstants.EXIT_SCENE_REQUEST_TAG);
            _playerInputProvider = container.Resolve<PlayerInputProvider>();

            _disposable.Add(EventsHandler.Observe("controlhub").Subscribe(e =>
            {
                _playerInputProvider.Disable(InputModuleType.All);
                var viewModel = OpenScreenControlHub();
                viewModel.CloseRequested.Take(1).Subscribe(_ =>
                {
                    _playerInputProvider.Enable(InputModuleType.All);
                });
            }));
        }
        
        public ScreenControlHubViewModel OpenScreenControlHub()
        {
            var viewModel = new ScreenControlHubViewModel(this);
            var rootUI = Container.Resolve<UIGameplayRootViewModel>();

            rootUI.OpenScreen(viewModel);
            return viewModel;
        }

        public override void Dispose()
        {
            _disposable.Dispose();
        }
    }
}