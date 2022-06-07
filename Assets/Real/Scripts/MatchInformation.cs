using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerProfile
{
    public float productivityMultiplier;
    public float powerMultiplier;
    public float speedMultiplier;

    public PlayerProfile(float productivity, float power, float speed)
    {
        productivityMultiplier = productivity;
        powerMultiplier = power;
        speedMultiplier = speed;
    }
}
public class MatchInformation : MonoBehaviour
{
    public Color[] playerColors;
    public List<PlayerProfile> playerProfiles = new List<PlayerProfile>();
    public int currentLevel;


    void AddWorldPlayerProfile()
    {
        playerProfiles.Add(new PlayerProfile(1f, 1f, 1f));
    }

    void LoadPlayersPlayerProfile()
    {
        int skillsOnStrength = PlayerPrefs.GetInt("Strength", 0);
        int skillsOnReproduction = PlayerPrefs.GetInt("Reproduction", 0);
        int skillsOnSpeed = PlayerPrefs.GetInt("Speed", 0);
        float strengthModifier = 1f + 0.05f * skillsOnStrength;
        float reprodModifier = 1f + 0.05f * skillsOnReproduction;
        float speedModifier = 1f + 0.05f * skillsOnSpeed;
        playerProfiles.Add(new PlayerProfile(reprodModifier, strengthModifier, speedModifier));
    }

    void GenerateRandomProfile()
    {
        int difficulty = currentLevel;
        int skillsOnStrength = 0;
        int skillsOnReproduction = 0;
        int skillsOnSpeed = 0;

        for(int i = 0; i < difficulty; i++)
        {
            float r = Random.value;
            if (r < 0.33333f)
                skillsOnStrength++;
            else if (r < 0.666666f)
                skillsOnReproduction++;
            else
                skillsOnSpeed++;
        }

        float strengthModifier = 1f + 0.05f * skillsOnStrength;
        float reprodModifier = 1f + 0.05f * skillsOnReproduction;
        float speedModifier = 1f + 0.05f * skillsOnSpeed;
        playerProfiles.Add(new PlayerProfile(reprodModifier, strengthModifier, speedModifier));

    }

    void LoadSavedMatchProfiles()
    {
        switch(currentLevel)
        {
            case 1:
                playerProfiles.Add(new PlayerProfile(1f, 1f, 1f));
                break;
            case 2:
                playerProfiles.Add(new PlayerProfile(1f, 1f, 1f));
                break;
            case 3:
                playerProfiles.Add(new PlayerProfile(1f, 1f, 1f));
                break;
            case 4:
                playerProfiles.Add(new PlayerProfile(1f, 1f, 1f));
                playerProfiles.Add(new PlayerProfile(1f, 1f, 1f));
                playerProfiles.Add(new PlayerProfile(1f, 1f, 1f));
                break;
            case 5:
                playerProfiles.Add(new PlayerProfile(1.1f, 1.1f, 1f));
                playerProfiles.Add(new PlayerProfile(1.05f, 1.05f, 1.05f));
                break;
            case 6:
                playerProfiles.Add(new PlayerProfile(1.1f, 1.1f, 1.05f));
                playerProfiles.Add(new PlayerProfile(1.2f, 1f, 1f));
                break;
            case 7:
                playerProfiles.Add(new PlayerProfile(1.1f, 1.2f, 1.05f));
                playerProfiles.Add(new PlayerProfile(1.2f, 1.05f, 1.1f));
                break;
            case 8:
                playerProfiles.Add(new PlayerProfile(1.15f, 1.15f, 1.15f));
                break;
            case 9:
                playerProfiles.Add(new PlayerProfile(1.15f, 1.15f, 1.15f));
                playerProfiles.Add(new PlayerProfile(1.25f, 1.25f, 1.0f));
                playerProfiles.Add(new PlayerProfile(1.3f, 1.05f, 1.05f));
                break;
            case 10:
                playerProfiles.Add(new PlayerProfile(1.1f, 1.4f, 1.0f));
                break;
            case 11:
                playerProfiles.Add(new PlayerProfile(1.25f, 1.25f, 1.1f));
                break;
            case 12:
                playerProfiles.Add(new PlayerProfile(1.1f, 1.4f, 1.05f));
                playerProfiles.Add(new PlayerProfile(1.5f, 1.05f, 1.0f));
                playerProfiles.Add(new PlayerProfile(1.4f, 1.4f, 1.4f));
                break;
            case 13:
                playerProfiles.Add(new PlayerProfile(1.4f, 1.4f, 1.0f));
                break;
            case 14:
                playerProfiles.Add(new PlayerProfile(1.25f, 0.5f, 1f));
                break;
            case 15:
                playerProfiles.Add(new PlayerProfile(1.4f, 1.1f, 1.25f));
                break;
            case 16:
                playerProfiles.Add(new PlayerProfile(1.4f, 1.4f, 1.1f));
                playerProfiles.Add(new PlayerProfile(1.3f, 1.3f, 1.3f));
                break;
            case 17:
                playerProfiles.Add(new PlayerProfile(1.15f, 1.75f, 1.0f));
                break;
            case 18:
                playerProfiles.Add(new PlayerProfile(1.35f, 1.35f, 1.35f));
                break;
            case 19:
                playerProfiles.Add(new PlayerProfile(1.25f, 1.8f, 1.25f));
                playerProfiles.Add(new PlayerProfile(1.65f, 1.65f, 1.1f));
                playerProfiles.Add(new PlayerProfile(1.8f, 1.25f, 1.25f));
                playerProfiles.Add(new PlayerProfile(1.5f, 1.5f, 1.5f));
                break;
            case 20:
                playerProfiles.Add(new PlayerProfile(1.75f, 1.75f, 1.75f));
                break;
        }
        for (int i = 0; i < 7; i++)
            playerProfiles.Add(new PlayerProfile(1f, 1f, 1f));
    }

    void LoadOtherPlayerProfiles()
    {
        if (currentLevel <= 20)
        {
            LoadSavedMatchProfiles();
        }
        else
            for (int i = 0; i < 7; i++)
                GenerateRandomProfile();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentLevel = FindObjectOfType<GameManager>().currentLevel;
        AddWorldPlayerProfile();
        LoadPlayersPlayerProfile();
        LoadOtherPlayerProfiles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
