using UnityEngine;

namespace DefaultNamespace
{
    public class Tile : MonoBehaviour
    {
        public enum TileType
        {
            EMPTY,
            BOMB,
            CLUE,
            MASK
        }
        
        public TileType Type => _type;
        
        public bool IsRevealed => _isRevealed;

        public Vector2Int Position
        {
            get => _position;
            set => _position = value;
        }

        private Sprite _defaultSprite;
        private Sprite _maskSprite;

        private Vector2Int _position;

        private TileType _type;
        private bool _isRevealed;
        
        public void InitWithType(TileType type, string path)
        {
            _type = type;
            _defaultSprite = Resources.Load<Sprite>("Graphics/" + path);
            _maskSprite = Resources.Load<Sprite>("Graphics/Mask");
            
            GetComponent<SpriteRenderer>().sprite = _maskSprite;
        }

        public void Reveal()
        {
            GetComponent<SpriteRenderer>().sprite = _defaultSprite;
            _isRevealed = true;
        }

        private void OnMouseDown() => Game.Instance.OnTileClicked(this);
    }
}