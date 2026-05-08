using R3;
using Workshop47.Scripts.Game.Gameplay.Input;
using Workshop47.Scripts.Game.Gameplay.Input.States;
using Workshop47.Scripts.Game.Gameplay.Services;
using Workshop47.Scripts.MVVM.UI;

namespace Workshop47.Scripts.Game.Gameplay.View.UI.ScreenControlHub
{
    public class ScreenControlHubViewModel : WindowViewModel
    {
        public override string Id => "ScreenControlHub";
        
        private readonly GameplayUIManager _uiManager;
        private readonly PlayerService _playerService;
        private readonly InputModesHandler _inputModesHandler;
        
        public ScreenControlHubViewModel(GameplayUIManager uiManager, PlayerService playerService, 
            InputModesHandler inputModesHandler)
        {
            _uiManager = uiManager;
            _playerService = playerService;
            _inputModesHandler = inputModesHandler;
        }

        public void OnRequestCloseWindow()
        {
            RequestClose();
        }

        public void OnRequestEnterRtsMode()
        {
            _inputModesHandler.SetState<RtsModeState>();
            var viewModel = _uiManager.OpenScreenRtsMode();
            _playerService.SwitchRtsMode();
            viewModel.CloseRequested.Subscribe(_ =>
            {
                _inputModesHandler.SetState<UIModeState>();
                _playerService.SwitchRtsMode();
            });
        }
    }
}