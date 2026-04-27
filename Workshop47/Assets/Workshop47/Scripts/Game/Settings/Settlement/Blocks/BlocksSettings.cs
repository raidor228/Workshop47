using System.Collections.Generic;
using UnityEngine;

namespace Workshop47.Scripts.Game.Settings.Settlement.Blocks
{
    [CreateAssetMenu(fileName = "BlocksSettings", menuName = "Game Settings/World/Blocks/New Blocks Settings")]
    public class BlocksSettings : ScriptableObject
    {
        [field: SerializeField] public List<BlockSettings> Blocks { get; private set; }
    }
}