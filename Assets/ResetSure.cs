using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetSure : MonoBehaviour
{
    public float timeSinceActive = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnDisable()
    {
        timeSinceActive = 0f;
    }

    private void Grow()
    {
        timeSinceActive += Time.deltaTime;
        if (timeSinceActive > .5f)
            timeSinceActive = .5f;
        transform.localScale = new Vector3(1, 1, 1) * 2f * timeSinceActive;
    }

    // Update is called once per frame
    void Update()
    {
        Grow();
    }
}
