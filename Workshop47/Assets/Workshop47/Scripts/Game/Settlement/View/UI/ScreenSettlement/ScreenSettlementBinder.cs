using UnityEngine;
using UnityEngine.UI;
using Workshop47.Scripts.MVVM.UI;

namespace Workshop47.Scripts.Game.Settlement.View.UI.ScreenSettlement
{
    public class ScreenSettlementBinder : WindowBinder<ScreenSettlementViewModel>
    {
        [SerializeField] private Button _btnGoToMenu;

        private void OnEnable()
        {
            _btnGoToMenu.onClick.AddListener(OnGoToMainMenuButtonClicked);
        }

        private void OnDisable()
        {
            _btnGoToMenu.onClick.RemoveListener(OnGoToMainMenuButtonClicked);
        }

        private void OnGoToMainMenuButtonClicked()
        {
            ViewModel.RequestGoToMainMenu();
        }
    }
}