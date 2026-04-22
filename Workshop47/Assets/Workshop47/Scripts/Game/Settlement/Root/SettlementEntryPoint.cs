using System;
using Workshop47.Scripts.DI;
using Workshop47.Scripts.Game.Common;
using Workshop47.Scripts.Game.MainMenu.Root;
using Workshop47.Scripts.Game.Root;
using Workshop47.Scripts.Game.Settlement.Root.View;
using Workshop47.Scripts.Game.Settlement.View.UI;
using UnityEngine;
using R3;
using Workshop47.Scripts.Game.Settlement.Services;

namespace Workshop47.Scripts.Game.Settlement.Root
{
    public class SettlementEntryPoint : MonoBehaviour
    {
        [SerializeField] private UISettlementRootBinder _sceneUIRootPrefab;
        [SerializeField] private WorldSettlementRootBinder _worldRootBinder;
        
        public Observable<SettlementExitParams> Run(DIContainer settlementContainer, SettlementEnterParams enterParams)
        {
            SettlementRegistrations.Register(settlementContainer, enterParams);
            var settlementViewModelsContainer = new DIContainer(settlementContainer);
            SettlementViewModelsRegistrations.Register(settlementViewModelsContainer);
            
            InitWorld(settlementViewModelsContainer);
            InitUI(settlementViewModelsContainer);

            var mainMenuEnterParams = new MainMenuEnterParams();
            var exitParams = new SettlementExitParams(mainMenuEnterParams);
            var exitSceneRequest = settlementContainer.Resolve<Subject<Unit>>(AppConstants.EXIT_SCENE_REQUEST_TAG);
            var exitToMainMenuSceneSignal = exitSceneRequest.Select(_ => exitParams);

            return exitToMainMenuSceneSignal;
        }
        
        private void InitWorld(DIContainer viewsContainer)
        {
            _worldRootBinder.Bind(viewsContainer.Resolve<WorldSettlementRootViewModel>());
        }

        private void InitUI(DIContainer viewsContainer)
        {
            var uiRoot = viewsContainer.Resolve<UIRootView>();
            var uiSceneRootBinder = Instantiate(_sceneUIRootPrefab);
            uiRoot.AttachSceneUI(uiSceneRootBinder.gameObject);
            
            var uiSceneRootViewModel = viewsContainer.Resolve<UISettlementRootViewModel>();
            uiSceneRootBinder.Bind(uiSceneRootViewModel);
            
            var uiManager = viewsContainer.Resolve<SettlementUIManager>();
            uiManager.OpenScreenSettlement();
        }
    }
}