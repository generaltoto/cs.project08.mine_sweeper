using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class MainMenu : MonoBehaviour
    {
        public void LoadGame()
        {
            if (width > 0 && height > 0) GameManager.Instance.StartGame(width, height);
        }
        public void EndGame() => Application.Quit();

        public void UpdateValueFromDropdown()
        {
            switch (GetComponent<Dropdown>().value)
            {
                case 0:
                case 1:
                    width = DEFAULT_WIDTH;
                    height = DEFAULT_HEIGHT;
                    break;
                case 2:
                    width = (int)Math.Floor(DEFAULT_WIDTH * 1.5);
                    height = (int)Math.Floor(DEFAULT_HEIGHT * 1.5) + 5;
                    break;
                case 3:
                    width = (int)Math.Floor(DEFAULT_WIDTH * 2.5);
                    height = (int)Math.Floor(DEFAULT_HEIGHT * 2.5) + 8;
                    break;
            }
        }

        [SerializeField] private int width;
        [SerializeField] private int height;

        private const int DEFAULT_WIDTH = 9;
        private const int DEFAULT_HEIGHT = 9;
    }
}