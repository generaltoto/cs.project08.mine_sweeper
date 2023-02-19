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

        private Sprite _defaultSprite;
        private Sprite _maskSprite;

        public void InitWithType(TileType type, string path)
        {
            _type = type;
            _defaultSprite = Resources.Load<Sprite>("Graphics/" + path);
            _maskSprite = Resources.Load<Sprite>("Graphics/Mask");
            
            GetComponent<SpriteRenderer>().sprite = _maskSprite;
        }


        private TileType _type;

        private void OnMouseDown()
        {
           Reveal(); 
        }
        
        private void Reveal() => GetComponent<SpriteRenderer>().sprite = _defaultSprite;
    }
}