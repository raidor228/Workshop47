using Workshop47.Scripts.Game.Root;

namespace Workshop47.Scripts.Game.Settlement.Root
{
    public class SettlementEnterParams : SceneEnterParams
    {
        public int MapId { get; }
        
        public SettlementEnterParams(int mapId) : base(Scenes.SETTLEMENT)
        {
            MapId = mapId;
        }
    }
}