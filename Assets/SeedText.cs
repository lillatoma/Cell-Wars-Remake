using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SeedText : MonoBehaviour
{
    public TMP_Text text;
    private RandomGameGenerator rgg;

    private void Awake()
    {
        rgg = FindObjectOfType<RandomGameGenerator>();
    }
    // Start is called before the first frame update
    void Update()
    {
        text = GetComponent<TMP_Text>();
        text.text = rgg.currentSeed.ToString();
    }
}
