using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticsDrawer : MonoBehaviour
{
    public bool shouldRenderShit = false;
    public RenderTexture texture;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnPostRender()
    {
        if (shouldRenderShit)
            Graphics.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);
    }
}
