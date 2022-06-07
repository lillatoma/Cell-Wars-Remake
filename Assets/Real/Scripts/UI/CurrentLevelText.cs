using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class CurrentLevelText : MonoBehaviour
{
    public TMP_Text text;
    
    // Start is called before the first frame update
    void Awake()
    {
        text = GetComponent<TMP_Text>();
        text.text = SceneManager.GetActiveScene().name;
    }


}
