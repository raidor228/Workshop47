namespace Workshop47.Scripts.Game.Gameplay.Input.States
{
    public class RtsModeState : ModeState
    {
        public RtsModeState(Fsm.Fsm fsm, InputContextManager inputContextManager) : 
            base(fsm, inputContextManager)
        {
        }
        
        public override void Enter()
        {
            inputContextManager.EnableOnly(InputModuleType.Rts);
        }

        public override void Exit()
        {
            inputContextManager.Disable(InputModuleType.All);
        }
    }
}