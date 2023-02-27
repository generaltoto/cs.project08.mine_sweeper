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
            SceneManager.LoadScene(1);
        } 
        public void EndGame()
        {
            Application.Quit();
        }
        public void ChooseDifficulty()
        {

            int value = GetComponent<Dropdown>().value;

            switch (value)
            {
                case 1:
                    SceneManager.LoadScene(2);
                    break;

                case 2:
                    SceneManager.LoadScene(1);
                    break;

                case 3:
                    SceneManager.LoadScene(2);
                    break;
            }
        }
    }
}

