using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int currentLevel;
    public Image panel;
    public Image statisticsPanel;
    public CanvasGroup mainMenuButton;
    public CanvasGroup nextButton;
    public CanvasGroup againButton;
    private Statistics statistics;
    private Cell[] cells;
    private bool gameEnded = false;
    private bool gameWon = false;
    private float timeSinceGameEnded = 0f;
    public float timeSinceMatchStart = 0f;
    public void Reset()
    {
        gameEnded = false;
        gameWon = false;
        timeSinceGameEnded = 0f;
        timeSinceMatchStart = 0f;
        Start();
    }

    // Start is called before the first frame update
    void Start()
    {
        cells = FindObjectsOfType<Cell>();
        statistics = FindObjectOfType<Statistics>();

        if (mainMenuButton != null)
            mainMenuButton.gameObject.SetActive(false);
        if (nextButton != null)
            nextButton.gameObject.SetActive(false);
        if (againButton != null)
            againButton.gameObject.SetActive(false);
    }

    void CheckGameEnd()
    {
        if (gameEnded)
            return;
        int alivePlayers = statistics.GetAlivePlayers();
        if(statistics.GetPlayerPower(1) == 0)
        {
            gameWon = false;
            Time.timeScale = 0;
            gameEnded = true;
            foreach (Cell cell in cells)
                Destroy(cell);
        }
        else if (alivePlayers == 1)
        {
            gameWon = true;
            Time.timeScale = 0;
            gameEnded = true;
            foreach (Cell cell in cells)
                Destroy(cell);
            RandomGameGenerator rgg = FindObjectOfType<RandomGameGenerator>();
            if (rgg)
                rgg.GenerateNewSeed();
        }
    }

    void BringPanelDown()
    {
        if(timeSinceGameEnded == 0f)
        {
            panel.rectTransform.localPosition = new Vector3(0, 720f, 0);
            statisticsPanel.color = new Color(0, 0, 0, 0);
            nextButton.gameObject.SetActive(false);
            nextButton.alpha = 0;
            mainMenuButton.gameObject.SetActive(false);
            mainMenuButton.alpha = 0;
            againButton.gameObject.SetActive(false);
            againButton.alpha = 0;
        }
        if (timeSinceGameEnded > 0f)
            statistics.StopCollectingData();
        if (timeSinceGameEnded < 1f)
        {
            panel.rectTransform.localPosition = new Vector3(0, 720f - 720f * timeSinceGameEnded, 0);
            statisticsPanel.color = new Color(0, 0, 0, 0);
            if(!gameWon)
            {
                float value = 1f - timeSinceGameEnded * 0.6f;
                panel.color = new Color(1f, value, value, 100f / 255f);
            }
        }
        else panel.rectTransform.localPosition = Vector3.zero;
        {
            if (timeSinceGameEnded < 1.5f)
                statisticsPanel.color = new Color(0, 0, 0, 2f*(timeSinceGameEnded - 1f));
            else
            {
                statisticsPanel.color = Color.black;
                statistics.StartDrawing();
                if(timeSinceGameEnded > 2.5f)
                {
                    if(gameWon)
                    {
                        nextButton.gameObject.SetActive(true);
                        nextButton.alpha = 2f * (timeSinceGameEnded - 2.5f);
                        if (nextButton.alpha > 1f)
                            nextButton.alpha = 1f;
                        mainMenuButton.gameObject.SetActive(true);
                        mainMenuButton.alpha = 2f * (timeSinceGameEnded - 2.5f);
                        if (mainMenuButton.alpha > 1f)
                            mainMenuButton.alpha = 1f;
                        LevelChanger.SaveProgress(currentLevel);
                    }
                    else
                    {
                        againButton.gameObject.SetActive(true);
                        againButton.alpha = 2f * (timeSinceGameEnded - 2.5f);
                        if (againButton.alpha > 1f)
                            againButton.alpha = 1f;
                        mainMenuButton.gameObject.SetActive(true);
                        mainMenuButton.alpha = 2f * (timeSinceGameEnded - 2.5f);
                        if (mainMenuButton.alpha > 1f)
                            mainMenuButton.alpha = 1f;
                    }
                }
            }
        }


        
    }

    // Update is called once per frame
    void Update()
    {
        
        CheckGameEnd();
        if (gameEnded)
            timeSinceGameEnded += Time.unscaledDeltaTime;
        else
            timeSinceMatchStart += Time.deltaTime;
        BringPanelDown();
    }
}
