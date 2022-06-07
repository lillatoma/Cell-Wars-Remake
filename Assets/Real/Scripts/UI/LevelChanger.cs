using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    static public void SaveProgress(int i)
    {
        int currentProgress = PlayerPrefs.GetInt("CompletedLevels", 0);
        if (currentProgress < i)
            PlayerPrefs.SetInt("CompletedLevels", i);
        int totalSkillPoints = PlayerPrefs.GetInt("TotalPoints", 0);
        if (totalSkillPoints < i)
            PlayerPrefs.SetInt("TotalPoints", i);
        PlayerPrefs.Save();
    }
    static public void ChangeLevel(string levelName)
    {
        System.GC.Collect();
        Time.timeScale = 1f;
        SceneManager.LoadScene(levelName);
    }

    static public void ReloadCurrentLevel()
    {
        ChangeLevel(SceneManager.GetActiveScene().name);
    }

    static public void ChangeToHighestLevel()
    {
        string[] names = new string[] { "Level 1", "Level 2", "Level 3", "Level 4", "Level 5", "Level 6", "Level 7", "Level 8", "Level 9", "Level 10",
        "Level 11","Level 12","Level 13","Level 14","Level 15","Level 16","Level 17","Level 18","Level 19","Level 20","Freegame"};
        int currentProgress = PlayerPrefs.GetInt("CompletedLevels", 0);
        if (currentProgress > 20)
            currentProgress = 20;
        ChangeLevel(names[currentProgress]);
    }



    static public void ChangeToLevel(int level)
    {
        string[] names = new string[] { "Level 1", "Level 2", "Level 3", "Level 4", "Level 5", "Level 6", "Level 7", "Level 8", "Level 9", "Level 10",
        "Level 11","Level 12","Level 13","Level 14","Level 15","Level 16","Level 17","Level 18","Level 19","Level 20","Freegame"};
        if (level > 20)
            level = 20;
        ChangeLevel(names[level]);
    }
    static public void CloseApplication()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
