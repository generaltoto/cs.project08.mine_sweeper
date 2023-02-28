using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class timer : MonoBehaviour
{
    private float timer_game ;
    [SerializeField] private TMP_Text timerTxt;

    private void Update()
    {
        // ReSharper disable HeapView.BoxingAllocation
        timerTxt.text = string.Format("{0:00} : {1:00}", Mathf.Floor(timer_game / 60), timer_game % 60);
        timer_game = Time.time;
    }
}