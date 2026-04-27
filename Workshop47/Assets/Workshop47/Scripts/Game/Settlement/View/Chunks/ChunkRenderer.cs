using System;
using System.Collections.Generic;
using UnityEngine;
using Workshop47.Scripts.Game.Settings.Settlement.Blocks;
using Workshop47.Scripts.Game.State.Chunks;

namespace Workshop47.Scripts.Game.Settlement.View.Chunks
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class ChunkRenderer : MonoBehaviour
    {
        [SerializeField] private MeshCollider _meshCollider;
        [SerializeField] private MeshFilter _meshFilter;
        
        private static int[] _triangles;

        private Mesh _chunkMesh;
        
        private ChunkViewModel _leftChunk;
        private ChunkViewModel _rightChunk;
        private ChunkViewModel _forwardChunk;
        private ChunkViewModel _backChunk;
        
        private ChunkViewModel _chunkViewModel;
        
        private readonly List<Vector3> _vertices = new List<Vector3>();
        private readonly List<Vector2> _uvs = new List<Vector2>();

        public void Initialize(ChunkViewModel chunkViewModel)
        {
            _chunkViewModel = chunkViewModel;

            _leftChunk = chunkViewModel.LeftChunk;
            _rightChunk = chunkViewModel.RightChunk;
            _forwardChunk = chunkViewModel.ForwardChunk;
            _backChunk = chunkViewModel.BackChunk;
            
            _chunkMesh = new Mesh();
            RegenerateMesh();
            _meshFilter.mesh = _chunkMesh;
        }
        
        public static void InitTriangles()
        {
            _triangles = new int[65536 * 6 / 4];

            int vertexNum = 4;
            for (int i = 0; i < _triangles.Length; i += 6)
            {
                _triangles[i] = vertexNum - 4;
                _triangles[i + 1] = vertexNum - 3;
                _triangles[i + 2] = vertexNum - 2;
                _triangles[i + 3] = vertexNum - 3;
                _triangles[i + 4] = vertexNum - 1;
                _triangles[i + 5] = vertexNum - 2;

                vertexNum += 4;
            }
        }
        
        private void RegenerateMesh()
        {
            _vertices.Clear();
            _uvs.Clear();

            int maxY = 0;
            for (int y = 0; y < ChunkData.ChunkHeight; y++)
            {
                for (int x = 0; x < ChunkData.ChunkWidth; x++)
                {
                    for (int z = 0; z < ChunkData.ChunkWidth; z++)
                    {
                        bool generated = GenerateBlock(x, y, z);
                        if (generated && y > maxY)
                        {
                            maxY = y;
                        }
                    }
                }
            }

            _chunkMesh.triangles = Array.Empty<int>();
            _chunkMesh.vertices = _vertices.ToArray();
            _chunkMesh.uv = _uvs.ToArray();
            _chunkMesh.SetTriangles(_triangles, 0, 
                _vertices.Count * 6 / 4, 0, false);

            _chunkMesh.Optimize();
            
            _chunkMesh.RecalculateNormals();

            Vector3 boundsSize = new Vector3(ChunkData.ChunkWidth, maxY, ChunkData.ChunkWidth);
            _chunkMesh.bounds = new Bounds(boundsSize / 2, boundsSize);
            
            _meshCollider.sharedMesh = _chunkMesh;
        }
        
        private bool GenerateBlock(int x, int y, int z)
        {
            var blockPosition = new Vector3Int(x, y, z);

            var blockType = GetBlockAtPosition(blockPosition);
            if (blockType == BlockType.Air)
            {
                return false;
            }

            if (GetBlockAtPosition(blockPosition + Vector3Int.right) == 0)
            {
                GenerateRightSide(blockPosition);
                AddUvs(blockType, Vector3Int.right);
            }
            if (GetBlockAtPosition(blockPosition + Vector3Int.left) == 0)
            {
                GenerateLeftSide(blockPosition);
                AddUvs(blockType, Vector3Int.left);
            }
            if (GetBlockAtPosition(blockPosition + Vector3Int.forward) == 0)
            {
                GenerateFrontSide(blockPosition);
                AddUvs(blockType, Vector3Int.forward);
            }
            if (GetBlockAtPosition(blockPosition + Vector3Int.back) == 0)
            {
                GenerateBackSide(blockPosition);
                AddUvs(blockType, Vector3Int.back);
            }
            if (GetBlockAtPosition(blockPosition + Vector3Int.up) == 0)
            {
                GenerateTopSide(blockPosition);
                AddUvs(blockType, Vector3Int.up);
            }
            if (blockPosition.y > 0 && GetBlockAtPosition(blockPosition + Vector3Int.down) == 0)
            {
                GenerateBottomSide(blockPosition);
                AddUvs(blockType, Vector3Int.down);
            }
            
            return true;
        }

        private BlockType GetBlockAtPosition(Vector3Int blockPosition)
        {
            if (blockPosition.x >= 0 && blockPosition.x < ChunkData.ChunkWidth &&
                blockPosition.y >= 0 && blockPosition.y < ChunkData.ChunkHeight &&
                blockPosition.z >= 0 && blockPosition.z < ChunkData.ChunkWidth)
            {
                return _chunkViewModel.GetBlock(blockPosition.x, blockPosition.y, blockPosition.z);
            }
            else
            {
                if (blockPosition.y < 0 || blockPosition.y >= ChunkData.ChunkHeight)
                {
                    return BlockType.Air;
                }
                
                if (blockPosition.x < 0)
                {
                    if (_leftChunk == null)
                    {
                        return BlockType.Air;
                    }
                    
                    blockPosition.x += ChunkData.ChunkWidth;
                    return _leftChunk.GetBlock(blockPosition.x, blockPosition.y, blockPosition.z);
                }
                else if (blockPosition.x >= ChunkData.ChunkWidth)
                {
                    if (_rightChunk == null)
                    {
                        return BlockType.Air;
                    }
                    
                    blockPosition.x -= ChunkData.ChunkWidth;
                    return _rightChunk.GetBlock(blockPosition.x, blockPosition.y, blockPosition.z);
                }
                
                if (blockPosition.z < 0)
                {
                    if (_backChunk == null)
                    {
                        return BlockType.Air;
                    }
                    
                    blockPosition.z += ChunkData.ChunkWidth;
                    return _backChunk.GetBlock(blockPosition.x, blockPosition.y, blockPosition.z);
                }
                else if (blockPosition.z >= ChunkData.ChunkWidth)
                {
                    if (_forwardChunk == null)
                    {
                        return BlockType.Air;
                    }
                    
                    blockPosition.z -= ChunkData.ChunkWidth;
                    return _forwardChunk.GetBlock(blockPosition.x, blockPosition.y, blockPosition.z);
                }

                return BlockType.Air;
            }
        }

        private void GenerateRightSide(Vector3Int blockPosition)
        {
            _vertices.Add((new Vector3(1, 0, 0) + blockPosition));
            _vertices.Add((new Vector3(1, 1, 0) + blockPosition));
            _vertices.Add((new Vector3(1, 0, 1) + blockPosition));
            _vertices.Add((new Vector3(1, 1, 1) + blockPosition));
        }

        private void GenerateLeftSide(Vector3Int blockPosition)
        {
            _vertices.Add((new Vector3(0, 0, 0) + blockPosition));
            _vertices.Add((new Vector3(0, 0, 1) + blockPosition));
            _vertices.Add((new Vector3(0, 1, 0) + blockPosition));
            _vertices.Add((new Vector3(0, 1, 1) + blockPosition));
        }

        private void GenerateFrontSide(Vector3Int blockPosition)
        {
            _vertices.Add((new Vector3(0, 0, 1) + blockPosition));
            _vertices.Add((new Vector3(1, 0, 1) + blockPosition));
            _vertices.Add((new Vector3(0, 1, 1) + blockPosition));
            _vertices.Add((new Vector3(1, 1, 1) + blockPosition));
        }

        private void GenerateBackSide(Vector3Int blockPosition)
        {
            _vertices.Add((new Vector3(0, 0, 0) + blockPosition));
            _vertices.Add((new Vector3(0, 1, 0) + blockPosition));
            _vertices.Add((new Vector3(1, 0, 0) + blockPosition));
            _vertices.Add((new Vector3(1, 1, 0) + blockPosition));
        }

        private void GenerateTopSide(Vector3Int blockPosition)
        {
            _vertices.Add((new Vector3(0, 1, 0) + blockPosition));
            _vertices.Add((new Vector3(0, 1, 1) + blockPosition));
            _vertices.Add((new Vector3(1, 1, 0) + blockPosition));
            _vertices.Add((new Vector3(1, 1, 1) + blockPosition));
        }

        private void GenerateBottomSide(Vector3Int blockPosition)
        {
            _vertices.Add((new Vector3(0, 0, 0) + blockPosition));
            _vertices.Add((new Vector3(1, 0, 0) + blockPosition));
            _vertices.Add((new Vector3(0, 0, 1) + blockPosition));
            _vertices.Add((new Vector3(1, 0, 1) + blockPosition));
        }

        private void AddUvs(BlockType blockType, Vector3Int normal)
        {
            Vector2 uv;
            BlockSettings blockSettings = _chunkViewModel.GetBlockSettings(blockType);
            if (blockSettings != null)
            {
                Vector2 pixelOffsets = blockSettings.GetPixelOffsets(normal);
                uv = new Vector2(pixelOffsets.x / 1000f, pixelOffsets.y / 500f);
            }
            else
            {
                uv = new Vector2(0f, 0f);
            }
            
            _uvs.Add(uv + new Vector2(0, 0));
            _uvs.Add(uv + new Vector2(0, 16f / 500f));
            _uvs.Add(uv + new Vector2(16f / 1000f, 0));
            _uvs.Add(uv + new Vector2(16f / 1000f, 16f / 500f));
        }
    }
}