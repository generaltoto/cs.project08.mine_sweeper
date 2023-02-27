using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


namespace DefaultNamespace
{
    public class MainMenu : MonoBehaviour
    {
        public void LoadGame() => GameManager.Instance.StartGame(_width, _height);
        public void EndGame() => Application.Quit();

        public void UpdateValueFromDropdown()
        {
            int value = GetComponent<Dropdown>().value;
            switch (value)
            {
                case 0:
                    _width = DEFAULT_WIDTH;
                    _height = DEFAULT_HEIGHT;
                    break;
                case 1:
                    _width = (int)(DEFAULT_WIDTH * 1.5);
                    _height = (int)(DEFAULT_HEIGHT * 1.5) + 5;
                    break;
                case 2:
                    _width = (int)(DEFAULT_WIDTH * 2.5);
                    _height = (int)(DEFAULT_HEIGHT * 2.5) + 8;
                    break;
            }
        }

        private void Awake()
        {
            _width = DEFAULT_WIDTH;
            _height = DEFAULT_HEIGHT;
        }

        private int _width;
        private int _height;
        
        private const int DEFAULT_WIDTH = 9;
        private const int DEFAULT_HEIGHT = 9;
    }
}

