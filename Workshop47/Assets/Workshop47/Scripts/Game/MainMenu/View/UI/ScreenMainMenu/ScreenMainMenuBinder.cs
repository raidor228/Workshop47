using UnityEngine;
using UnityEngine.UI;
using Workshop47.Scripts.MVVM.UI;

namespace Workshop47.Scripts.Game.MainMenu.View.UI.ScreenMainMenu
{
    public class ScreenMainMenuBinder : WindowBinder<ScreenMainMenuViewModel>
    {
        [SerializeField] private Button _btnGoToGameplay;

        private void OnEnable()
        {
            _btnGoToGameplay.onClick.AddListener(OnGoToGameplayButtonClicked);
        }

        private void OnDisable()
        {
            _btnGoToGameplay.onClick.RemoveListener(OnGoToGameplayButtonClicked);
        }

        private void OnGoToGameplayButtonClicked()
        {
            ViewModel.RequestGoToGameplay();
        }
    }
}