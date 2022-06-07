using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillsMenu : MonoBehaviour
{
    public CanvasGroup[] plusButtons;
    public TMP_Text remainingPointsText;
    public TMP_Text skillsSpentText;
    SkillsHolder skillsHolder;


    // Start is called before the first frame update
    void Start()
    {
        skillsHolder = FindObjectOfType<SkillsHolder>();
    }

    

    public void IncreaseStrength()
    {
        if (skillsHolder.GetRemainingSkillPoints() > 0)
        {
            skillsHolder.skillsOnStrength++;
            skillsHolder.spentSkillPoints++;
        }
    }

    public void IncreaseReproduction()
    {
        if (skillsHolder.GetRemainingSkillPoints() > 0)
        {
            skillsHolder.skillsOnReproduction++;
            skillsHolder.spentSkillPoints++;
        }
    }

    public void IncreaseSpeed()
    {
        if (skillsHolder.GetRemainingSkillPoints() > 0)
        {
            skillsHolder.skillsOnSpeed++;
            skillsHolder.spentSkillPoints++;
        }
    }

    public void SaveSkillPoints()
    {
        skillsHolder.SaveSkillPoints();
    }


    // Update is called once per frame
    void Update()
    {
        if (skillsHolder.GetRemainingSkillPoints() == 0)
            for (int i = 0; i < plusButtons.Length; i++)
                plusButtons[i].alpha = 0.5f;
        else
            for (int i = 0; i < plusButtons.Length; i++)
                plusButtons[i].alpha = 1f;
        remainingPointsText.text = skillsHolder.GetRemainingSkillPoints().ToString();
        skillsSpentText.text = "Power: " + skillsHolder.skillsOnStrength + "\nReproduction: " + skillsHolder.skillsOnReproduction + "\nSpeed: " + skillsHolder.skillsOnSpeed;
    }
}
