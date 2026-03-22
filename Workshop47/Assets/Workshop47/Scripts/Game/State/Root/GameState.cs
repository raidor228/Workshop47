using System.Linq;
using ObservableCollections;
using Workshop47.Scripts.Game.State.GameResources;
using Workshop47.Scripts.Game.State.Maps;
using R3;

namespace Workshop47.Scripts.Game.State.Root
{
    public class GameState
    {
        public readonly ReactiveProperty<int> CurrentMapId = new();
        public readonly ReactiveProperty<int> ControllableEntityId = new();
        
        public ObservableList<Resource> Resources { get; } = new();
        public ObservableList<Map> Maps { get; } = new();

        private readonly GameStateData _gameStateData;

        public GameState(GameStateData gameStateData)
        {
            _gameStateData = gameStateData;
            
            InitMaps(gameStateData);
            InitResources(gameStateData);
            
            CurrentMapId.Subscribe(newCurrentMapId => { gameStateData.CurrentMapId = newCurrentMapId; });
            ControllableEntityId.Subscribe(newControllableEntityId => { gameStateData.ControllableEntityId = newControllableEntityId; });
        }

        public int CreateEntityId()
        {
            return _gameStateData.CreateEntityId();
        }
        
        private void InitMaps(GameStateData gameStateData)
        {
            gameStateData.Maps.ForEach(mapOrigin => Maps.Add(new Map(mapOrigin)));
            
            Maps.ObserveAdd().Subscribe(e =>
            {
                var addedMap = e.Value;
                gameStateData.Maps.Add(addedMap.Origin);
            });
            
            Maps.ObserveRemove().Subscribe(e =>
            {
                var removedMap = e.Value;
                var removedMapState = gameStateData.Maps.FirstOrDefault(b => b.Id == removedMap.Id);
                gameStateData.Maps.Remove(removedMapState);
            });
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