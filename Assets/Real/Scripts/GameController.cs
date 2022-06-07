using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private List<Cell> selectedCells = new List<Cell>();
    private Cell targetCell = null;
    private Camera mainCamera;
    // Start is called before the first frame update
    
    void ChangeTargetCell(Cell newTargetCell)
    {
        if(targetCell != null)
        {
            bool found = false;
            for(int i = 0; i < selectedCells.Count;i++)
                if(selectedCells[i] == targetCell)
                {
                    found = true;
                    break;
                }
            if (!found)
                targetCell.BecomeUnselected();
        }
        targetCell = newTargetCell;
        targetCell.BecomeSelected();
    }

    void UnselectTargetCell()
    {
        if (targetCell != null)
        {
            bool found = false;
            for (int i = 0; i < selectedCells.Count; i++)
                if (selectedCells[i] == targetCell)
                {
                    found = true;
                    break;
                }
            if (!found)
                targetCell.BecomeUnselected();
        }
        targetCell = null;
    }

    void DoMousing()
    {

        if (Input.GetMouseButton(0))
        { // if left button pressed...
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Cell cell = hit.transform.GetComponent<Cell>();
                if (cell)
                {
                    if (cell.ownerID == 1)
                    {
                        AddCell(cell);
                    }
                    ChangeTargetCell(cell);
                }
                else UnselectTargetCell();

                
            }
        }
        else ReleaseCells();
    }
    void AddCell(Cell cell)
    {
        for(int i = 0; i < selectedCells.Count; i++)
        {
            if (selectedCells[i] == cell)
                return;
        }
        selectedCells.Add(cell);
        cell.BecomeSelected();
    }
    
    void VerifySelectedCells()
    {
        for (int i = selectedCells.Count - 1; i >=0; i--)
        {
            if (selectedCells[i].ownerID != 1)
            {
                selectedCells[i].BecomeUnselected();
                selectedCells.RemoveAt(i);
            }
        }
    }
    void ReleaseCells()
    {
        for (int i = selectedCells.Count - 1; i >= 0; i--)
        {
            if (selectedCells[i].ownerID == 1)
                if(targetCell && targetCell != selectedCells[i])
                {
                    selectedCells[i].ReleaseVirusToTarget(targetCell);
                }
            selectedCells[i].BecomeUnselected();
            selectedCells.RemoveAt(i);
        }
        UnselectTargetCell();
    }
    
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        VerifySelectedCells();
        DoMousing();
    }
}
