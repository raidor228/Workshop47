using UnityEngine;
using Workshop47.Scripts.MVVM.UI;

namespace Workshop47.Scripts.Game.Gameplay.View.UI.ScreenControlHub
{
    public class ScreenControlHubViewModel : WindowViewModel
    {
        public override string Id => "ScreenControlHub";
        
        private readonly GameplayUIManager _uiManager;
        
        public ScreenControlHubViewModel(GameplayUIManager uiManager)
        {
            _uiManager = uiManager;
        }

        public void OnRequestCloseWindow()
        {
            RequestClose();
        }

        public void OnRequestEnterRtsMode()
        {
            Debug.Log("Request to enter RTS mode");
        }
    }
}