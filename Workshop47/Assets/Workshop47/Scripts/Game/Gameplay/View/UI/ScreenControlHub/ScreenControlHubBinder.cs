using UnityEngine;
using UnityEngine.UI;
using Workshop47.Scripts.MVVM.UI;

namespace Workshop47.Scripts.Game.Gameplay.View.UI.ScreenControlHub
{
    public class ScreenControlHubBinder : WindowBinder<ScreenControlHubViewModel>
    {
        [SerializeField] private Button _btnEnterRtsMode;
        [SerializeField] private Button _btnCloseWindow;

        private void OnEnable()
        {
            _btnCloseWindow.onClick.AddListener(OnCloseWindowButtonClicked);
            _btnEnterRtsMode.onClick.AddListener(OnEnterRtsModeButtonClicked);
        }

        private void OnDisable()
        {
            _btnCloseWindow.onClick.RemoveListener(OnCloseWindowButtonClicked);
            _btnEnterRtsMode.onClick.RemoveListener(OnEnterRtsModeButtonClicked);
        }

        private void OnCloseWindowButtonClicked()
        {
            ViewModel.OnRequestCloseWindow();
            Close();
        }
        
        private void OnEnterRtsModeButtonClicked()
        {
            ViewModel.OnRequestEnterRtsMode();
        }
    }
}