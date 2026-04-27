using Workshop47.Scripts.Game.State.Commands;

namespace Workshop47.Scripts.Game.Gameplay.Commands
{
    public class CmdCreateMap: ICommand
    {
        public readonly int MapId;
        
        public CmdCreateMap(int mapId)
        {
            MapId = mapId;
        }
    }
}