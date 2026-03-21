using UnityEngine;
using UnityEngine.UI;
using Workshop47.Scripts.MVVM.UI;

namespace Workshop47.Scripts.Game.MainMenu.View.UI.ScreenMainMenu
{
    public class ScreenMainMenuBinder : WindowBinder<ScreenMainMenuViewModel>
    {
        [SerializeField] private Button _btnGoToSettlement;

        private void OnEnable()
        {
            _btnGoToSettlement.onClick.AddListener(OnGoToSettlementButtonClicked);
        }

        private void OnDisable()
        {
            _btnGoToSettlement.onClick.RemoveListener(OnGoToSettlementButtonClicked);
        }

        private void OnGoToSettlementButtonClicked()
        {
            ViewModel.RequestGoToSettlement();
        }
    }
}