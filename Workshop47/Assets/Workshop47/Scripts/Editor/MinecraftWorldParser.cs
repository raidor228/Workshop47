using System;
using System.Collections.Generic;
using System.IO;
using fNbt;
using UnityEngine;
using Workshop47.Scripts.Game.Settings.Gameplay.Blocks;
using Workshop47.Scripts.Game.State.Chunks;

namespace Workshop47.Scripts.World.Editor
{
    public static class MinecraftWorldParser
    {
        public static Dictionary<Vector2Int, List<BlockData>> ParseWorld(string regionsPath)
        {
            var result = new Dictionary<Vector2Int, List<BlockData>>();
            
            var files = Directory.GetFiles(regionsPath, "r.*.*.mca");
            foreach (var file in files)
            {
                ParseRegion(file, result);
            }

            return result;
        }

        private static void ParseRegion(string filePath, Dictionary<Vector2Int, List<BlockData>> dict)
        {
            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            using var br = new BinaryReader(fs);

            byte[] header = br.ReadBytes(4096);

            for (int i = 0; i < 1024; i++)
            {
                int offset = (header[i * 4] << 16) | (header[i * 4 + 1] << 8) | header[i * 4 + 2];
                if (offset == 0)
                {
                    continue;
                }

                long chunkPos = offset * 4096;
                fs.Seek(chunkPos, SeekOrigin.Begin);

                int length = ReadInt32BigEndian(br);
                byte compressionType = br.ReadByte();

                byte[] compressedData = br.ReadBytes(length - 1);

                using var ms = new MemoryStream(compressedData);

                var compression = compressionType switch
                {
                    1 => NbtCompression.GZip,
                    2 => NbtCompression.ZLib,
                    _ => NbtCompression.None
                };

                var nbt = new NbtFile();
                nbt.LoadFromStream(ms, compression);

                ParseChunk(nbt, dict);
            }
        }

        private static void ParseChunk(NbtFile nbt, Dictionary<Vector2Int, List<BlockData>> dict)
        {
            var root = nbt.RootTag;
            var level = root["Level"] ?? root;

            int chunkX = level["xPos"].IntValue;
            int chunkZ = level["zPos"].IntValue;

            var sections = level["sections"] ?? level["Sections"];
            if (sections == null)
            {
                return;
            }

            var blocks = new List<BlockData>();
            
            var sectionsList = sections as NbtList;
            if (sectionsList == null)
            {
                return;
            }

            foreach (var tag in sectionsList)
            {
                if (tag is not NbtCompound section)
                {
                    continue;
                }

                if (!section.Contains("block_states"))
                {
                    continue;
                }

                int sectionY = section["Y"].IntValue;

                var blockStatesTag = section["block_states"];
                var palette = blockStatesTag["palette"] as NbtList;
                var dataTag = blockStatesTag["data"] as NbtLongArray;

                if (palette == null)
                {
                    continue;
                }

                long[] data = dataTag?.Value;
                
                int bitsPerBlock = data != null
                    ? (data.Length * 64) / 4096
                    : 0;

                bitsPerBlock = Math.Max(bitsPerBlock, 4);

                for (int i = 0; i < 4096; i++)
                {
                    int paletteIndex;
                    if (data == null)
                    {
                        paletteIndex = 0;
                    }
                    else
                    {
                        paletteIndex = GetBlockIndex(data, i, bitsPerBlock);

                        if (paletteIndex < 0 || paletteIndex >= palette.Count)
                        {
                            paletteIndex = 0;
                        }
                    }
                    
                    string name = ((NbtCompound)palette[paletteIndex])["Name"].StringValue;
                    
                    int x = i & 15;
                    int z = (i >> 4) & 15;
                    int y = (i >> 8) & 15;

                    int worldY = sectionY * 16 + y;
                    if (worldY >= 0 && worldY < ChunkData.ChunkHeight)
                    {
                        BlockType blockType = ConvertBlock(name);
                        if (blockType != BlockType.Air)
                        {
                            var blockData = new BlockData(new Vector3Int(x, worldY, z), blockType);
                            blocks.Add(blockData);
                        }
                    }
                }
            }

            if (blocks.Count != 0)
            {
                dict[new Vector2Int(chunkX, chunkZ)] = blocks;
            }
        }

        private static int GetBlockIndex(long[] data, int index, int bitsPerBlock)
        {
            int bitIndex = index * bitsPerBlock;
            int longIndex = bitIndex / 64;
            int startBit = bitIndex % 64;

            long value = data[longIndex] >> startBit;

            int bitsLeft = 64 - startBit;
            if (bitsLeft < bitsPerBlock)
            {
                value |= data[longIndex + 1] << bitsLeft;
            }

            long mask = (1L << bitsPerBlock) - 1;
            return (int)(value & mask);
        }
        
        private static BlockType ConvertBlock(string name)
        {
            BlockType blockType = name switch
            {
                "minecraft:air" => BlockType.Air,
                "minecraft:stone" => BlockType.Stone,
                "minecraft:dirt" => BlockType.Dirt,
                "minecraft:grass_block" => BlockType.Grass,
                "minecraft:snow_block" => BlockType.SnowBlock,
                "minecraft:white_concrete" => BlockType.WhiteConcrete,
                "minecraft:glass" => BlockType.Glass,
                _ => BlockType.Air
            };
            
            return blockType;
        }

        private static int ReadInt32BigEndian(BinaryReader br)
        {
            var bytes = br.ReadBytes(4);
            return (bytes[0] << 24) | (bytes[1] << 16) | (bytes[2] << 8) | bytes[3];
        }
    }
}