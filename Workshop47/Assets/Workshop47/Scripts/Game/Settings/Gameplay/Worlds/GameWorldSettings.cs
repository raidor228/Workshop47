using System;
using System.Collections.Generic;
using UnityEngine;
using Workshop47.Scripts.Game.State.Chunks;

namespace Workshop47.Scripts.Game.Settings.Gameplay.Worlds
{
    [CreateAssetMenu(fileName = "New GameWorldSettings", menuName = "Game Settings/World/New Game World Settings")]
    public class GameWorldSettings : ScriptableObject
    {
        [field: SerializeField] public List<ChunkEntry> Chunks { get; private set; }
        
#if UNITY_EDITOR
        public void SetChunks(List<ChunkEntry> chunks)
        {
            Chunks = chunks;
        }
#endif
    }
    
    [Serializable]
    public class ChunkEntry
    {
        [field: SerializeField] public Vector2Int Position { get; private set; }
        [field: SerializeField] public List<BlockData> Blocks { get; private set; }
        
#if UNITY_EDITOR
        public void Initialize(Vector2Int position, List<BlockData> blocks)
        {
            Position = position;
            Blocks = blocks;
        }
#endif
    }
}