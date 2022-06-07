using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioWrapper : MonoBehaviour
{
    public bool loopable = false;
    public bool dontDestroy = false;
    public bool allowDuplicates = true;
    public float length;
    public string[] destroyOnTheseScenes;
    [Range(0, 1)]
    public float volume = 0.5f;
    private AudioSource audioSource;
    private GlobalSettings globalSettings;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        globalSettings = FindObjectOfType<GlobalSettings>();
        if (!allowDuplicates)
        {
            var wrappers = FindObjectsOfType<AudioWrapper>();
            foreach(var wrapper in wrappers)
            {
                if (wrapper == this || wrapper.audioSource == null)
                    continue;
                else if (wrapper.audioSource.clip == audioSource.clip)
                {
                    Destroy(gameObject);
                    return;
                }
            }
        }
        if(dontDestroy)
            DontDestroyOnLoad(this);
        audioSource.volume = volume * globalSettings.volume;
        if (loopable)
            audioSource.loop = true;
        audioSource.Play();
        if(!loopable)
            Destroy(gameObject, length);
    }

    void CheckScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        foreach (string str in destroyOnTheseScenes)
            if (str == currentScene)
                Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        CheckScene();
        audioSource.volume = volume * globalSettings.volume;
    }
}
