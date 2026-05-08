using R3;
using Workshop47.Scripts.MVVM.UI;

namespace Workshop47.Scripts.Game.MainMenu.View.UI.ScreenMainMenu
{
    public class ScreenMainMenuViewModel : WindowViewModel
    {
        public override string Id => "ScreenMainMenu";

        private readonly MainMenuUIManager _uiManager;
        private readonly Subject<Unit> _exitSceneRequest;

        public ScreenMainMenuViewModel(MainMenuUIManager uiManager, Subject<Unit> exitSceneRequest)
        {
            _uiManager = uiManager;
            _exitSceneRequest = exitSceneRequest;
        }

        public void RequestGoToGameplay()
        {
            _exitSceneRequest.OnNext(Unit.Default);
        }
    }
}