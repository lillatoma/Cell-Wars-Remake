using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct Virus
{
    public Vector3 position;
    public Vector3 direction;
    public Color color;
    public float power;
    public float speed;
    public Vector3 targetPos;
    public float targetSize;
    public int hit;
}

public class VirusDeployer : MonoBehaviour
{
    public ComputeShader computeShader;
    public GameObject prefab;
    public RenderTexture renderTexture;

    public Material virusMaterial;
    public Mesh virusMesh;
    private List<Virus> viruses = new List<Virus>();
    private List<Cell> targets = new List<Cell>();
    private List<int> playerOrigins = new List<int>();
    //private List<GameObject> virusObjects = new List<GameObject>(); 
    private ComputeBuffer argsBuffer;
    private MatchInformation matchInformation;

    void SetupArgsBuffer()
    {
        uint[] args = new uint[5] { 0, 0, 0, 0, 0 };
        args[0] = (uint)virusMesh.GetIndexCount(0);
        args[1] = (uint)viruses.Count;
        args[2] = (uint)virusMesh.GetIndexStart(0);
        args[3] = (uint)virusMesh.GetBaseVertex(0);
        argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
        argsBuffer.SetData(args);
    }


    void Start()
    {
        matchInformation = FindObjectOfType<MatchInformation>();
        for (int i = 0; i < 10; i++)
            CreateFakeVirus();
        InvokeRepeating("EraseUnneededVirusData", 5f, 5f);
        SetupArgsBuffer();
    }

    public void Restart()
    {
        viruses = new List<Virus>();
        targets = new List<Cell>();
        playerOrigins = new List<int>();
        matchInformation = FindObjectOfType<MatchInformation>();
        for (int i = 0; i < 10; i++)
            CreateFakeVirus();
    }

    void OnMoveGPU()
    {
        if (viruses.Count <= 10 || Time.timeScale == 0f)
            return;
        int size = sizeof(float) * 16 + sizeof(int);
        ComputeBuffer computeBuffer = new ComputeBuffer(viruses.Count,size);

        Virus[] inputArray = viruses.ToArray();
        computeBuffer.SetData(inputArray);

        computeShader.SetBuffer(0, "viruses", computeBuffer);
        computeShader.SetFloat("deltaTime", Time.deltaTime);
        computeShader.Dispatch(0, 1+viruses.Count/10, 1, 1);

        computeBuffer.GetData(inputArray);
        for (int i = viruses.Count - 1; i >= 0 ; i--)
        {
            Virus virus = inputArray[i];
            if (virus.hit != 0)
            {
                targets[i].Damage(playerOrigins[i],virus.power);
                virus.power = 0;
                //viruses.RemoveAt(i);
            }
            viruses[i] = virus;
            //obj.GetComponent<MeshRenderer>().material.SetColor("_Color", virus.color);

        }
        computeBuffer.Release();
    }

    void EraseUnneededVirusData()
    {
        for(int i = viruses.Count -1; i >= 0; i--)
        {
            if(viruses[i].hit != 0)
            {
                viruses.RemoveAt(i);
                targets.RemoveAt(i);
                playerOrigins.RemoveAt(i);
            }
        }
    }
    
    public void CreateVirus(Cell origin, Cell target)
    {
        for(int i = 0; i < viruses.Count; i++)
        {
            if(viruses[i].hit != 0)
            {
                var virusData = viruses[i];
                virusData.position = Random.insideUnitCircle.normalized * Random.Range(0.1f, 1) * origin.GetBounds().size.x / 2;
                virusData.position = new Vector3(virusData.position.x, 0, virusData.position.y);
                virusData.position += origin.transform.position;
                virusData.direction = (target.transform.position - virusData.position).normalized;
                virusData.power = origin.GetCellPower();
                virusData.speed = origin.GetCellSpeed() * Random.Range(0.8f,1.25f);
                virusData.color = matchInformation.playerColors[origin.ownerID];
                virusData.hit = 0;
                virusData.targetPos = target.transform.position;
                virusData.targetSize = target.GetBounds().size.x / 2;
                targets[i] = target;
                viruses[i] = virusData;
                playerOrigins[i] = origin.ownerID;
                return;
            }
        }
        Virus _virusData = new Virus();
        _virusData.position = Random.insideUnitCircle.normalized * Random.Range(0.1f, 1) * origin.GetBounds().size.x / 2;
        _virusData.position = new Vector3(_virusData.position.x, 0, _virusData.position.y);
        _virusData.position += origin.transform.position;
        _virusData.direction = (target.transform.position - _virusData.position).normalized;
        _virusData.power = origin.GetCellPower();
        _virusData.speed = origin.GetCellSpeed() * Random.Range(0.8f, 1.25f);
        _virusData.color = matchInformation.playerColors[origin.ownerID];
        _virusData.hit = 0;
        _virusData.targetPos = target.transform.position;
        _virusData.targetSize = target.GetBounds().size.x / 2;
        viruses.Add(_virusData);
        targets.Add(target);
        playerOrigins.Add(origin.ownerID);
        //Vector3 position = virusData.position
    }

    public float[] GetPowerDistribution(int elemCount)
    {
        float[] ret = new float[elemCount];

        for(int i = 0; i < viruses.Count; i++)
        {
            if (viruses[i].hit == 0 && playerOrigins[i] != 0)
                ret[playerOrigins[i] - 1] += 1f;
        }
        return ret;
    }

    public List<int> GetPlayerIDs()
    {
        List<int> ret = new List<int>();

        for (int i = 0; i < viruses.Count; i++)
        {
            if (viruses[i].hit == 0 && playerOrigins[i] != 0)
            {
                bool found = false;
                for (int j = 0; j < ret.Count; j++)
                    if (ret[j] == playerOrigins[i])
                        found = true;
                if (!found)
                    ret.Add(playerOrigins[i]);
            }    
        }
        return ret;
    }

    void CreateFakeVirus()
    {
        Virus virusData = new Virus();
        virusData.position = Random.onUnitSphere * Random.Range(500f, 1000f);
        virusData.direction = Vector3.zero;
        virusData.power = 1f;
        virusData.speed = 0f;
        virusData.color = Random.ColorHSV();
        virusData.hit = 0;
        virusData.targetPos = Random.onUnitSphere;
        virusData.targetSize = 0;
        viruses.Add(virusData);
        targets.Add(null);
        playerOrigins.Add(0);
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (renderTexture == null)
        {
            renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
            renderTexture.enableRandomWrite = true;
            renderTexture.Create();
        }

        computeShader.SetTexture(0, "Result", renderTexture);
        computeShader.Dispatch(0, renderTexture.width / 8, renderTexture.height / 8, 1);

        Graphics.Blit(renderTexture, destination);
    }

    Matrix4x4[] GetMatrixes(int start, int total)
    {
        Matrix4x4[] ret = new Matrix4x4[total];
        Quaternion rotation = Quaternion.Euler(new Vector3(45f,45f,45f));
        Vector3 scale = new Vector3(0.05f, 0.05f, 0.05f);
        int iteration = 0;
        for (int i = start; i < viruses.Count && i < (start + total); i++)
        {
            Vector3 position = viruses[i].position;
            if(viruses[i].hit == 0)
                ret[iteration] = Matrix4x4.TRS(position, rotation, scale);
            else
                ret[iteration] = Matrix4x4.TRS(position, rotation, new Vector3(0,0,0));
            iteration++;
        }
        return ret;
    }

    void MouseCheck()
    {

    }

    MaterialPropertyBlock SetMaterialMeshPropertiesBuffer(int start, int total)
    {
        Vector4[] colors = new Vector4[total];

        var block = new MaterialPropertyBlock();
        int iterat = 0;
        for (int i = start; i < viruses.Count && i < (start + total); i++)
        {
            // Build matrix.

            colors[iterat] = viruses[i].color;
            iterat++;
        }

        block.SetVectorArray("_Colors", colors);

        return block;
    }

    // Update is called once per frame
    void Update()
    {
        OnMoveGPU();
        int size = sizeof(float)*16;


        // var block = SetMaterialMeshPropertiesBuffer();
        //var argsBuffer = new ComputeBuffer(1, viruses.Count * size, ComputeBufferType.IndirectArguments);
        //argsBuffer.SetData(viruses.ToArray());
        //for (int i = 0; i < 1 + viruses.Count / 1000; i++)
        //{
        //    if (i == viruses.Count / 1000)
        //        Graphics.DrawMeshInstanced(virusMesh, 0, virusMaterial, GetMatrixes(i * 1000, viruses.Count % 1000), viruses.Count % 1000, SetMaterialMeshPropertiesBuffer(i * 1000, viruses.Count % 1000), UnityEngine.Rendering.ShadowCastingMode.Off);
        //    else Graphics.DrawMeshInstanced(virusMesh, 0, virusMaterial, GetMatrixes(i * 1000, 1000), 1000, SetMaterialMeshPropertiesBuffer(i * 1000,1000), UnityEngine.Rendering.ShadowCastingMode.Off);

        //}

        for (int i = 0; i < viruses.Count;)
        {
            int count = 300;
            if (viruses.Count - i < 300)
                count = viruses.Count - i;
            Graphics.DrawMeshInstanced(virusMesh, 0, virusMaterial, GetMatrixes(i, count), count, SetMaterialMeshPropertiesBuffer(i, count), UnityEngine.Rendering.ShadowCastingMode.Off);
            i += count;
            //if (i > 0)
            //    break;
        }
        //Graphics.DrawMeshInstancedIndirect(virusMesh, 0, virusMaterial, new Bounds(Vector3.zero, new Vector3(100.0f, 100.0f, 100.0f)), argsBuffer);
        MouseCheck();
    }
}
