using System;
using System.Collections.Generic;
using UnityEngine;
using Workshop47.Scripts.Game.State.Chunks;

namespace Workshop47.Scripts.Game.Settings.Gameplay.Entities.Buildings
{
    [CreateAssetMenu(fileName = "BuildingViewSettings", menuName = "Game Settings/Entities/Buildings/New Building View Settings")]
    public class BuildingViewSettings : ScriptableObject
    {
        [field: SerializeField] public List<BlockEntry> Blocks { get; private set; }
        
#if UNITY_EDITOR
        public void SetBlocks(List<BlockEntry> blocks)
        {
            Blocks = blocks;
        }
#endif
    }

    [Serializable]
    public class BlockEntry
    {
        [field: SerializeField] public Vector3Int Position { get; private set; }
        [field: SerializeField] public BlockType BlockType { get; private set; }

#if UNITY_EDITOR
        public void Initialize(Vector3Int position, BlockType blockType)
        {
            Position = position;
            BlockType = blockType;
        }
#endif
    }
}