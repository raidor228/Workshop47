namespace Workshop47.Scripts.Game.State
{
    public class GameState
    {
        private readonly GameStateData _gameStateData;

        public GameState(GameStateData gameStateData)
        {
            _gameStateData = gameStateData;
        }

        public int CreateEntityId()
        {
            return _gameStateData.CreateEntityId();
        }
    }
}