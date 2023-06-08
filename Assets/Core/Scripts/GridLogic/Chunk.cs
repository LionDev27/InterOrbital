using InterOrbital.Utils;
using UnityEngine;

namespace InterOrbital.WorldSystem
{

    public class Chunk : MonoBehaviour
    {
        private bool _revealed;
        private bool _border;
        private Orientation _borderPos;
        private Vector2Int _chunkMapPos;

        private void Start()
        {
            SetChunkPos((int)transform.position.x, (int)transform.position.y);
            _revealed = false;
        }

        public void SetChunkPos(int x, int y)
        {
            _chunkMapPos = new Vector2Int(x, y);
        }

        public void SetBorder(bool b)
        {
            _border = b;
        }
        public void SetBorderPos(Orientation orientation)
        {
            _borderPos = orientation;
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
                _revealed = true;
                if (_border)
                {
                    var width = GridLogic.Instance.width;
                    var height = GridLogic.Instance.height;
                    switch (_borderPos) 
                    {
                        case Orientation.SW:
                            GridLogic.Instance.GenerateChunkMinimap(_chunkMapPos.x + width, _chunkMapPos.y + height);
                            break;
                        case Orientation.S:
                            GridLogic.Instance.GenerateChunkMinimap(_chunkMapPos.x, _chunkMapPos.y + height);
                            break;
                        case Orientation.SE:
                            GridLogic.Instance.GenerateChunkMinimap(_chunkMapPos.x - width, _chunkMapPos.y + height);
                            break;
                        case Orientation.W:
                            GridLogic.Instance.GenerateChunkMinimap(_chunkMapPos.x + width, _chunkMapPos.y);
                            break;
                        case Orientation.E:
                            GridLogic.Instance.GenerateChunkMinimap(_chunkMapPos.x - width, _chunkMapPos.y);
                            break;
                        case Orientation.NW:
                            GridLogic.Instance.GenerateChunkMinimap(_chunkMapPos.x + width, _chunkMapPos.y - height);
                            break;
                        case Orientation.N:
                            GridLogic.Instance.GenerateChunkMinimap(_chunkMapPos.x, _chunkMapPos.y - height);
                            break;
                        case Orientation.NE:
                            GridLogic.Instance.GenerateChunkMinimap(_chunkMapPos.x - width, _chunkMapPos.y - height);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
