using Workshop47.Scripts.Game.MainMenu.Root;

namespace Workshop47.Scripts.Game.World.Root
{
    public class WorldExitParams
    {
        public MainMenuEnterParams MainMenuEnterParams { get; }

        public WorldExitParams(MainMenuEnterParams mainMenuEnterParams)
        {
            MainMenuEnterParams = mainMenuEnterParams;
        }
    }
}