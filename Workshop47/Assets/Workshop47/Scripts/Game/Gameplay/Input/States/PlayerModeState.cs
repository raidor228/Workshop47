namespace Workshop47.Scripts.Game.Gameplay.Input.States
{
    public class PlayerModeState : ModeState
    {
        public PlayerModeState(Fsm.Fsm fsm, InputContextManager inputContextManager) : 
            base(fsm, inputContextManager)
        {
        }

        public override void Enter()
        {
            inputContextManager.EnableOnly(InputModuleType.Player);
        }

        public override void Exit()
        {
            inputContextManager.Disable(InputModuleType.All);
        }
    }
}