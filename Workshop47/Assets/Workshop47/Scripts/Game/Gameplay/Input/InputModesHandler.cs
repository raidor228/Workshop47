using Workshop47.Scripts.Game.Gameplay.Input.States;

namespace Workshop47.Scripts.Game.Gameplay.Input
{
    public class InputModesHandler : Fsm.Fsm
    {
        private readonly InputContextManager _inputContextManager;
        
        public InputModesHandler(InputContextManager inputContextManager)
        {
            _inputContextManager = inputContextManager;
            
            AddState(new PlayerModeState(this, inputContextManager));
            AddState(new UIModeState(this, inputContextManager));
            AddState(new RtsModeState(this, inputContextManager));
            
            SetState<PlayerModeState>();
        }
    }
}