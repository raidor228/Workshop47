namespace Workshop47.Scripts.Game.State
{
    public class GameStateData
    {
        public int GlobalEntityId { get; set; }

        public int CreateEntityId()
        {
            return GlobalEntityId++;
        }
    }
}