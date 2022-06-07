using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelectorButton : MonoBehaviour
{
    public bool changeName = true;
    [Range(0, 20)]
    public int levelIndex;
    private TMP_Text text;
    public void OnClickButton()
    {
        LevelChanger.ChangeToLevel(levelIndex);
    }

    public void OnFreegameClickButton()
    {
        LevelChanger.ChangeLevel("Freegame");
    }

    // Start is called before the first frame update
    void Start()
    {
        if (changeName)
        {
            text = transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
            text.text = "Level " + (levelIndex + 1);
        }
        int maxLevel = PlayerPrefs.GetInt("CompletedLevels", 0);

        if (maxLevel < levelIndex)
        {
            GetComponent<Button>().enabled = false;
            GetComponent<CanvasGroup>().alpha = 0.5f;

        }
    }



}
