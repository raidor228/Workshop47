using Workshop47.Scripts.Fsm;

namespace Workshop47.Scripts.Game.Gameplay.Input.States
{
    public class ModeState : FsmState
    {
        protected readonly InputContextManager inputContextManager;
        
        protected ModeState(Fsm.Fsm fsm, InputContextManager inputContextManager) : base(fsm)
        {
            this.inputContextManager = inputContextManager;
        }
    }
}