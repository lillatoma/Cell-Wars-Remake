using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class Cell : MonoBehaviour
{
    public Transform innerCell;
    public float innerCellScalingMod = 0.8f;
[Range(0,8)]
    [Tooltip("0 is world, and others are players")]
    public int ownerID = 0; 
    public Color[] ownerColors;
    public int capacity;
    public int currentVirusCount;
    [Header("Modifiers")]
    public float reproductivityMod = 1f;
    public float strengthMod = 1f;
    public float protectionMod = 1f;
    public float speedMod = 1f;

    private MeshRenderer innerCellMeshRenderer;
    private Vector3 scaleScaler = new Vector3(100, 50, 100);
    // Start is called before the first frame update
    void Start()
    {

    }

    void Awake()
    {
        if (Application.isPlaying && innerCell && innerCell.GetComponent<MeshRenderer>())
        {
            innerCellMeshRenderer = innerCell.GetComponent<MeshRenderer>();
            innerCellMeshRenderer.materials[0] = new Material(innerCellMeshRenderer.materials[0]);

        }
    }
    void ChangeInnerCellColor()
    {
        innerCellMeshRenderer.materials[0].SetColor("Color_13fa19b613d94d848f3bcc05dab3ea31", ownerColors[ownerID]);
    }

    // Update is called once per frame
    void Update()
    {
        if (!innerCellMeshRenderer)
            return;
        transform.localScale = scaleScaler * (0.01f*capacity);
        innerCell.transform.localScale = new Vector3(innerCellScalingMod * currentVirusCount / capacity, innerCellScalingMod * currentVirusCount / capacity, innerCellScalingMod * currentVirusCount / capacity);
        if (Application.isPlaying)
        {
            ChangeInnerCellColor();
        }
    }
}
