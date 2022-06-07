using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerBar : MonoBehaviour
{
    public Image[] playerPowers;
    public TMP_Text percentText;
    private Image image;
    private Cell[] allCells;
    private VirusDeployer virusDeployer;

    private Statistics statistics;
    // Start is called before the first frame update
    void Start()
    {
        virusDeployer = FindObjectOfType<VirusDeployer>();
        allCells = FindObjectsOfType<Cell>();
        image = GetComponent<Image>();
        MatchInformation matchInformation = FindObjectOfType<MatchInformation>();
        for (int i = 0; i < playerPowers.Length; i++)
            playerPowers[i].color = matchInformation.playerColors[i + 1];
        statistics = FindObjectOfType<Statistics>();
    }

    void DoPowers()
    {
        float totalPower = statistics.GetTotalPower();
        if (totalPower < 1)
            totalPower = 1;
        float[] powers = statistics.GetPowers();

        float countedPower = 0;
        for(int i = 0; i < powers.Length; i++)
        {
            playerPowers[i].rectTransform.localPosition = new Vector3(1280f * countedPower, 0, 0);
            playerPowers[i].rectTransform.sizeDelta = new Vector2(1280f * powers[i] / totalPower, 5);
            countedPower += powers[i] / totalPower;
        }

        percentText.text = string.Format("{0:#,###.#}%", 100f * powers[0] / totalPower);
        percentText.color = playerPowers[0].color;
    }

    // Update is called once per frame
    void Update()
    {
        DoPowers();
    }
}
