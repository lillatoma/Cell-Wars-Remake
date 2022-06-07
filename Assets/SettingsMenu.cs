using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Saver
{
    static public void CreateIntKey(string a, int b)
    {
        if (!PlayerPrefs.HasKey(a))
            PlayerPrefs.SetInt(a, b);
    }

    static public void CreateFloatKey(string a, float b)
    {
        if (!PlayerPrefs.HasKey(a))
            PlayerPrefs.SetFloat(a, b);
    }
}


public class SettingsMenu : MonoBehaviour
{
    GlobalSettings settings;
    public Slider volumeSlider;
    public TMP_Dropdown fpsSettings;
    public Toggle fpsMeter;
    public ResetSure resetSure;


    private void Start()
    {
        BruteforceStart();
    }

    void BruteforceStart()
    {
        settings = FindObjectOfType<GlobalSettings>();
        if (settings)
        {
            volumeSlider.value = settings.volume;
            fpsSettings.value = settings.fpsLock;
            fpsMeter.isOn = settings.showFPS;
        }
    }

    public void OpenResetSure()
    {
        resetSure.timeSinceActive = 0f;
        resetSure.transform.localScale = Vector3.zero;
        resetSure.gameObject.SetActive(true);
    }

    public void CloseResetSure()
    {
        resetSure.timeSinceActive = 0f;
        resetSure.gameObject.SetActive(false);
    }

    public void ResetProgress()
    {
        PlayerPrefs.SetInt("CompletedLevels", 0);
        PlayerPrefs.SetInt("TotalPoints", 0);
        PlayerPrefs.SetInt("Strength", 0);
        PlayerPrefs.SetInt("Reproduction", 0);
        PlayerPrefs.SetInt("Speed", 0);
        PlayerPrefs.SetInt("SpentPoints", 0);
        PlayerPrefs.SetInt("TotalPoints", 0);
        PlayerPrefs.SetInt("RandomSeed", Random.Range(int.MinValue, int.MaxValue));
        PlayerPrefs.Save();
    }

    public void ChangeVolume()
    {
        settings.volume = volumeSlider.value;
    }

    public void ToggleFPSMeasure()
    {
        settings.showFPS = fpsMeter.isOn;
    }

    public void ChangeFPSOption()
    {
        settings.fpsLock = fpsSettings.value;
        settings.ChangeFramerate();
    }

    public void SaveSettings()
    {
        settings.SaveSettings();
    }

    public void ResetSettings()
    {
        settings.ResetSettings();
        volumeSlider.value = settings.volume;
        fpsSettings.value = settings.fpsLock;
        fpsMeter.isOn = settings.showFPS;
    }

    public void Update()
    {
        while (settings == null)
            BruteforceStart();
    }


}
