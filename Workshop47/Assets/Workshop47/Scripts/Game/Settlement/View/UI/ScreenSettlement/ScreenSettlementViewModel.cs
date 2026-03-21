using R3;
using Workshop47.Scripts.MVVM.UI;

namespace Workshop47.Scripts.Game.Settlement.View.UI.ScreenSettlement
{
    public class ScreenSettlementViewModel : WindowViewModel
    {
        public override string Id => "ScreenSettlement";
        
        private readonly SettlementUIManager _uiManager;
        private readonly Subject<Unit> _exitSceneRequest;

        public ScreenSettlementViewModel(SettlementUIManager uiManager, Subject<Unit> exitSceneRequest)
        {
            _uiManager = uiManager;
            _exitSceneRequest = exitSceneRequest;
        }

        public void RequestGoToMainMenu()
        {
            _exitSceneRequest.OnNext(Unit.Default);
        }
    }
}