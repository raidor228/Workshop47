using System.Linq;
using ObservableCollections;
using R3;
using Workshop47.Scripts.Game.State.GameResources;

namespace Workshop47.Scripts.Game.State
{
    public class GameState
    {
        public ObservableList<Resource> Resources { get; } = new();

        private readonly GameStateData _gameStateData;

        public GameState(GameStateData gameStateData)
        {
            _gameStateData = gameStateData;
            
            InitResources(gameStateData);
        }

        public int CreateEntityId()
        {
            return _gameStateData.CreateEntityId();
        }
        
        private void InitResources(GameStateData gameStateData)
        {
            gameStateData.Resources.ForEach(resourceData => Resources.Add(new Resource(resourceData)));
            
            Resources.ObserveAdd().Subscribe(e =>
            {
                var addedResource = e.Value;
                gameStateData.Resources.Add(addedResource.Origin);
            });
            
            Resources.ObserveRemove().Subscribe(e =>
            {
                var removedResource = e.Value;
                var removedResourceData = gameStateData.Resources.FirstOrDefault(b => b.ResourceType == removedResource.ResourceType);
                gameStateData.Resources.Remove(removedResourceData);
            });
        }
    }
}