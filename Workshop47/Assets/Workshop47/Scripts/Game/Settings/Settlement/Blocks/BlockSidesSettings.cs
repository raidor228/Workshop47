using UnityEngine;

namespace Workshop47.Scripts.Game.Settings.Settlement.Blocks
{
    [CreateAssetMenu(fileName = "BlockSidesSettings", menuName = "Game Settings/World/Blocks/New Block Sides Settings")]
    public class BlockSidesSettings : BlockSettings
    {
        [field: SerializeField] public Vector2 PixelOffsetUp { get; private set; }
        [field: SerializeField] public Vector2 PixelOffsetDown { get; private set; }
        
        public override Vector2 GetPixelOffsets(Vector3Int normal)
        {
            if (normal == Vector3Int.up)
            {
                return PixelOffsetUp;
            }

            if (normal == Vector3Int.down)
            {
                return PixelOffsetDown;
            }

            return base.GetPixelOffsets(normal);
        }
    }
}