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

        private Sprite _defaultSprite;
        private Sprite _maskSprite;

        private Vector2Int _position;

        private TileType _type;
        private bool _isRevealed;
        private bool _isFlagged;

        private const string GRAPHICS_PATH = "Graphics/";
        private const string MASK_PREFAB_NAME = "mask";
        private const string FLAG_PREFAB_NAME = "flag";

        public void InitWithType(TileType type, string path)
        {
            _type = type;
            _defaultSprite = Resources.Load<Sprite>(GRAPHICS_PATH + path);
            _maskSprite = Resources.Load<Sprite>(GRAPHICS_PATH + MASK_PREFAB_NAME);

            GetComponent<SpriteRenderer>().sprite = _maskSprite;
        }

        public void Reveal()
        {
            GetComponent<SpriteRenderer>().sprite = _defaultSprite;
            _isRevealed = true;
        }

        private void OnMouseDown() => Game.Instance.OnTileClicked(this);

        private void OnMouseOver()
        {
            if (Input.GetMouseButton(1))
            {
                GetComponent<SpriteRenderer>().sprite =
                    _isFlagged ? _maskSprite : Resources.Load<Sprite>(GRAPHICS_PATH + FLAG_PREFAB_NAME);
                _isFlagged = !_isFlagged;
            }
        }
    }
}