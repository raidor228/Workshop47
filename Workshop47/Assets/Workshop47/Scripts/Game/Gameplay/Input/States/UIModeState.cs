namespace Workshop47.Scripts.Game.Gameplay.Input.States
{
    public class UIModeState : ModeState
    {
        public UIModeState(Fsm.Fsm fsm, InputContextManager inputContextManager) : 
            base(fsm, inputContextManager)
        {
        }
        
        public override void Enter()
        {
            inputContextManager.Disable(InputModuleType.All);
        }
    }
}