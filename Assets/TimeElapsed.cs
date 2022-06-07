using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeElapsed : MonoBehaviour
{
    private TMP_Text text;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        float timeElapsed = gameManager.timeSinceMatchStart; 
        int seconds = 0;
        int minutes = 0;

        while(timeElapsed > 60)
        {
            minutes++;
            timeElapsed-=60;
        }
        while(timeElapsed > 1)
        {
            seconds++;
            timeElapsed--;
        }
        int milliseconds = (int)(timeElapsed * 1000);
        string str = "Time Elapsed: ";
        str += minutes.ToString() + ":";
        if (seconds < 10)
            str += "0";
        str += seconds.ToString() + ".";
        str += milliseconds.ToString();
        text.text = str;

    }
}
