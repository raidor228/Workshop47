using Workshop47.Scripts.Game.State.Commands;

namespace Workshop47.Scripts.Game.Settlement.Commands
{
    public class CmdControlCharacter : ICommand
    {
        public readonly int CharacterId;

        public CmdControlCharacter(int characterId)
        {
            CharacterId = characterId;
        }
    }
}