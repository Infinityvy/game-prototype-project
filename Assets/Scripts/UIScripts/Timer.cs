using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text text;

    private int minutes = 0;
    private int seconds = 0;

    void Update()
    {
        minutes = (int)Time.timeSinceLevelLoad / 60;
        int newSeconds = (int)Time.timeSinceLevelLoad % 60;

        if(newSeconds != seconds)
        {
            text.text = (minutes.ToString().Length < 2 ? "0" : "") + minutes.ToString() + ":" + (newSeconds.ToString().Length < 2 ? "0" : "") + newSeconds.ToString();
        }

        seconds = newSeconds;
    }
}
