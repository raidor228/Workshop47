using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Workshop47.Scripts.Game.Settings.Gameplay.Worlds;

namespace Workshop47.Scripts.World.Editor
{
    public static class MinecraftImporterMenu
    {
        [MenuItem("Tools/Workshop47/Import Minecraft World")]
        public static void Import()
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
    }
}