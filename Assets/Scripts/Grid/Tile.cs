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

        public TileType Type { get; private set; }

        public bool IsRevealed { get; private set; }

        public bool IsFlagged { get; private set; }

        public Vector2Int Position { get; set; }

        public int ClueCount { get; set; }
        
        public void InitWithType(TileType type)
        {
            Type = type;

            _defaultSprite = Resources.Load<Sprite>(GRAPHICS_PATH + GetCorrectPathNameFromType());

            _maskSprite = Resources.Load<Sprite>(GRAPHICS_PATH + MASK_PREFAB_NAME);
            _flagSprite = Resources.Load<Sprite>(GRAPHICS_PATH + FLAG_SPRITE_NAME);
            _wrongFlagSprite = Resources.Load<Sprite>(GRAPHICS_PATH + WRONG_FLAG_SPRITE_NAME);

            _bombExplodedSprite = (type == TileType.BOMB)
                ? Resources.Load<Sprite>(GRAPHICS_PATH + BOMB_EXPLODED_SPRITE_NAME)
                : null;

            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = _maskSprite;
            
            _animator = GetComponent<Animator>();
            _animator.enabled = false;
        }

        public void Reveal()
        {
            _spriteRenderer.sprite = _defaultSprite;
            IsRevealed = true;
        }
        
        public void Reveal(bool isExploded, bool wasFlagged)
        {
            if (isExploded) _spriteRenderer.sprite = _bombExplodedSprite;
            else if (wasFlagged && Type != TileType.BOMB) _spriteRenderer.sprite = _wrongFlagSprite;
            else _spriteRenderer.sprite = _defaultSprite;
            
            IsRevealed = true;
        }

        public void ToggleFlag()
        {
            _spriteRenderer.sprite = IsFlagged ? _maskSprite : _flagSprite;
            IsFlagged = !IsFlagged;
        }
        
        public void FadeOut() => _animator.enabled = true;

        public void OnLeftClickCallback() => GameManager.Instance.OnLeftClick(this);

        public void OnRightClickCallback() => GameManager.Instance.OnRightClick(this);

        private SpriteRenderer _spriteRenderer;
        private Sprite _defaultSprite;
        private Sprite _maskSprite, _flagSprite, _wrongFlagSprite;
        [CanBeNull] private Sprite _bombExplodedSprite;
        
        private Animator _animator;

        private Vector2Int _position;

        private const string GRAPHICS_PATH = "Graphics/";
        private const string MASK_PREFAB_NAME = "mask";
        private const string EMPTY_SPRITE_NAME = "empty";
        private const string FLAG_SPRITE_NAME = "flag";
        private const string WRONG_FLAG_SPRITE_NAME = "bomb_wrong";
        private const string BOMB_SPRITE_NAME = "bomb";
        private const string BOMB_EXPLODED_SPRITE_NAME = "bomb_exploded";
        
        private string GetCorrectPathNameFromType()
        {
            return Type switch
            {
                TileType.EMPTY => EMPTY_SPRITE_NAME,
                TileType.BOMB => BOMB_SPRITE_NAME,
                TileType.CLUE => ClueCount.ToString(),
                _ => throw new ArgumentException("Invalid TileType")
            };
        }
    }
}