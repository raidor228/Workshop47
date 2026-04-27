using UnityEngine;
using Workshop47.Scripts.Game.State.Chunks;

namespace Workshop47.Scripts.Game.Settlement.View.Chunks
{
    [RequireComponent(typeof(ChunkRenderer))]
    public class ChunkBinder : MonoBehaviour
    {
        [SerializeField] private ChunkRenderer _chunkRenderer;
        
        private ChunkViewModel _chunkViewModel;
        
        public void Bind(ChunkViewModel chunkViewModel)
        {
            _chunkViewModel = chunkViewModel;
            _chunkRenderer.Initialize(chunkViewModel);
            
            var xOffset = chunkViewModel.Position.x * ChunkData.ChunkWidth;
            var zOffset = chunkViewModel.Position.y * ChunkData.ChunkWidth;

            transform.position = new Vector3(xOffset, 0, zOffset);
        }
    }
}