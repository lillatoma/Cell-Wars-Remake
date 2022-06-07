using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class RandomLevelText : MonoBehaviour
{
    public TMP_Text text;
    private GameManager manager;
    // Start is called before the first frame update
    void Awake()
    {
        text = GetComponent<TMP_Text>();
        manager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        text.text = "Level " + manager.currentLevel;
    }


}
