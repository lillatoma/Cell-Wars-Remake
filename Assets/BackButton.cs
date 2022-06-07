using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{

    public void GoBack()
    {
        System.GC.Collect();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            GoBack();
    }
}
