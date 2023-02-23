using System;
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
        }

        public TileType Type => _type;

        public bool IsRevealed => _isRevealed;
        public bool IsFlagged => _isFlagged;

        public Vector2Int Position { get; set; }
        
        public int ClueCount { get; set; }

        private Sprite _defaultSprite;
        private Sprite _maskSprite;

        private Vector2Int _position;

        private TileType _type;
        private bool _isRevealed;
        private bool _isFlagged;

        private const string GRAPHICS_PATH = "Graphics/";
        private const string MASK_PREFAB_NAME = "mask";
        private const string EMPTY_SPRITE_NAME = "mask";
        private const string FLAG_SPRITE_NAME = "flag";
        private const string BOMB_SPRITE_NAME = "bomb";

        public void InitWithType(TileType type)
        {
            _type = type;
            _defaultSprite = Resources.Load<Sprite>(GRAPHICS_PATH + GetCorrectPathNameFromType());
            _maskSprite = Resources.Load<Sprite>(GRAPHICS_PATH + MASK_PREFAB_NAME);

            GetComponent<SpriteRenderer>().sprite = _maskSprite;
        }

        public void Reveal()
        {
            GetComponent<SpriteRenderer>().sprite = _defaultSprite;
            _isRevealed = true;
        }

        public void OnLeftClickCallback() => Game.Instance.OnTileClicked(this);
        
        public void OnRightClickCallback()
        {
            GetComponent<SpriteRenderer>().sprite =
                _isFlagged ? _maskSprite : Resources.Load<Sprite>(GRAPHICS_PATH + FLAG_SPRITE_NAME);
            _isFlagged = !_isFlagged; 
        }

        private string GetCorrectPathNameFromType()
        {
            switch (_type)
            {
                case TileType.EMPTY:
                    return EMPTY_SPRITE_NAME;
                case TileType.BOMB:
                    return BOMB_SPRITE_NAME;
                case TileType.CLUE:
                    return ClueCount.ToString();
                default:
                    throw new ArgumentException("Invalid TileType");
            }
        }
    }
}