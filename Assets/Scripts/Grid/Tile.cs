using System;
using DefaultNamespace;
using JetBrains.Annotations;
using UnityEngine;

namespace Grid
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

        private SpriteRenderer _spriteRenderer;
        private Sprite _defaultSprite;
        private Sprite _maskSprite, _flagSprite;
        [CanBeNull] private Sprite _bombExplodedSprite;

        private Vector2Int _position;

        private TileType _type;
        private bool _isRevealed;
        private bool _isFlagged;

        private const string GRAPHICS_PATH = "Graphics/";
        private const string MASK_PREFAB_NAME = "mask";
        private const string EMPTY_SPRITE_NAME = "empty";
        private const string FLAG_SPRITE_NAME = "flag";
        private const string BOMB_SPRITE_NAME = "bomb";
        private const string BOMB_EXPLODED_SPRITE_NAME = "bomb_exploded";

        public void InitWithType(TileType type)
        {
            _type = type;

            _defaultSprite = Resources.Load<Sprite>(GRAPHICS_PATH + GetCorrectPathNameFromType());

            _maskSprite = Resources.Load<Sprite>(GRAPHICS_PATH + MASK_PREFAB_NAME);
            _flagSprite = Resources.Load<Sprite>(GRAPHICS_PATH + FLAG_SPRITE_NAME);

            _bombExplodedSprite = type == TileType.BOMB
                ? Resources.Load<Sprite>(GRAPHICS_PATH + BOMB_EXPLODED_SPRITE_NAME)
                : null;

            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = _maskSprite;
        }

        public void Reveal(bool isExploded = false)
        {
            _spriteRenderer.sprite = !isExploded ? _defaultSprite : _bombExplodedSprite;
            _isRevealed = true;
        }

        public void ToggleFlag()
        {
            _spriteRenderer.sprite = _isFlagged ? _maskSprite : _flagSprite;
            _isFlagged = !_isFlagged;
        }

        public void OnLeftClickCallback() => GameManager.Instance.OnLeftClick(this);

        public void OnRightClickCallback() => GameManager.Instance.OnRightClick(this);

        private string GetCorrectPathNameFromType()
        {
            return _type switch
            {
                TileType.EMPTY => EMPTY_SPRITE_NAME,
                TileType.BOMB => BOMB_SPRITE_NAME,
                TileType.CLUE => ClueCount.ToString(),
                _ => throw new ArgumentException("Invalid TileType")
            };
        }
    }
}