using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    public float volume = 0.25f;
    public bool showFPS = false;
    public int fpsLock = 1; // 0- 30, 1 -60, 2 - 120, 3 - unlimited
    public List<float> countedFrames;
    public float currentTime = 0f;

    public void CheckFrames(float maxDiff)
    {
        for (int i = countedFrames.Count - 1; i >= 0; i--)
            if (countedFrames[i] - currentTime < -maxDiff)
                countedFrames.RemoveAt(i);
    }

    public void AddFrame()
    {
        countedFrames.Add(currentTime);
    }

    void BeginKeys()
    {
        Saver.CreateIntKey("ShowFPS", 0);
        Saver.CreateIntKey("FPSLock", 1);
        Saver.CreateIntKey("TotalPoints", 0);
        Saver.CreateIntKey("CompletedLevels", 0);
        Saver.CreateIntKey("SpentPoints", 0);
        Saver.CreateIntKey("Speed", 0);
        Saver.CreateIntKey("Power", 0);
        Saver.CreateIntKey("Reproduction", 0);
        Saver.CreateIntKey("RandomSeed", Random.Range(int.MinValue, int.MaxValue));
        Saver.CreateFloatKey("Volume", 0.25f);
    }

    // Start is called before the first frame update
    void Start()
    {
        BeginKeys();
        DontDestroyOnLoad(this);

        if (FindObjectsOfType<GlobalSettings>().Length > 1)
            Destroy(gameObject);
        else
        {
            volume = PlayerPrefs.GetFloat("Volume", 0.25f);
            showFPS = false;
            if (PlayerPrefs.GetInt("ShowFPS", 0) != 0)
                showFPS = true;
            fpsLock = PlayerPrefs.GetInt("FPSLock", 1);
            if (fpsLock < 0)
                fpsLock = 0;
            if (fpsLock > 3)
                fpsLock = 3;
            int[] FPSValues = new int[] { 30, 60, 120, 5000 };
            Application.targetFrameRate = FPSValues[fpsLock];
        }
        if(Screen.currentResolution.width < 1600 || Screen.currentResolution.height < 900)
        {
            Screen.SetResolution(1280, 720, false);
        }

    }

    public void ResetSettings()
    {
        volume = 0.25f;
        showFPS = false;
        fpsLock = 1;
    }

    public void ChangeFramerate()
    {
        int[] FPSValues = new int[] { 30, 60, 120, 5000 };
        Application.targetFrameRate = FPSValues[fpsLock];
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.SetInt("FPSLock", fpsLock);
        if(showFPS)
            PlayerPrefs.SetInt("ShowFPS", 1);
        else
            PlayerPrefs.SetInt("ShowFPS", 0);
        PlayerPrefs.Save();

    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.unscaledDeltaTime;

    }
}
