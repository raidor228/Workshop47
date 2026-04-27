using UnityEngine;
using Workshop47.Scripts.DI;
using Workshop47.Scripts.Game.MainMenu.Root.View;
using Workshop47.Scripts.Game.Root;
using Workshop47.Scripts.Game.MainMenu.View.UI;
using Workshop47.Scripts.Game.Common;
using R3;
using Workshop47.Scripts.Game.Gameplay.Root;

namespace Workshop47.Scripts.Game.MainMenu.Root
{
    public class MainMenuEntryPoint : MonoBehaviour
    {
        [SerializeField] private UIMainMenuRootBinder _sceneUIRootPrefab;
        [SerializeField] private WorldMainMenuRootBinder _worldRootBinder;
        
        public Observable<MainMenuExitParams> Run(DIContainer mainMenuContainer, MainMenuEnterParams enterParams)
        {
            MainMenuRegistrations.Register(mainMenuContainer, enterParams);
            var mainMenuViewModelsContainer = new DIContainer(mainMenuContainer);
            MainMenuViewModelsRegistrations.Register(mainMenuViewModelsContainer);
            
            mainMenuViewModelsContainer.Resolve<UIMainMenuRootViewModel>();
            
            InitWorld(mainMenuViewModelsContainer);
            InitUI(mainMenuViewModelsContainer);
            
            var gameplayEnterParams = new GameplayEnterParams(0);
            var exitParams = new MainMenuExitParams(gameplayEnterParams);
            var exitSceneRequest = mainMenuContainer.Resolve<Subject<Unit>>(AppConstants.EXIT_SCENE_REQUEST_TAG);
            var exitToMainMenuSceneSignal = exitSceneRequest.Select(_ => exitParams);

            return exitToMainMenuSceneSignal;
        }
        
        private void InitWorld(DIContainer viewsContainer)
        {
            _worldRootBinder.Bind(viewsContainer.Resolve<WorldMainMenuRootViewModel>());
        }

        private void InitUI(DIContainer viewsContainer)
        {
            var uiRoot = viewsContainer.Resolve<UIRootView>();
            var uiSceneRootBinder = Instantiate(_sceneUIRootPrefab);
            uiRoot.AttachSceneUI(uiSceneRootBinder.gameObject);
            
            var uiSceneRootViewModel = viewsContainer.Resolve<UIMainMenuRootViewModel>();
            uiSceneRootBinder.Bind(uiSceneRootViewModel);
            
            var uiManager = viewsContainer.Resolve<MainMenuUIManager>();
            uiManager.OpenScreenMainMenu();
        }
    }
}