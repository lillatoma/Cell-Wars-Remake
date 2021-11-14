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

public class VirusCS : MonoBehaviour
{
    public ComputeShader computeShader;
    public GameObject prefab;
    public RenderTexture renderTexture;
    public Cell target;
    public Material virusMaterial;
    public Mesh virusMesh;
    private List<Virus> viruses = new List<Virus>();

    //private List<GameObject> virusObjects = new List<GameObject>(); 
    
    // Start is called before the first frame update
    void Start()
    {

        renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        renderTexture.enableRandomWrite = true;
        renderTexture.Create();

        computeShader.SetTexture(0, "Result", renderTexture);
        for (int i = 0; i < 2; i++)
            CreateFakeVirus();
        
        for (int i = 0; i < 100 * 100; i++)
            CreateVirus();
    }

    void OnMoveGPU()
    {
        if (viruses.Count <= 0)
            return;
        int size = sizeof(float) * 16 + sizeof(int);
        ComputeBuffer computeBuffer = new ComputeBuffer(viruses.Count,size);

        Virus[] inputArray = viruses.ToArray();
        computeBuffer.SetData(inputArray);

        computeShader.SetBuffer(0, "viruses", computeBuffer);
        computeShader.SetFloat("deltaTime", Time.deltaTime);
        computeShader.Dispatch(0, 1+viruses.Count/2, 1, 1);

        computeBuffer.GetData(inputArray);
        for (int i = viruses.Count - 1; i >= 0 ; i--)
        {
            Virus virus = inputArray[i];
            if (virus.hit != 0)
            {
                target.Damage(virus.power);
                viruses.RemoveAt(i);
                continue;
            }
            viruses[i] = virus;
            //obj.GetComponent<MeshRenderer>().material.SetColor("_Color", virus.color);

        }

        computeBuffer.Release();
    }

    void CreateVirus()
    {
        Virus virusData = new Virus();
        virusData.position = Random.onUnitSphere * Random.Range(0.1f,1) + new Vector3(0,1,0);
        virusData.direction = (target.transform.position - virusData.position).normalized;
        virusData.power = 1f;
        virusData.speed = Random.Range(0.75f, 1);
        virusData.color = Random.ColorHSV();
        virusData.hit = 0;
        virusData.targetPos = target.transform.position;
        virusData.targetSize = target.GetComponent<SphereCollider>().bounds.size.x / 2;
        viruses.Add(virusData);
        //Vector3 position = virusData.position
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
        virusData.targetPos = target.transform.position;
        virusData.targetSize = target.GetComponent<SphereCollider>().bounds.size.x / 2;
        viruses.Add(virusData);

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

    Matrix4x4[] GetMatrixes()
    {
        Matrix4x4[] ret = new Matrix4x4[viruses.Count];
        Quaternion rotation = Quaternion.identity;
        Vector3 scale = new Vector3(0.1f, 0.1f, 0.1f);
        for (int i = 0; i < viruses.Count; i++)
        {
            Vector3 position = viruses[i].position;
            ret[i] = Matrix4x4.TRS(position, rotation, scale);
        }
        return ret;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("BufferSize: " + viruses.Count);
        OnMoveGPU();
        int size = sizeof(float)*16;
        var matrixes = GetMatrixes();
        //var argsBuffer = new ComputeBuffer(1, viruses.Count * size, ComputeBufferType.IndirectArguments);
        //argsBuffer.SetData(viruses.ToArray());
        for (int i = 0; i < 1 + matrixes.Length / 1000; i++)
        {
            if (i == matrixes.Length / 1000)
                Graphics.DrawMeshInstanced(virusMesh, 0, virusMaterial, matrixes, matrixes.Length % 1000);
            else Graphics.DrawMeshInstanced(virusMesh, 0, virusMaterial, matrixes, 1000);
        }
        //Graphics.DrawMeshInstancedIndirect(virusMesh, 0, virusMaterial, new Bounds(Vector3.zero, new Vector3(100.0f, 100.0f, 100.0f)), argsBuffer);
    }
}
