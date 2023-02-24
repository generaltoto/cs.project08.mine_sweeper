using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class timer : MonoBehaviour
{
    float timer_game ;
    public GameObject timerTxt;
  
    void Update()
    {
        //timerTxt.GetComponent<TMP_Text>().SetText(timer_game.ToString());
        timerTxt.GetComponent<TMP_Text>().text = string.Format("{0:00} : {1:00}", Mathf.Floor(timer_game / 60), timer_game % 60);
        timer_game = Time.time;
    }
}