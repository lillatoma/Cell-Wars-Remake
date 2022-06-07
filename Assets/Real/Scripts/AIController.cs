using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public int controlledID = 2;
    public float timeToInstantiate;
    public Vector2 timeBetweenSteps;
    public int totalCellSelection;
    [Header("Aggressive behaviour")]
    [Range(0,3)]
    [Tooltip("0 Random, 1 Weakest, 2 WeakestPlayerRandom, 3 WeakestWeakest")]
    public int targetingPriority; //0 - Random, 1 - WeakestCell, 2 - WeakestPlayerRandom, 3 - WeakestWeakest
    [Range(0,2)]
    [Tooltip("0 Random, 1 Strongest, 2 Closest")]
    public int selectingPriority; //0 - Random, 1 - Strongest, 2 - Closest
    public int minimumCellsToAggress;
    public float aggressivePercent;
    [Header("Conquering behaviour")]
    [Range(0, 2)]
    public int conquerTargetingPriority; // 0 - Random, 1 - Weakest, 2 - Closest
    [Range(0, 2)]
    public int conquerSelectingPriority; // 0 - Random, 1 - Strongest, 3 - Closest


    private float timeTillNext;
    private Cell[] allCells;
    private Statistics statistics;
    // Start is called before the first frame update
    void Start()
    {
        timeTillNext = timeToInstantiate;
        allCells = FindObjectsOfType<Cell>();
        statistics = FindObjectOfType<Statistics>();
    }

    int GetOwnedCellCount()
    {
        int count = 0;
        foreach(Cell cell in allCells)
            if (cell.ownerID == controlledID)
                count++;
        return count;
    }

    int GetFreeCellCount()
    {
        int count = 0;
        foreach (Cell cell in allCells)
            if (cell.ownerID == 0)
                count++;
        return count;
    }

    Cell GetRandomEnemyCell()
    {
        int foundCells = 0;
        foreach (Cell cell in allCells)
            if (cell.ownerID != 0 && cell.ownerID != controlledID)
                foundCells++;
        if (foundCells == 0)
            return null;
        while(true)
        {
            int id = Random.Range(0, allCells.Length);
            if (allCells[id].ownerID != 0 && allCells[id].ownerID != controlledID)
                return allCells[id];
        }
    }

    Cell GetRandomFreeCell()
    {
        while (true)
        {
            int id = Random.Range(0, allCells.Length);
            if (allCells[id].ownerID == 0)
                return allCells[id];
        }
    }

    Cell GetWeakestFreeCell()
    {
        List<Cell> cells = GetPlayersCells(0);
        int weakestID = 0;
        for (int i = 1; i < cells.Count; i++)
            if (cells[i].currentVirusCount < cells[weakestID].currentVirusCount)
                weakestID = i;
        return cells[weakestID];
    }

    Cell GetClosestFreeCell(Cell origin)
    {
        List<Cell> cells = GetPlayersCells(0);
        int closestID = 0;
        for (int i = 1; i < cells.Count; i++)
            if ((cells[i].transform.position - origin.transform.position).sqrMagnitude < 
                (cells[closestID].transform.position - origin.transform.position).sqrMagnitude)
                closestID = i;
        return cells[closestID];
    }

    Cell GetWeakestEnemyCell()
    {
        List<Cell> enemyCells = new List<Cell>();
        foreach(Cell cell in allCells)
        {
            if (cell.ownerID != controlledID && cell.ownerID != 0)
                enemyCells.Add(cell);
        }
        if (enemyCells.Count == 0)
            return null;
        int bestIndex = 0;
        for(int i = 1; i < enemyCells.Count; i++)
            if (enemyCells[i].currentVirusCount < enemyCells[bestIndex].currentVirusCount)
                bestIndex = i;
        return enemyCells[bestIndex];
    }

    int GetWeakestPlayer()
    {
        //List<float> powers = new List<float>();
        float[] powers = statistics.GetPowers();
        int smallestID = -1;
        for (int i = 0; i < powers.Length; i++)
        {
            if (powers[i] == 0 ||i + 1 == controlledID)
                continue;
            if (smallestID == -1 || powers[i] < powers[smallestID])
                smallestID = i;
        }
        return smallestID;
    }

    List<Cell> GetPlayersCells(int playerID)
    {
        List<Cell> cells = new List<Cell>();
        foreach (Cell cell in allCells)
            if (cell.ownerID == playerID)
                cells.Add(cell);
        return cells;
    }

    Cell GetRandomCellFromWeakestEnemy()
    {
        int weakestID = GetWeakestPlayer() + 1;
        if (weakestID == 0)
            return null;
        List<Cell> enemyCells = GetPlayersCells(weakestID);
        if (enemyCells.Count == 0)
            return null;
        return enemyCells[Random.Range(0, enemyCells.Count)];
    }

    Cell GetWeakestCellFromWeakestEnemy()
    {
        int weakestID = GetWeakestPlayer() + 1;
        if (weakestID == 0)
            return null;
        List<Cell> enemyCells = GetPlayersCells(weakestID);
        if (enemyCells.Count == 0)
            return null;
        int weakestCellID = 0;
        for (int i = 1; i < enemyCells.Count; i++)
            if (enemyCells[i].currentVirusCount < enemyCells[weakestCellID].currentVirusCount)
                weakestCellID = i;
        return enemyCells[weakestCellID];
    }

    List<Cell> GetOwnCells()
    {
        List<Cell> myCells = new List<Cell>();
        for(int i = 0; i < allCells.Length; i++)
        {
            if (allCells[i].ownerID == controlledID)
                myCells.Add(allCells[i]); 
        }
        return myCells;
    }

    List<Cell> GetRandomCellsForSelection(List<Cell> myCells)
    {
        List<Cell> selectedCells = new List<Cell>();
        int iterationCount = totalCellSelection;
        if (totalCellSelection > myCells.Count)
            iterationCount = myCells.Count;
        for (int i = 0; i < iterationCount; i++)
        {
            int r = Random.Range(0, myCells.Count);
            selectedCells.Add(myCells[r]);
            myCells.RemoveAt(r);
        }
        return selectedCells;
    }

    List<Cell> GetStrongestCellsForSelection(List<Cell> myCells)
    {
        List<Cell> selectedCells = new List<Cell>();
        int iterationCount = totalCellSelection;
        if (totalCellSelection > myCells.Count)
            iterationCount = myCells.Count;
        for (int i = 0; i < iterationCount; i++)
        {
            int strongestIndex = 0;
            for (int j = 1; j < myCells.Count; j++)
            {
                if (myCells[j].currentVirusCount > myCells[strongestIndex].currentVirusCount)
                    strongestIndex = j;
            }
            selectedCells.Add(myCells[strongestIndex]);
            myCells.RemoveAt(strongestIndex);
        }
        return selectedCells;
    }

    List<Cell> GetClosestCellsForSelection(List<Cell> myCells, Cell targetCell)
    {
        List<Cell> selectedCells = new List<Cell>();
        int iterationCount = totalCellSelection;
        if (totalCellSelection > myCells.Count)
            iterationCount = myCells.Count;
        for (int i = 0; i < iterationCount; i++)
        {
            int closestIndex = 0;
            for (int j = 1; j < myCells.Count; j++)
            {
                if ((myCells[j].transform.position - targetCell.transform.position).sqrMagnitude < 
                    (myCells[closestIndex].transform.position - targetCell.transform.position).sqrMagnitude)
                    closestIndex = j;
            }
            selectedCells.Add(myCells[closestIndex]);
            myCells.RemoveAt(closestIndex);
        }
        return selectedCells;
    }

    void AttackEnemy()
    {
        Cell targetCell = null;
        if (targetingPriority == 0)
            targetCell = GetRandomEnemyCell();
        else if (targetingPriority == 1)
            targetCell = GetWeakestEnemyCell();
        else if (targetingPriority == 2)
            targetCell = GetRandomCellFromWeakestEnemy();
        else if (targetingPriority == 3)
            targetCell = GetWeakestCellFromWeakestEnemy();
        if (targetCell == null)
            return;
        List<Cell> myCells = GetOwnCells();
        if (selectingPriority == 0)
        {
            myCells = GetRandomCellsForSelection(myCells);
            foreach (Cell cell in myCells)
                cell.ReleaseVirusToTarget(targetCell);
        }
        else if (selectingPriority == 1)
        {
            myCells = GetStrongestCellsForSelection(myCells);
            foreach (Cell cell in myCells)
                cell.ReleaseVirusToTarget(targetCell);
        }
        else if (selectingPriority == 2)
        {
            myCells = GetClosestCellsForSelection(myCells,targetCell);
            foreach (Cell cell in myCells)
                cell.ReleaseVirusToTarget(targetCell);
        }
    }

    void ConquerFreeCell()
    {
        List<Cell> myCells = GetOwnCells();
        Cell targetCell = null;
        if (conquerTargetingPriority == 0)
            targetCell = GetRandomFreeCell();
        else if (conquerTargetingPriority == 1)
            targetCell = GetWeakestFreeCell();
        else if (conquerTargetingPriority == 2)
            targetCell = GetClosestFreeCell(myCells[Random.Range(0,myCells.Count)]);

        if (conquerSelectingPriority == 0)
        {
            myCells = GetRandomCellsForSelection(myCells);
            foreach (Cell cell in myCells)
                cell.ReleaseVirusToTarget(targetCell);
        }
        else if (conquerSelectingPriority == 1)
        {
            myCells = GetStrongestCellsForSelection(myCells);
            foreach (Cell cell in myCells)
                cell.ReleaseVirusToTarget(targetCell);
        }
        else if (conquerSelectingPriority == 2)
        {
            myCells = GetClosestCellsForSelection(myCells, targetCell);
            foreach (Cell cell in myCells)
                cell.ReleaseVirusToTarget(targetCell);
        }


    }

    void Step()
    {
        float r = Random.value;

        if (r < aggressivePercent)
        {
            int freeCellCount = GetFreeCellCount();
            if ((freeCellCount > 0 && GetOwnedCellCount() >= minimumCellsToAggress)
                || freeCellCount == 0)
                AttackEnemy();
            else
                ConquerFreeCell();
        }
        else
        {
            int freeCellCount = GetFreeCellCount();
            if (freeCellCount > 0)
                ConquerFreeCell();
            else
                AttackEnemy();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (allCells.Length == 0)
        {
            RandomGameGenerator.RemakeObject(this.gameObject);
            return;
        }
        if (statistics.GetPlayerPower(controlledID) <= 0)
            Destroy(gameObject);
        timeTillNext -= Time.deltaTime;
        if(timeTillNext <= 0f)
        {
            timeTillNext = Random.Range(timeBetweenSteps.x, timeBetweenSteps.y);
            Step();
        }
    }
}
