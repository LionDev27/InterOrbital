using InterOrbital.Utils;
using UnityEngine;

namespace InterOrbital.WorldSystem
{
    public class Chunk : MonoBehaviour
    {
        private bool _revealed;
        private Vector2Int _chunkMapPos;

        private void Start()
        {
            _revealed = false;
        }

        public void SetChunkPos(int x, int y)
        {
            _chunkMapPos = new Vector2Int(x, y);
        }

        public bool IsRevealed() 
        { 
            return _revealed; 
        }

        public void SetRevealed(bool b)
        {
            _revealed = b;
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("PlayerMinimap") && !_revealed)
            {
                GridLogic.Instance.GenerateChunkMinimap(_chunkMapPos.x, _chunkMapPos.y);
            }
        }
    }
}
