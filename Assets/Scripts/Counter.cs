using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Counter : MonoBehaviour
{
    int nb_bomb = 5;
    public GameObject nb_count;
   
    void Update()
    {
        for (int x = 0; x > 0; x--)
        {
            nb_count.GetComponent<TMP_Text>().text = string.Format("0", nb_bomb);
            nb_bomb -= nb_bomb;
        }
    }
}
