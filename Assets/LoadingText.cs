using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingText : MonoBehaviour
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
        if (gameManager.timeSinceMatchStart < 1f)
            text.color = new Color(255, 255, 255, 255);
        else
            text.color = new Color(255, 255, 255, 0);
    }
}
