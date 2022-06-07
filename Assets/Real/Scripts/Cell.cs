using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class Cell : MonoBehaviour
{
    public Transform innerCell;
    public GameObject selectionOutline;
    public float innerCellScalingMod = 0.8f;
[Range(0,8)]
    [Tooltip("0 is world, and others are players")]
    public int ownerID = 0; 
    public float capacity;
    public float currentVirusCount;
    [Header("Modifiers")]
    public float reproductivityMod = 1f;
    public float powerMod = 1f;
    public float protectionMod = 1f;
    public float speedMod = 1f;


    private MeshRenderer innerCellMeshRenderer;
    private MeshRenderer selectionMeshRenderer;
    private Vector3 scaleScaler = new Vector3(100, 50, 100);
    private MatchInformation matchInformation;
    private VirusDeployer virusDeployer;
    private float overproductionKillMinValue = 20f;
    private SphereCollider sphereCollider;
    private Camera mainCamera;

    private readonly Vector2 selectionSizeBounds = new Vector2(5, 25);
    private float selectionSizeChangeSpeed = 20f;
    private float selectionSize = 10f;
    private bool selectionSizeUp = true;


    public void Copy(Cell c)
    {
        ownerID = c.ownerID;
        innerCellScalingMod = c.innerCellScalingMod;
        capacity = c.capacity;
        currentVirusCount = c.currentVirusCount;
        reproductivityMod = c.reproductivityMod;
        powerMod = c.powerMod;
        protectionMod = c.protectionMod;
        speedMod = c.speedMod;

    }

    void SimulateSelectionSize()
    {
        if(selectionSizeUp)
        {
            selectionSize += selectionSizeChangeSpeed * Time.deltaTime;
            if (selectionSize > selectionSizeBounds.y)
                selectionSizeUp = false;
        }
        else
        {
            selectionSize -= selectionSizeChangeSpeed * Time.deltaTime;
            if (selectionSize < selectionSizeBounds.x)
                selectionSizeUp = true;
        }
    }


    public Bounds GetBounds()
    {
        return sphereCollider.bounds;
    }
    // Start is called before the first frame update
    void Start()
    {
    }


    void Awake()
    {
        mainCamera = Camera.main;
        sphereCollider = GetComponent<SphereCollider>();
        virusDeployer = FindObjectOfType<VirusDeployer>();
        matchInformation = FindObjectOfType<MatchInformation>();
        if (Application.isPlaying && innerCell && innerCell.GetComponent<MeshRenderer>())
        {
            innerCellMeshRenderer = innerCell.GetComponent<MeshRenderer>();
            innerCellMeshRenderer.materials[0] = new Material(innerCellMeshRenderer.materials[0]);
            selectionMeshRenderer = selectionOutline.GetComponent<MeshRenderer>();
            selectionMeshRenderer.materials[0] = new Material(selectionMeshRenderer.materials[0]);
        }
    }




    [ContextMenu("Update Color")]
    void ChangeInnerCellColor()
    {
        innerCellMeshRenderer.materials[0].SetColor("Color_13fa19b613d94d848f3bcc05dab3ea31", matchInformation.playerColors[ownerID]);
    }
    [ContextMenu("Reassign Material")]
    public void ReassignMaterial()
    {
        innerCellMeshRenderer = innerCell.GetComponent<MeshRenderer>();
        innerCellMeshRenderer.materials[0] = new Material(innerCellMeshRenderer.materials[0]);
        ChangeInnerCellColor();
        selectionMeshRenderer = selectionOutline.GetComponent<MeshRenderer>();
        selectionMeshRenderer.materials[0] = new Material(selectionMeshRenderer.materials[0]);
    }

    public void Damage(int ownerID, float power)
    {
        if (ownerID != this.ownerID)
        {
            currentVirusCount -= (power / protectionMod / matchInformation.playerProfiles[ownerID].powerMultiplier);
            if (currentVirusCount <= 0f)
            {
                currentVirusCount = 0f;
                this.ownerID = ownerID;
            }
        }
        else
        {
            if(power != 0)
                currentVirusCount += 1;

        }
    }

    public void BecomeSelected()
    {
        selectionOutline.SetActive(true);
    }



    public void BecomeUnselected()
    {
        selectionOutline.SetActive(false);
    }

    public void UpdateFunction()
    {
        transform.localScale = scaleScaler * (0.01f * capacity);
        innerCell.transform.localScale = new Vector3(innerCellScalingMod * currentVirusCount / capacity / transform.parent.lossyScale.x,
            innerCellScalingMod * currentVirusCount / capacity / transform.parent.lossyScale.y,
            innerCellScalingMod * currentVirusCount / capacity / transform.parent.lossyScale.z);
        selectionOutline.transform.localScale = new Vector3((selectionSize + transform.lossyScale.x) / transform.lossyScale.x, (selectionSize + transform.lossyScale.y) / transform.lossyScale.y, (selectionSize + transform.lossyScale.z) / transform.lossyScale.z);

    }

    // Update is called once per frame
    void Update()
    {
        if (!innerCellMeshRenderer)
            return;
        transform.localScale = scaleScaler * (0.01f * capacity);
        innerCell.transform.localScale = new Vector3(innerCellScalingMod * currentVirusCount / capacity / transform.parent.lossyScale.x,
            innerCellScalingMod * currentVirusCount / capacity / transform.parent.lossyScale.y,
            innerCellScalingMod * currentVirusCount / capacity / transform.parent.lossyScale.z);
        selectionOutline.transform.localScale = new Vector3((selectionSize + transform.lossyScale.x) / transform.lossyScale.x, (selectionSize + transform.lossyScale.y) / transform.lossyScale.y, (selectionSize + transform.lossyScale.z) / transform.lossyScale.z);
        if (Application.isPlaying)
        {
            ChangeInnerCellColor();
            Produce();
            KillOverproduction();
            if (currentVirusCount > capacity / innerCellScalingMod)
                KillOverproduction();
            DoMousing();
        }
    }

    public float GetCellSpeed()
    {
        return matchInformation.playerProfiles[ownerID].speedMultiplier * speedMod;
    }
    public float GetCellPower()
    {
        return matchInformation.playerProfiles[ownerID].powerMultiplier * powerMod;
    }
    public float GetCellReproductionSpeed()
    {
        return matchInformation.playerProfiles[ownerID].productivityMultiplier * reproductivityMod;
    }


    void Produce()
    {
        if (ownerID == 0)
            return;
        currentVirusCount += Time.deltaTime * GetCellReproductionSpeed();
    }

    float max(float a, float b)
    {
        return (a > b) ? a : b;
    }

    void KillOverproduction()
    {
        float overProductionRate = currentVirusCount / capacity;
        if(overProductionRate > 1)
        {
            currentVirusCount -= Time.deltaTime * max(overproductionKillMinValue,GetCellReproductionSpeed()) * overProductionRate * 1.25f;
        }
    }

    public void ReleaseVirusToTarget(Cell other)
    {
        int reduction = 0;
        for (int i = 0; i < currentVirusCount / 2; i++)
        {
            virusDeployer.CreateVirus(this, other);
            reduction++;
        }
        currentVirusCount -= reduction;
        
    }

    void DoMousing()
    {
        SimulateSelectionSize();
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Cell cell = hit.transform.GetComponent<Cell>();
            if (cell && cell == this && Input.GetMouseButton(0))
            {
                if(cell.ownerID != 1)
                    selectionMeshRenderer.materials[0].SetColor("Color_f47716d4201441d8a4aad21e9a0bb0e2", new Color(1f, 0.25f, 0));
                else
                    selectionMeshRenderer.materials[0].SetColor("Color_f47716d4201441d8a4aad21e9a0bb0e2", new Color(0.25f, 1f, 0));

            }
            else
                selectionMeshRenderer.materials[0].SetColor("Color_f47716d4201441d8a4aad21e9a0bb0e2", new Color(1f, 1f, 0));

        }
    }

}
