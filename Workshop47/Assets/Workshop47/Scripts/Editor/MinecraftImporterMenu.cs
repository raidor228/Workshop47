using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Workshop47.Scripts.Game.Settings.Gameplay.Entities.Buildings;
using Workshop47.Scripts.Game.Settings.Gameplay.Worlds;

namespace Workshop47.Scripts.World.Editor
{
    public static class MinecraftImporterMenu
    {
        [MenuItem("Tools/Workshop47/Import Minecraft World")]
        public static void ImportWorld()
        {
            string regionsPath = EditorUtility.OpenFolderPanel("Select Region Folder", "", "");
            string assetPath = EditorUtility.SaveFilePanelInProject(
                "Save World Asset",
                "New World",
                "asset",
                "Select location to save the world asset"
            );
            
            if (string.IsNullOrEmpty(regionsPath))
            {
                return;
            }

            var dict = MinecraftWorldParser.ParseWorld(regionsPath);
            var asset = ScriptableObject.CreateInstance<GameWorldSettings>();
            
            List<ChunkEntry> chunks = new List<ChunkEntry>();
            foreach (var kvp in dict)
            {
                var chunkEntry = new ChunkEntry();
                chunkEntry.Initialize(kvp.Key, kvp.Value);
                chunks.Add(chunkEntry);
            }
            
            asset.SetChunks(chunks);

            AssetDatabase.CreateAsset(asset, assetPath);
            AssetDatabase.SaveAssets();
        }

        [MenuItem("Tools/Workshop47/Import Minecraft Structure")]
        public static void ImportStructure()
        {
            string filePath = EditorUtility.OpenFilePanel("Select NBT Structure", "", "nbt");
            string assetPath = EditorUtility.SaveFilePanelInProject(
                "Save Structure Asset",
                "New Structure",
                "asset",
                "Select location to save the structure asset"
            );
            
            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            var blocks = MinecraftStructureParser.ParseStructure(filePath);
            var asset = ScriptableObject.CreateInstance<BuildingViewSettings>();

            List<BlockEntry> blockEntries = new List<BlockEntry>();
            foreach (var blockData in blocks)
            {
                var blockEntry = new BlockEntry();
                blockEntry.Initialize(blockData.Position, blockData.BlockType);
                blockEntries.Add(blockEntry);
            }
            
            asset.SetBlocks(blockEntries);

            AssetDatabase.CreateAsset(asset, assetPath);
            AssetDatabase.SaveAssets();
        }
    }
}