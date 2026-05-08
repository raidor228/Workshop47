using System;
using UnityEngine;
using UnityEngine.UI;
using Workshop47.Scripts.MVVM.UI;

namespace Workshop47.Scripts.Game.Gameplay.View.UI.ScreenRts
{
    public class ScreenRtsBinder : WindowBinder<ScreenRtsViewModel>
    {
        [SerializeField] private Button _btnExitFromRtsMode;

        private void OnEnable()
        {
            _btnExitFromRtsMode.onClick.AddListener(OnExitFromRtsModeButtonClicked);
        }

        private void OnDisable()
        {
            _btnExitFromRtsMode.onClick.RemoveListener(OnExitFromRtsModeButtonClicked);
        }

        private void OnExitFromRtsModeButtonClicked()
        {
            ViewModel.OnRequestExitFromRtsMode();
            Close();
        }
    }
}