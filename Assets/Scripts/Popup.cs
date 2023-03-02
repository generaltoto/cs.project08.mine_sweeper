using System;
using System.Linq;
using Grid;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class Popup : MonoBehaviour
    {
        public GameObject popup;

        
        /*public void OpenModal()
        {
            popup.SetActive(popup.activeSelf);
        }*/
        public void CloseModal()
        {
            popup.SetActive (!popup.activeSelf);
            if (!popup.activeSelf) 
            {
                Time.timeScale = 1f;
            }
        }
    }
}