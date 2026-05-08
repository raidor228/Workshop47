using R3;
using UnityEngine;
using Workshop47.Scripts.DI;
using Workshop47.Scripts.Game.Common;
using Workshop47.Scripts.Game.Gameplay.Root.View;
using Workshop47.Scripts.Game.Gameplay.View.UI;
using Workshop47.Scripts.Game.MainMenu.Root;
using Workshop47.Scripts.Game.Root;

namespace Workshop47.Scripts.Game.Gameplay.Root
{
    public class GameplayEntryPoint : MonoBehaviour
    {
        [SerializeField] private UIGameplayRootBinder _sceneUIRootPrefab;
        [SerializeField] private WorldGameplayRootBinder _worldRootBinder;

        private readonly CompositeDisposable _disposable = new();
        
        public Observable<GameplayExitParams> Run(DIContainer gameplayContainer, GameplayEnterParams enterParams)
        {
            GameplayRegistrations.Register(gameplayContainer, enterParams);
            var gameplayViewModelsContainer = new DIContainer(gameplayContainer);
            GameplayViewModelsRegistrations.Register(gameplayViewModelsContainer);
            
            InitWorld(gameplayViewModelsContainer);
            InitUI(gameplayViewModelsContainer);

            var mainMenuEnterParams = new MainMenuEnterParams();
            var exitParams = new GameplayExitParams(mainMenuEnterParams);
            var exitSceneRequest = gameplayContainer.Resolve<Subject<Unit>>(AppConstants.EXIT_SCENE_REQUEST_TAG);
            var exitToMainMenuSceneSignal = exitSceneRequest.Select(_ => exitParams);

            return exitToMainMenuSceneSignal;
        }
        
        private void InitWorld(DIContainer viewsContainer)
        {
            _worldRootBinder.Bind(viewsContainer.Resolve<WorldGameplayRootViewModel>());
        }

        private void InitUI(DIContainer viewsContainer)
        {
            var uiRoot = viewsContainer.Resolve<UIRootView>();
            var uiSceneRootBinder = Instantiate(_sceneUIRootPrefab);
            uiRoot.AttachSceneUI(uiSceneRootBinder.gameObject);
            
            var uiSceneRootViewModel = viewsContainer.Resolve<UIGameplayRootViewModel>();
            uiSceneRootBinder.Bind(uiSceneRootViewModel);

            _disposable.Add(uiSceneRootViewModel);
            _disposable.Add(viewsContainer.Resolve<GameplayUIManager>());
        }

        private void OnDestroy()
        {
            _disposable?.Dispose();
        }
    }
}