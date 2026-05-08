using R3;
using UnityEngine;
using Workshop47.Scripts.Game.Gameplay.Input;
using Workshop47.Scripts.Game.Gameplay.Input.States;
using Workshop47.Scripts.MVVM.UI;

namespace Workshop47.Scripts.Game.Gameplay.View.UI.ScreenRts
{
    public class ScreenRtsViewModel : WindowViewModel
    {
        public override string Id => "ScreenRtsMode";

        private readonly GameplayUIManager _uiManager;
        private readonly InputModesHandler _inputModesHandler;
        
        public ScreenRtsViewModel(GameplayUIManager uiManager, InputModesHandler inputModesHandler)
        {
            _uiManager = uiManager;
            _inputModesHandler = inputModesHandler;
        }

        public void OnRequestExitFromRtsMode()
        {
            RequestClose();
            
            Debug.Log("Need to change camera to Player");
            var viewModel = _uiManager.OpenScreenControlHub();
            viewModel.CloseRequested.Subscribe(_ =>
            {
                _inputModesHandler.SetState<PlayerModeState>();
            });
        }
    }
}