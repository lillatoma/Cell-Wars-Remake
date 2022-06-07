using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public struct StatisticsLine
{
    public Vector2 a;
    public Vector2 b;
    public Color lColor;
}

public class Statistics : MonoBehaviour
{
    public RenderTexture renderTexture;
    public Image image;
    public ComputeShader computeShader;
    public float shaderDotDistance;
    public float timeToDrawCompletely = 2f;
    private bool drawing = false;
    private Cell[] allCells;
    private VirusDeployer virusDeployer;
    private float[] powers = new float[8];
    private List<float>[] stats = new List<float>[8];
    private List<StatisticsLine> lines = new List<StatisticsLine>();
    private MatchInformation matchInformation;
    private Sprite sprite;
    public Texture2D dest;
    private float timeSinceDrawing = 0f;
    private int maxData = 0;
    private bool collectingData = true;

    void BeginStatistics()
    {
        for (int i = 0; i < 8; i++)
            stats[i] = new List<float>();
    }

    void AppendStatistics()
    {
        if (stats == null  || powers == null)
            return;
        Debug.Log("Statistics appended");
        for (int i = 0; i < 8; i++)
        {
            if (stats[i] == null)
                continue;
            stats[i].Add(powers[i]);
        }
    }

    float GetHighestAmountFromStats()
    {
        if (stats == null || stats[0] == null || stats[0].Count == 0)
            return 1f;
        float highestAmount = 0;
        for(int i = 0; i < 8; i++)
        {
            for (int j = 0; j < stats[i].Count; j++)
                if (stats[i][j] > highestAmount)
                    highestAmount = stats[i][j];

        }
        return highestAmount;
    }

    void CalculateStatisticsLines()
    {
        float highestAmount = GetHighestAmountFromStats();
        if (stats[0] == null)
            return;
        maxData = stats[0].Count;

        for (int i = 0; i < 8; i++)
        {
            Debug.Log("This started: " + i + "with elements: " + stats[i].Count);
            float fX = 0;
            float fY = stats[i][0] / highestAmount;
            for (int j = 1; j < stats[i].Count; j++)
            {
                float x = (1f * j) / (1f * stats[i].Count) * 800f;
                float y = stats[i][j] / highestAmount * 450f;

                StatisticsLine line;
                line.a = new Vector2(fX, fY);
                line.b = new Vector2(x, y);
                line.lColor = matchInformation.playerColors[i + 1];
                lines.Add(line);
                fX = x;
                fY = y;
            }
            Debug.Log("This done: " + i);
        }

    }
    void GiveStatisticsToShader()
    {
        renderTexture = new RenderTexture(800,450,24);
        renderTexture.enableRandomWrite = true;
        renderTexture.Create();
        if (stats[0] == null)
            return;
        ComputeBuffer computeBuffer = new ComputeBuffer(stats[0].Count * 8, sizeof(float) * 8);
        computeBuffer.SetData(lines.ToArray());

        computeShader.SetTexture(0, "Result", renderTexture);
        computeShader.SetBuffer(0, "Lines", computeBuffer);
        computeShader.SetInt("maxProgress", maxData);
    }


    public void StartDrawing()
    {
        if (!drawing)
        {
            CalculateStatisticsLines();
            GiveStatisticsToShader();
            dest = new Texture2D(800,450);
        }
        drawing = true;
    }

    public void StopCollectingData()
    {
        collectingData = false;
    }

    public float[] GetPowers()
    {
        return powers;
    }

    public float GetPlayerPower(int id)
    {
        return powers[id - 1];
    }

    public float GetTotalPower()
    {
        float count = 0;
        for (int i = 0; i < 8; i++)
            count += powers[i];
        return count;
    }

    public int GetAlivePlayers()
    {
        int count = 0;
        for (int i = 0; i < 8; i++)
            if (powers[i] > 0)
                count++;
        return count;
    }


    public IEnumerator Restart()
    {
        yield return new WaitForEndOfFrame();
        drawing = false;
        timeSinceDrawing = 0f;
        maxData = 0;
        collectingData = true;
        allCells = FindObjectsOfType<Cell>();
        virusDeployer = FindObjectOfType<VirusDeployer>();
        matchInformation = FindObjectOfType<MatchInformation>();
        lines = new List<StatisticsLine>();
        CalculatePowers();
        BeginStatistics();
    }

    [ContextMenu("Reset")]
    public void RestartNoEnum()
    {
        image.sprite = null;
        image.color = new Color(0,0,0,0);
        drawing = false;
        timeSinceDrawing = 0f;
        maxData = 0;
        collectingData = true;
        stats = new List<float>[8];
        allCells = FindObjectsOfType<Cell>();
        virusDeployer = FindObjectOfType<VirusDeployer>();
        matchInformation = FindObjectOfType<MatchInformation>();
        lines = new List<StatisticsLine>();
        CalculatePowers();
        BeginStatistics();
        Debug.Log("Statistics Started");
    }

    // Start called before the first frame update
    void Start()
    {
        RestartNoEnum();
    }

    void CalculatePowers()
    {
        if (allCells == null)
            return;
        float totalPower = 0;
        for (int i = 0; i < powers.Length; i++)
            powers[i] = 0;
        foreach (Cell cell in allCells)
        {
            if (cell.ownerID != 0)
                powers[cell.ownerID - 1] += cell.currentVirusCount;
        }

        float[] distribution = virusDeployer.GetPowerDistribution(8);
        for (int i = 0; i < 8; i++)
            powers[i] += distribution[i];
        foreach (float power in powers)
            totalPower += power;
    }

    public Texture2D toTexture2D(RenderTexture rTex)
    {
        

        RenderTexture.active = rTex;
        //dest = new Texture2D(rTex.width, rTex.height, TextureFormat.RGBA32, true);
        dest.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        //Graphics.CopyTexture(rTex, dest);
        dest.Apply();

        return dest;
    }

    void DrawShader()
    {
        if (stats[0] == null || lines == null || lines.Count < 100)
            return;
        image.color = new Color(255, 255, 255, 255);
        if (timeSinceDrawing > timeToDrawCompletely * 1.5f)
            return;
        int progress = (int)(timeSinceDrawing / timeToDrawCompletely * maxData);

        computeShader.SetInt("progress", progress);
        computeShader.SetFloat("dotDistance", shaderDotDistance);
        computeShader.Dispatch(0, 1 + lines.Count / 100, 1, 1);

        DestroyImmediate(sprite, true);
        toTexture2D(renderTexture);
        sprite = Sprite.Create(dest, new Rect(0, 0, 800, 450), new Vector2(0, 0));

        image.sprite = sprite;

    }

    // Update is called once per frame
    void Update()
    {
        CalculatePowers();
        if (collectingData)
            AppendStatistics();
        if (drawing)
        {
            timeSinceDrawing += Time.unscaledDeltaTime;
            DrawShader();
        }
    }
}
