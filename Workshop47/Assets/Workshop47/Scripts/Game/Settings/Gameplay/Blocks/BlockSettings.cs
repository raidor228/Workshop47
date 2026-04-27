using UnityEngine;
using Workshop47.Scripts.Game.State.Chunks;

namespace Workshop47.Scripts.Game.Settings.Gameplay.Blocks
{
    [CreateAssetMenu(fileName = "BlockSettings", menuName = "Game Settings/World/Blocks/New Block Settings")]
    public class BlockSettings : ScriptableObject
    {
        [field: SerializeField] public BlockType BlockType { get; private set; }
        [field: SerializeField] public Vector2 PixelsOffsets { get; private set; }

        public virtual Vector2 GetPixelOffsets(Vector3Int normal)
        {
            return PixelsOffsets;
        }
    }
}