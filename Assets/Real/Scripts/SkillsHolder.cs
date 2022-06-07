using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsHolder : MonoBehaviour
{
    public int skillsOnStrength;
    public int skillsOnReproduction;
    public int skillsOnSpeed;
    public int spentSkillPoints = 0;
    public int totalSkillPoints = 0;

    // Start is called before the first frame update
    void Start()
    {
        skillsOnStrength = PlayerPrefs.GetInt("Strength", 0);
        skillsOnReproduction = PlayerPrefs.GetInt("Reproduction", 0);
        skillsOnSpeed = PlayerPrefs.GetInt("Speed", 0);
        spentSkillPoints = PlayerPrefs.GetInt("SpentPoints", 0);
        totalSkillPoints = PlayerPrefs.GetInt("TotalPoints", 0);
        //DontDestroyOnLoad(this);
    }

    public void SaveSkillPoints()
    {
        PlayerPrefs.SetInt("Strength", skillsOnStrength);
        PlayerPrefs.SetInt("Reproduction", skillsOnReproduction);
        PlayerPrefs.SetInt("Speed", skillsOnSpeed);
        PlayerPrefs.SetInt("SpentPoints", spentSkillPoints);
        PlayerPrefs.SetInt("TotalPoints", totalSkillPoints);
        PlayerPrefs.Save();
    }



    public int GetRemainingSkillPoints()
    {
        return totalSkillPoints - spentSkillPoints;
    }

    public void ResetSkillPoints()
    {
        skillsOnStrength = 0;
        skillsOnReproduction = 0;
        skillsOnSpeed = 0;
        spentSkillPoints = 0;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
