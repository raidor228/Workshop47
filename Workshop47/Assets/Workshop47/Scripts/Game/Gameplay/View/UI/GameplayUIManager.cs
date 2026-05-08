using R3;
using Workshop47.Scripts.DI;
using Workshop47.Scripts.Game.Common;
using Workshop47.Scripts.Game.Gameplay.Input;
using Workshop47.Scripts.Game.Gameplay.Input.States;
using Workshop47.Scripts.Game.Gameplay.Services;
using Workshop47.Scripts.Game.Gameplay.Services.Events;
using Workshop47.Scripts.Game.Gameplay.View.UI.ScreenControlHub;
using Workshop47.Scripts.Game.Gameplay.View.UI.ScreenRts;
using Workshop47.Scripts.MVVM.UI;

namespace Workshop47.Scripts.Game.Gameplay.View.UI
{
    public class GameplayUIManager : UIManager
    {
        private readonly InputModesHandler _inputModesHandler;
        private readonly PlayerService _playerService;
        private readonly Subject<Unit> _exitSceneRequest;

        private readonly CompositeDisposable _disposable = new();
        
        public GameplayUIManager(DIContainer container) : base(container)
        {
            _exitSceneRequest = container.Resolve<Subject<Unit>>(AppConstants.EXIT_SCENE_REQUEST_TAG);
            _inputModesHandler = container.Resolve<InputModesHandler>();
            _playerService = container.Resolve<PlayerService>();
            
            _disposable.Add(EventsHandler.Observe("controlhub").Subscribe(e =>
            {
                _inputModesHandler.SetState<UIModeState>();
                var viewModel = OpenScreenControlHub();
                viewModel.CloseRequested.Subscribe(_ =>
                {
                    _inputModesHandler.SetState<PlayerModeState>();
                });
            }));
        }

        public ScreenControlHubViewModel OpenScreenControlHub()
        {
            var viewModel = new ScreenControlHubViewModel(this, _playerService, _inputModesHandler);
            var rootUI = Container.Resolve<UIGameplayRootViewModel>();

            rootUI.OpenScreen(viewModel);
            return viewModel;
        }
        
        public ScreenRtsViewModel OpenScreenRtsMode()
        {
            var viewModel = new ScreenRtsViewModel(this, _inputModesHandler);
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