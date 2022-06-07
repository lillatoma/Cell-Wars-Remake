using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndCredits : MonoBehaviour
{

    public CanvasGroup congratulations;
    public CanvasGroup completed;
    public CanvasGroup credits;
    public CanvasGroup myName;
    public CanvasGroup socials;

    public CanvasGroup backButton;
    public CanvasGroup freeplayButton;

    private float timeSinceStartup = 0;
    private CanvasGroup thisCanvasGroup;

    bool alreadyFinished = false;
    // Start is called before the first frame update
    void Start()
    {
        backButton.gameObject.SetActive(false);
        freeplayButton.gameObject.SetActive(false);
        congratulations.alpha = 0f;
        completed.alpha = 0f;
        credits.alpha = 0f;
        myName.alpha = 0f;
        socials.alpha = 0f;
        thisCanvasGroup = GetComponent<CanvasGroup>();
        alreadyFinished = PlayerPrefs.GetInt("GameFinished", 0) != 0;
    }

    void EndOther()
    {
        timeSinceStartup += Time.deltaTime;

        if (timeSinceStartup > 1f)
        {
            float alpha = 2f * (timeSinceStartup - 1f);
            if (alpha > 1f)
                alpha = 1f;
            congratulations.alpha = alpha;
            completed.alpha = alpha;
            credits.alpha = alpha;
            myName.alpha = alpha;
            socials.alpha = alpha;
            backButton.gameObject.SetActive(true);
            freeplayButton.gameObject.SetActive(true);
            alpha = 2f * (timeSinceStartup - 2f);
            if (alpha > 1f)
                alpha = 1f;
            backButton.alpha = alpha;
            freeplayButton.alpha = alpha;
            PlayerPrefs.SetInt("GameFinished", 1);
            PlayerPrefs.Save();
        }
    }

    void FirstTimeEnd()
    {
        timeSinceStartup += Time.deltaTime;
        if (timeSinceStartup < 1f)
            thisCanvasGroup.alpha = timeSinceStartup;
        else
            thisCanvasGroup.alpha = 1;
        if (timeSinceStartup > 1f)
        {
            float alpha = 2f * (timeSinceStartup - 1f);
            if (alpha > 1f)
                alpha = 1f;

            congratulations.alpha = alpha;
        }
        if (timeSinceStartup > 2f)
        {
            float alpha = 2f * (timeSinceStartup - 2f);
            if (alpha > 1f)
                alpha = 1f;

            completed.alpha = alpha;
        }
        if (timeSinceStartup > 4f)
        {
            float alpha = 2f * (timeSinceStartup - 4f);
            if (alpha > 1f)
                alpha = 1f;

            credits.alpha = alpha;
        }
        if (timeSinceStartup > 6f)
        {
            float alpha = 2f * (timeSinceStartup - 6f);
            if (alpha > 1f)
                alpha = 1f;

            myName.alpha = alpha;
        }
        if (timeSinceStartup > 7f)
        {
            float alpha = 2f * (timeSinceStartup - 7f);
            if (alpha > 1f)
                alpha = 1f;

            socials.alpha = alpha;
        }
        if (timeSinceStartup > 10f)
        {
            float alpha = 2f * (timeSinceStartup - 10f);
            if (alpha > 1f)
                alpha = 1f;
            backButton.gameObject.SetActive(true);
            freeplayButton.gameObject.SetActive(true);
            backButton.alpha = alpha;
            freeplayButton.alpha = alpha;
            PlayerPrefs.SetInt("GameFinished", 1);
            PlayerPrefs.Save();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (alreadyFinished)
            EndOther();
        else FirstTimeEnd();
    }
}
