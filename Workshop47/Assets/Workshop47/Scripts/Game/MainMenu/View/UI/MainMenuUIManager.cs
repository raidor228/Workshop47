using R3;
using Workshop47.Scripts.DI;
using Workshop47.Scripts.Game.Common;
using Workshop47.Scripts.Game.MainMenu.View.UI.ScreenMainMenu;
using Workshop47.Scripts.Game.Settlement.View.UI;
using Workshop47.Scripts.Game.Settlement.View.UI.ScreenSettlement;
using Workshop47.Scripts.MVVM.UI;

namespace Workshop47.Scripts.Game.MainMenu.View.UI
{
    public class MainMenuUIManager : UIManager
    {
        private readonly Subject<Unit> _exitSceneRequest;

        public MainMenuUIManager(DIContainer container) : base(container)
        {
            _exitSceneRequest = container.Resolve<Subject<Unit>>(AppConstants.EXIT_SCENE_REQUEST_TAG);
        }
        
        public ScreenMainMenuViewModel OpenScreenMainMenu()
        {
            var viewModel = new ScreenMainMenuViewModel(this, _exitSceneRequest);
            var rootUI = Container.Resolve<UIMainMenuRootViewModel>();

            rootUI.OpenScreen(viewModel);
            return viewModel;
        }
    }
}