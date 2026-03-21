using R3;
using UnityEngine;
using Workshop47.Scripts.DI;
using Workshop47.Scripts.Game.Common;
using Workshop47.Scripts.Game.Settlement.View.UI.ScreenSettlement;
using Workshop47.Scripts.MVVM.UI;

namespace Workshop47.Scripts.Game.Settlement.View.UI
{
    public class SettlementUIManager : UIManager
    {
        private readonly Subject<Unit> _exitSceneRequest;

        public SettlementUIManager(DIContainer container) : base(container)
        {
            _exitSceneRequest = container.Resolve<Subject<Unit>>(AppConstants.EXIT_SCENE_REQUEST_TAG);
        }
        
        public ScreenSettlementViewModel OpenScreenSettlement()
        {
            var viewModel = new ScreenSettlementViewModel(this, _exitSceneRequest);
            var rootUI = Container.Resolve<UISettlementRootViewModel>();

            rootUI.OpenScreen(viewModel);
            return viewModel;
        }
    }
}