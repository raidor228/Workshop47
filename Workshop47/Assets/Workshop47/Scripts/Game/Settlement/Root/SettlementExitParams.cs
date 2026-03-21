using Workshop47.Scripts.Game.MainMenu.Root;

namespace Workshop47.Scripts.Game.Settlement.Root
{
    public class SettlementExitParams
    {
        public MainMenuEnterParams MainMenuEnterParams { get; }

        public SettlementExitParams(MainMenuEnterParams mainMenuEnterParams)
        {
            MainMenuEnterParams = mainMenuEnterParams;
        }
    }
}