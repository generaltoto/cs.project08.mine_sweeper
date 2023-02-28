using Grid;
using TMPro;
using UnityEngine;

public class Counter : MonoBehaviour
{
    private int _nbBomb;
    private int _nbFlag;
    [SerializeField] private TMP_Text counterText;

    private void Start()
    {
        _nbBomb = 0;
        counterText.text = _nbBomb.ToString();
    }

    private void Update()
    {
        _nbBomb = GridManager.Instance.BombsCount;
        _nbFlag = GridManager.Instance.FlagsCount;
        
        counterText.text = (_nbBomb - _nbFlag).ToString();
    }
}