using System.Collections.Generic;
using System.IO;
using UnityEngine;
using fNbt;
using Workshop47.Scripts.Game.State.Chunks;

public static class MinecraftStructureParser
{
    public static List<BlockData> ParseStructure(string path)
    {
        List<BlockData> blocks = new();
        
        if (string.IsNullOrEmpty(path) || !File.Exists(path))
        {
            Debug.LogError("Invalid file path");
            return new List<BlockData>();
        }

        using (var stream = File.OpenRead(path))
        {
            var file = new NbtFile();
            file.LoadFromStream(stream, NbtCompression.GZip);

            var root = file.RootTag;

            // palette
            var paletteList = root["palette"] as NbtList;

            List<string> palette = new List<string>();
            foreach (NbtCompound entry in paletteList)
            {
                palette.Add(entry["Name"].StringValue);
            }

            // blocks
            var blocksList = root["blocks"] as NbtList;

            foreach (NbtCompound block in blocksList)
            {
                var posList = block["pos"] as NbtList;

                Vector3Int pos = new Vector3Int(
                    posList[0].IntValue,
                    posList[1].IntValue,
                    posList[2].IntValue
                );

                int state = block["state"].IntValue;
                string blockId = palette[state];

                BlockType blockType = ConvertBlock(blockId);
                if (blockType != BlockType.Air)
                {
                    blocks.Add(new BlockData(pos, blockType));
                }
            }
        }

        return blocks;
    }
    
    private static BlockType ConvertBlock(string name)
    {
        BlockType blockType = name switch
        {
            "minecraft:air" => BlockType.Air,
            "minecraft:stone" => BlockType.Stone,
            "minecraft:dirt" => BlockType.Dirt,
            "minecraft:grass_block" => BlockType.Grass,
            "minecraft:oak_planks" => BlockType.OakPlanks,
            "minecraft:oak_log" => BlockType.OakLog,
            _ => BlockType.Air
        };
            
        return blockType;
    }
}